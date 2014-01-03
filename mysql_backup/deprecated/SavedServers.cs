using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace mysql_backup
{
    [Obsolete]
    public static class SavedServers
    {
        public class ServerDetails
        {
            public string serverName;
            public string serverAddress;
            public string serverDatabase;
            public string serverUsername;
            public string serverPassword;

            public bool autoBackup=false;      //enable auto backup?
            public int backupHour=7;
            public int backupMinute = 0;
            public DateTime nextBackupTime; //time of next due backup

            public volatile bool inUse = false;

            public void updateNextBackup()
            {
                nextBackupTime = DateTime.Today.AddHours(backupHour);
                while (nextBackupTime < DateTime.Now)
                {
                    nextBackupTime = nextBackupTime.AddDays(1);
                }
                EventLog.LogEvent(string.Format("Database  {0} next backup time updated to {1}", serverName, nextBackupTime.ToString()), true);
            }
        }
        public static  List<ServerDetails> serverList = new List<ServerDetails>();

        public static int loadServers()
        {
            try
            {
                XmlTextReader xmlFile = new XmlTextReader(ProgramSettings.databasesFileName);
                ServerDetails details = new ServerDetails();
                while (xmlFile.Read())
                {
                    if (xmlFile.IsStartElement())
                    {
                        switch (xmlFile.Name)
                        {
                            case "server":
                                details = new ServerDetails();
                                break;
                            case "label":
                                details.serverName = xmlFile.ReadString();
                                break;
                            case "address":
                                details.serverAddress = xmlFile.ReadString();
                                break;
                            case "database":
                                details.serverDatabase = xmlFile.ReadString();
                                break;
                            case "username":
                                details.serverUsername = xmlFile.ReadString();
                                break;
                            case "password":
                                details.serverPassword = xmlFile.ReadString();
                                break;
                            case "auto_backup":
                                details.autoBackup = bool.Parse(xmlFile.ReadString());
                                break;
                            case "next_backup":
                                details.nextBackupTime = DateTime.Parse(xmlFile.ReadString());
                                break;
                            case "hour_backup":
                                details.backupHour = int.Parse(xmlFile.ReadString());
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (xmlFile.Name == "server" && !string.IsNullOrEmpty(details.serverName))
                        {
                            serverList.Add(details);
                        }
                    }
                }
                xmlFile.Close();
            }
            catch
            {
                return 0;
            }
            return 0;
        }

        public static bool saveServers()
        {
            try
            {
                XmlTextWriter xmlFile = new XmlTextWriter(ProgramSettings.databasesFileName, null);
                xmlFile.Formatting = Formatting.Indented;
                xmlFile.WriteStartDocument();
                //xmlFile.WriteComment("This file contains list of saved mysql servers for backup purposes.");
                xmlFile.WriteStartElement("servers");
                foreach (ServerDetails details in serverList)
                {
                    xmlFile.WriteStartElement("server");
                    xmlFile.WriteElementString("label", details.serverName);
                    xmlFile.WriteElementString("address", details.serverAddress);
                    xmlFile.WriteElementString("database", details.serverDatabase);
                    xmlFile.WriteElementString("username", details.serverUsername);
                    xmlFile.WriteElementString("password", details.serverPassword);
                    xmlFile.WriteElementString("auto_backup", details.autoBackup.ToString());
                    xmlFile.WriteElementString("next_backup", details.nextBackupTime.ToString());
                    xmlFile.WriteElementString("hour_backup", details.backupHour.ToString());
                    xmlFile.WriteEndElement();
                }
                xmlFile.WriteEndElement();
                xmlFile.WriteEndDocument();
                xmlFile.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "ERROR", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        public static bool isInServerList(string name)
        {
            foreach(ServerDetails server in serverList)
            {
                if (server.serverName == name) return true;
            }
            return false;
        }

        public static int getServerIdByName(string name)
        {
            foreach (ServerDetails server in serverList)
            {
                if (server.serverName == name) return serverList.IndexOf(server);
            }
            return -1;
        }

        public static bool addServer(string name, string address, string username, string password, string database = "", bool autoBackup=false, int backupTime=7)
        {
            if (isInServerList(name)) return false;
            
            ServerDetails tmpServer = new ServerDetails();

            tmpServer.serverName        = name;
            tmpServer.serverAddress     = address;
            tmpServer.serverDatabase    = database;
            tmpServer.serverUsername    = username;
            tmpServer.serverPassword    = password;
            tmpServer.autoBackup        = autoBackup;
            tmpServer.backupHour        = backupTime;
            tmpServer.updateNextBackup();

            serverList.Add(tmpServer);
            EventLog.LogEvent("Database added: " + name);
            return saveServers();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace mysql_backup
{
    static class ProgramSettings
    {
        public static string backupPath = @"backups";
        public static int MaxFileSize = 1024 * 1024 * 90; //max size in bytes

        public static int autoBackupInterval = 10 * 1000;
        public const string programTitle = "MySQL backup";
        public const string databasesFileName = "databases.xml";
        public const string settingsFileName = "settings.xml";

        public static bool startMinimized; //runtime global var

        public static bool saveSettings()
        {
            try
            {

                XmlTextWriter writer = new XmlTextWriter(settingsFileName, null);
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();

                writer.WriteStartElement("settings");
                    writer.WriteElementString("backup_path", backupPath);
                    writer.WriteElementString("max_file_size", MaxFileSize.ToString());
                    writer.WriteElementString("auto_backup_interval", autoBackupInterval.ToString());
                writer.WriteEndElement();

                writer.WriteEndDocument();
                writer.Close();
            }
            catch
            {
                EventLog.LogEvent("Failed to save settings");
                return false;
            }
            return true;
        }

        public static bool loadSettings()
        {
            try
            {
                XmlTextReader reader = new XmlTextReader(settingsFileName);
                string val;
                while (reader.Read())
                {
                    switch (reader.Name)
                    {
                        case "backup_path":
                            backupPath = reader.ReadString();
                            EventLog.LogEvent("Path read from file: " + backupPath, true);
                            break;
                        case "max_file_size":
                            val = reader.ReadString();
                            if (int.TryParse(val, out MaxFileSize))
                            {
                                EventLog.LogEvent("Max filesize loaded: " + MaxFileSize.ToString(), true);
                            }
                            else
                            {
                                EventLog.LogEvent("ERROR: Max filesize cannot be loaded from file! "+val+" cannot be parsed to int. Current value: "+MaxFileSize.ToString());
                            }
                            break;
                        case "auto_backup_interval":
                            val = reader.ReadString();
                            if (int.TryParse(val, out autoBackupInterval))
                            {
                                EventLog.LogEvent("Backup interval loaded: " + autoBackupInterval.ToString(), true);
                            }
                            else
                            {
                                EventLog.LogEvent("ERROR: Backup interval cannot be loaded from file! " + val + " cannot be parsed to int. Current value: " + autoBackupInterval.ToString());
                            }
                            break;
                           
                    }
                }
                reader.Close();
            }
            catch
            {
                EventLog.LogEvent("Settings could not be loaded. Default settings assumed.");
                return false;
            }
            return true;
        }
    }
}

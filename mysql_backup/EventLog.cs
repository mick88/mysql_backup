using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace mysql_backup
{
    static class EventLog
    {
        private const string LOG_FILE_NAME = "log.txt";
        public static bool LogEvent(string text, bool debugOnly=false)
        {
            try
            {
                if (debugOnly == true)
                {
#if (!DEBUG)
                        return false;
#endif
                    StreamWriter logFile = new StreamWriter(LOG_FILE_NAME, true);
                    logFile.WriteLine(string.Format("{0}\t\t(DEBUG) {1}", DateTime.Now, text));
                    logFile.Close();
                }
                else
                {
                    StreamWriter logFile = new StreamWriter(LOG_FILE_NAME, true);
                    logFile.WriteLine(string.Format("{0}\t\t{1}", DateTime.Now, text));
                    logFile.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace mysql_backup
{
    class RegistryAccess
    {
        private const string startupName = "MySQL Backup utility";
        private static string startupPath = "\""+Application.ExecutablePath.ToString()+"\" -startup";

        public static void setStartup(CheckState state)
        {
            if (state == CheckState.Indeterminate) return;
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (state == CheckState.Checked) key.SetValue(startupName, startupPath);
            else key.DeleteValue(startupName, false);
        }

        public static void checkStartup(out CheckState state)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false);
                string path = key.GetValue(startupName, "").ToString();

                if (path == "") state = CheckState.Unchecked;
                else if (path == startupPath) state = CheckState.Checked;
                else state = CheckState.Indeterminate;
            }
            catch
            {
                state = CheckState.Indeterminate;
            }
        }
    }
}

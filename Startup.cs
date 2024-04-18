using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoot
{

    public class ProcessData
    {
        public string appName { get; set; }
        public string appPath { get; set; }
    }

    internal class Startup
    {
        private static ProcessData GetProcessData()
        {
            var currentProcess = Process.GetCurrentProcess();
            string appName = currentProcess.ProcessName;
            string appPath = currentProcess.MainModule.FileName;

            return new ProcessData { appName = appName, appPath = appPath };
        }

        public static bool IsAppSetToRunOnStartup()
        {
            // Open the registry key for current user startup programs
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
            {
                // Check if the application path exists in the registry
                if (key != null)
                {
                    var info = GetProcessData();
                    string value = (string)key.GetValue(info.appName);
                    return value != null && value.Equals(info.appPath, StringComparison.OrdinalIgnoreCase);
                }
            }
            return false;
        }

        public static void SetAppToRunOnStartup()
        {
            // Open the registry key for current user startup programs
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
            {
                var info = GetProcessData();

                // Add the application path to the registry
                key.SetValue(info.appName, info.appPath);
            }
        }

    }
}

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FetchUploadTool
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SetAutoStart(false);


            Application.Run(new Form1());
        }


        private static void SetAutoStart(bool enable)
        {
            const string appName = "AutoUploadTool.exe"; // 替换为你的应用程序名称
            string executablePath = Application.ExecutablePath;

            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (enable)
            {
                registryKey.SetValue(appName, executablePath);
                // 可以添加启动时的其他逻辑
            }
            else
            {
                registryKey.DeleteValue(appName, false);
                // 可以添加关闭时的其他逻辑
            }
        }
    }


}

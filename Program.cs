using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMTUploadTool
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

            // 获取程序路径
            string assemblyPath = Assembly.GetExecutingAssembly().Location;

            // 设置注册表项的键名
            const string keyName = "SMTUploadTool";

            // 设置注册表项的值名
            const string valueName = "SMTUploadToolPath";

            // 判断是否已经添加到注册表中
            if (!IsProgramRegistered(keyName, assemblyPath))
            {
                // 添加到注册表中
                RegisterProgram(keyName, valueName, assemblyPath);
                RunAsAdmin(assemblyPath);
            }

            // 确保以管理员身份运行程序
            

            Application.Run(new Form1());
        }

        static bool IsProgramRegistered(string keyName, string assemblyPath)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
            {
                return key.GetValue(keyName) != null && key.GetValue(keyName).ToString() == assemblyPath;
            }
        }

        static void RegisterProgram(string keyName, string valueName, string assemblyPath)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
            {
                key.SetValue(keyName, assemblyPath);
            }
        }

        static void RunAsAdmin(string assemblyPath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = assemblyPath,
                Verb = "runas" // "runas"表示以管理员身份运行
            };

            try
            {
                Process.Start(startInfo);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                // 用户取消了UAC提示
            }
        }
    }

}




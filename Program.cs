using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMTUploadTool
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        static Mutex mutex = new Mutex(true, "{E1CB36E2-41A5-4E35-9ED2-3D96DE7D9CB7}");
        [STAThread]
        static void Main()
        {
            //if (mutex.WaitOne(TimeSpan.Zero, true))
            //{
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // 
                string assemblyPath = Assembly.GetExecutingAssembly().Location;

                
                const string keyName = "SMTUploadTool";

                
                const string valueName = "SMTUploadToolPath";

               
                if (!IsProgramRegistered(keyName, assemblyPath))
                {
                    
                    RegisterProgram(keyName, valueName, assemblyPath);
                    RunAsAdmin(assemblyPath);
                }
                else
                {
                    Application.Run(new Form1());
                }
                Console.WriteLine("Hello World!");
            

                

                
          
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




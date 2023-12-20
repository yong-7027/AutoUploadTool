using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Win32;
using IWshRuntimeLibrary;

public struct ToolSetting
{
    public string monitorFolderPath;
    public string targetFileName;
    public string destinateFolderPath;
    public string line; //e.g. "L1"
    public string LogFilePath;
}
struct LogInfo
{
    public string actionTime;
    public string status;
    public string year;
    public string month;
    public string day;
    public string folderName;
    public string fileName;
    public string filePath;
    public long fileSize;
    public string destinationPath;
    public string destinationFolderName;
}

namespace FetchUploadTool
{
    
    public partial class Form1 : Form
    {
        private NotifyIcon notifyIcon;
        ToolSetting defaultSettings = new ToolSetting {
            monitorFolderPath = "",
            targetFileName = "",
            destinateFolderPath = "",
            line = "L0",
            //get current path
            LogFilePath = System.Environment.CurrentDirectory + @"\Log.txt"
        };
        ToolSetting toolSetting = new ToolSetting();
        string binDataFilePath = "settings.bin";
        

        private FileSystemWatcher watcher;
        Boolean initializaion;
        
        //List<LogInfo> logList;
        


        public Form1()
        { 
            InitializeComponent();
            this.FormClosing += MainForm_FormClosing;
            InitializeTrayIcon();
            // check if the binary file exist, if not, create one
            if (!System.IO.File.Exists(binDataFilePath))
            {
                // write default settings to binary file
                WriteStructToBinaryFile(binDataFilePath, defaultSettings);
            }
            SystemEvents.SessionEnding += SystemEvents_SessionEnding;
            // read settings from binary file
            toolSetting = ReadStructFromBinaryFile(binDataFilePath);
            /*
            if (!System.IO.File.Exists("Log.bin"))
            {
                using (FileStream fs = new FileStream("Log.bin", FileMode.Create, FileAccess.Write))
                {
                    // 
                }
            }
            */

            //InitializeTrayIcon();


            string appName = "FetchUploadTool";  // 替换为你的应用程序名称
            string executablePath = AppDomain.CurrentDomain.BaseDirectory + "AutoUploadTool.exe";  // 获取应用程序的可执行文件路径

            string startupFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string shortcutPath = Path.Combine(startupFolderPath, $"{appName}.lnk");

            CreateShortcut(shortcutPath, executablePath);
        }

        private void btnMonitorFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                // dialog description
                folderBrowserDialog.Description = "Select Folder Location that you want to monitor";

                // show dialog and get user operation result
                DialogResult result = folderBrowserDialog.ShowDialog();

                // check if user click "OK" button
                if (result == DialogResult.OK)
                {
                    // get user selected folder path and show in text box
                    //toolSetting.monitorFolderPath = folderBrowserDialog.SelectedPath;
                    //txtBoxMonitorFolder.Text = toolSetting.monitorFolderPath;   
                    
                    txtBoxMonitorFolder.Text=folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            CenterToScreen();
            //MessageBox.Show(toolSetting.LogFilePath);
            //string content = reader.ReadToEnd();
            //txtBoxMonitorFolder.Text = content;
            if (!System.IO.File.Exists(toolSetting.LogFilePath))
            {
                using (FileStream fs = new FileStream(toolSetting.LogFilePath, FileMode.Create, FileAccess.Write))
                {
                    // 
                }
            }
            // write log to txt file
            using (StreamWriter sw = new StreamWriter(toolSetting.LogFilePath, true))
            {
                sw.WriteLine(" ");
                sw.WriteLine("App open  time: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            }



            txtBoxMonitorFolder.Text = toolSetting.monitorFolderPath;
            txtTargetFileName.Text = toolSetting.targetFileName;
            txtDestinationFolder.Text = toolSetting.destinateFolderPath;
            numericUpDownLine.Value = ExtractNumberFromLine(toolSetting.line);
            txtLogPath.Text = toolSetting.LogFilePath;
            btnStop.Enabled = false;
            btnStop.BackColor = Color.Gray;

            linkPlantListFile.Enabled=false;
            //keep the link underlined
            
            
            

            btnLogPath.Enabled = false;
            
            numericUpDownLine.Enabled = false;
            numericUpDownLine.ReadOnly = true;

            txtBoxMonitorFolder.ReadOnly = true;
            txtBoxMonitorFolder.Enabled = false;
            btnMonitorFolder.Enabled = false;

            txtTargetFileName.ReadOnly = true;
            txtTargetFileName.Enabled = false;       

            txtDestinationFolder.ReadOnly = true;
            txtDestinationFolder.Enabled = false;
            btnDestinationFolder.Enabled = false;

            btnChangeSetting.Enabled = true;
            btnApply.Enabled = false;
            btnCancelSetting.Enabled = false;

            if(toolSetting.monitorFolderPath == "" || toolSetting.targetFileName == "" || toolSetting.destinateFolderPath == "")
            {
                btnStart.Enabled = false;
                btnStart.BackColor = Color.Gray;
                initializaion = false;
                btnChangeSetting.Focus();
                
            }
            else
            {
                
                btnStart.Enabled = true;
                btnStart.BackColor = Color.Green;
                initializaion = true;
                btnStart.PerformClick();
            }
            btnApply.Hide();
            btnCancelSetting.Hide();


            // read loglist
            /*
            if (!System.IO.File.Exists("Log.bin"))
            {
                using (FileStream fs = new FileStream("Log.bin", FileMode.Create, FileAccess.Write))
                {
                    // 
                }
            }
            logList = ReadLogFromBinaryFile("log.bin");
            */

            // create a txt file to record the log
            // create text file if not exist
           
            

        }
        

        // write the whole struct to binary file
        static void WriteStructToBinaryFile(string filePath, ToolSetting data)
        {
            using (BinaryWriter writer = new BinaryWriter(System.IO.File.Open(filePath, FileMode.Create)))
            {
                // write the whole struct to binary file
                writer.Write(data.monitorFolderPath);
                writer.Write(data.targetFileName);
                writer.Write(data.destinateFolderPath);
                writer.Write(data.line);
                writer.Write(data.LogFilePath);
            }
        }

        // read the whole struct from binary file
        static ToolSetting ReadStructFromBinaryFile(string filePath)
        {
            ToolSetting data = new ToolSetting();

            using (BinaryReader reader = new BinaryReader(System.IO.File.Open(filePath, FileMode.Open)))
            {
                // read the whole struct from binary file
                data.monitorFolderPath = reader.ReadString();
                data.targetFileName = reader.ReadString();
                data.destinateFolderPath = reader.ReadString();
                data.line = reader.ReadString();
                data.LogFilePath = reader.ReadString();
            }

            return data;
        }

        private void txtTargetFileName_TextChanged(object sender, EventArgs e)
        {
            
           
        }

        private void btnDestinationFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                // set dialog description
                folderBrowserDialog.Description = "Select the location of Destination Folder";

                // show dialog and get user operation result
                DialogResult result = folderBrowserDialog.ShowDialog();

                // check if user click "OK" button
                if (result == DialogResult.OK)
                {
                    // get user selected folder path and show in text box
                    //toolSetting.destinateFolderPath = folderBrowserDialog.SelectedPath;
                    //txtDestinationFolder.Text = toolSetting.destinateFolderPath;

                    txtDestinationFolder.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void txtDestinationFolder_TextChanged(object sender, EventArgs e)
        {
            //toolSetting.destinateFolderPath = txtDestinationFolder.Text;
            
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // set the folder path to monitor
            //string folderPathToMonitor = @"C:\Users\tcy\Desktop\checkFolder";

            // create a new FileSystemWatcher and set its properties
            watcher = new FileSystemWatcher(toolSetting.monitorFolderPath);

            // monitor all changes in the folder including subfolders
            watcher.IncludeSubdirectories = true;

            // set the file type to monitor
            watcher.NotifyFilter = NotifyFilters.DirectoryName;

            // event handlers when a new folder is created
            watcher.Created += OnFolderCreated;

            // start to monitor
            watcher.EnableRaisingEvents = true;

            //Console.WriteLine($"Monitoring folder: {toolSetting.monitorFolderPath}");
            //MessageBox.Show($"Monitoring folder: {toolSetting.monitorFolderPath}");
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            btnStop.BackColor = Color.Red;
            btnStart.BackColor = Color.Gray;

            btnChangeSetting.Enabled = false;


            if (!System.IO.File.Exists(toolSetting.LogFilePath))
            {
                using (FileStream fs = new FileStream(toolSetting.LogFilePath, FileMode.Create, FileAccess.Write))
                {
                    // 
                }
            }
            // write log to txt file
            using (StreamWriter sw = new StreamWriter(toolSetting.LogFilePath, true))
            {
                sw.WriteLine("-------------------------------------------------------------------------------------------------------------------------");
                sw.WriteLine("App \"START\" time: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            // stop monitoring
            if (watcher != null)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
                Console.WriteLine("Monitoring stopped.");
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                btnStop.BackColor = Color.Gray;
                btnStart.BackColor = Color.Green;
               
            }
            btnChangeSetting.Enabled = true;
            if (!System.IO.File.Exists(toolSetting.LogFilePath))
            {
                using (FileStream fs = new FileStream(toolSetting.LogFilePath, FileMode.Create, FileAccess.Write))
                {
                    // 
                }
            }
            // write log to txt file
            using (StreamWriter sw = new StreamWriter(toolSetting.LogFilePath, true))
            {
                sw.WriteLine("App \"STOP\" time: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                sw.WriteLine("-------------------------------------------------------------------------------------------------------------------------");
            }
        }
        /*
        private static async void OnFolderCreated(object sender, FileSystemEventArgs e)
        {
            ToolSetting toolSetting = new ToolSetting();
            string binDataFilePath = "settings.bin";
            toolSetting = ReadStructFromBinaryFile(binDataFilePath);

            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                


                // execute the function you want to trigger here
                //Console.WriteLine($"New folder created: {e.FullPath}");
                MessageBox.Show($"New folder created: {e.FullPath}");

                //get folder name
                string folderName = Path.GetFileName(e.FullPath);
                //Console.WriteLine(folderName);

                //get floder's all file
                string[] files = Directory.GetFiles(e.FullPath);
                //get file name
                //string fileName = Path.GetFileName(files[0]);

                //seach file, find and copy the insp_pad.txt and change file name then paste to specify folder
                foreach (string file in files)
                {
                    //Console.WriteLine(file);
                    if (string.Equals(Path.GetFileName(file), toolSetting.targetFileName, StringComparison.OrdinalIgnoreCase))
                    {
                        string fileName = Path.GetFileName(file);
                        // cut the "." and words after the "." of fileName
                        fileName = fileName.Substring(0, fileName.IndexOf("."));


                        //Console.WriteLine(file);
                        string newFileName = fileName + "_" + folderName + ".txt";
                        string newFilePath = toolSetting.destinateFolderPath + @"\"  + newFileName;
                        System.IO.File.Copy(file, newFilePath,true);
                        Console.WriteLine($"New file created: {newFilePath}");
                    }
                }
            }
        }
        
        /*
        private static async void OnFolderCreated(object sender, FileSystemEventArgs e)
        {
            ToolSetting toolSetting = new ToolSetting();
            string binDataFilePath = "settings.bin";
            toolSetting = ReadStructFromBinaryFile(binDataFilePath);

            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                // execute the function you want to trigger here
                // Console.WriteLine($"New folder created: {e.FullPath}");
                MessageBox.Show($"New folder created: {e.FullPath}");

                // get folder name
                string folderName = Path.GetFileName(e.FullPath);

                // loop until the folder is created and contains the target file
                while (!Directory.Exists(e.FullPath) || !Directory.GetFiles(e.FullPath).Any(file =>
                    string.Equals(Path.GetFileName(file), toolSetting.targetFileName, StringComparison.OrdinalIgnoreCase)))
                {
                    await Task.Delay(1000); // every second check once, can be adjusted according to the actual situation
                }

                // get folder's all files
                string[] files = Directory.GetFiles(e.FullPath);

                // search file, find and copy the insp_pad.txt and change file name then paste to specify folder
                foreach (string file in files)
                {
                    if (string.Equals(Path.GetFileName(file), toolSetting.targetFileName, StringComparison.OrdinalIgnoreCase))
                    {
                        string fileName = Path.GetFileName(file);
                        // cut the "." and words after the "." of fileName
                        fileName = fileName.Substring(0, fileName.IndexOf("."));

                        string newFileName = fileName + "_" + folderName + ".txt";
                        string newFilePath = toolSetting.destinateFolderPath + @"\" + newFileName;
                        System.IO.File.Copy(file, newFilePath, true);
                        Console.WriteLine($"New file created: {newFilePath}");
                }
                    }
            }
        }
        */
        private static async void OnFolderCreated(object sender, FileSystemEventArgs e)
        {
            ToolSetting toolSetting = new ToolSetting();
            string binDataFilePath = "settings.bin";
            toolSetting = ReadStructFromBinaryFile(binDataFilePath);

            if (e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Renamed)
            {
                

                // loop until the folder is the target file
                while (!Directory.GetFiles(e.FullPath).Any(file =>
                    string.Equals(Path.GetFileName(file), toolSetting.targetFileName, StringComparison.OrdinalIgnoreCase)))
                {
                    await Task.Delay(1); // every ? second check once, can be adjusted according to the actual situation
                }

                // get folder's all files
                string[] files = Directory.GetFiles(e.FullPath);

                // search file, find and copy the insp_pad.txt and change file name then paste to specify folder
                foreach (string file in files)
                {
                    if (string.Equals(Path.GetFileName(file), toolSetting.targetFileName, StringComparison.OrdinalIgnoreCase))
                    {
                        //check if the file content is empty, if empty, refresh the file
                        FileInfo fileInfo = new FileInfo(file);
                        if(fileInfo.Length == 0)
                        {
                            while (fileInfo.Length == 0)
                            {
                                //await Task.Delay(1);
                                fileInfo.Refresh();
                                //if time is more than 10s, break the loop



                            }
                        }


                        // get model
                        FileInfo fileInfo1 = new FileInfo(file);
                        string model = "";
                        if (fileInfo1.Length != 0)
                        {
                            //MessageBox.Show(fileInfo1.Length.ToString());
                            model = FindAndExtract(file, "CTM|");
                        }
                        else
                        {
                            model = "EmptyData";
                        }
                        //MessageBox.Show(e.FullPath);
                        string fileName = Path.GetFileName(file);
                        // cut the "." and words after the "." of fileName
                        fileName = fileName.Substring(0, fileName.IndexOf("."));

                        string newFileName = toolSetting.line+"_"+model + "_" + Path.GetFileName(e.FullPath) + ".txt";

                        //////////////////////////////
                        ///
                        string plantFilePath = "PlantList.txt";  // 替换为你的文本文件路径
                        string newFilePath="";
                        // 检查Plants文件是否存在
                        if (System.IO.File.Exists(plantFilePath))
                        {
                            // 读取每一行
                            string[] plants = System.IO.File.ReadAllLines(plantFilePath);
                            
                            string newFolderPath;
                            Boolean plantExist = false;
                            foreach (string plant in plants)
                            {
                                if (model.Contains(plant))
                                {
                                    //create new folder in destination folder
                                    newFolderPath = toolSetting.destinateFolderPath + @"\" + plant;
                                    if (!Directory.Exists(newFolderPath))
                                    {
                                        Directory.CreateDirectory(newFolderPath);
                                    }
                                    // new file path
                                    newFilePath = newFolderPath + @"\" + newFileName;
                                    System.IO.File.Copy(file, newFilePath, true);
                                    plantExist = true;
                                    break;
                                }  
                            }
                            if(plantExist == false)
                            {
                                //newFolderPath = toolSetting.destinateFolderPath + @"\other";
                                //newFilePath = newFolderPath + @"\" + newFileName;
                                newFilePath = toolSetting.destinateFolderPath + @"\" + newFileName;
                                System.IO.File.Copy(file, newFilePath, true);
                            }
                            
                            
                        }
                        else
                        {
                            newFilePath = toolSetting.destinateFolderPath + @"\" + newFileName;
                            System.IO.File.Copy(file, newFilePath, true);
                        }

                       
                        

                        //Console.WriteLine($"New file created: {newFilePath}");

                        // log section
                        //List<LogInfo> loglist = ReadLogFromBinaryFile("Log.bin");

                        
                        //write the log
                        LogInfo log = new LogInfo();
                        //get the time
                        log.actionTime = DateTime.Now.ToString("HH:mm:ss");
                        log.year = DateTime.Now.ToString("yyyy");
                        log.month = DateTime.Now.ToString("MM");
                        log.day = DateTime.Now.ToString("dd");
                        log.fileName = newFileName;
                        log.filePath = e.FullPath;
                        //log.status = "Successful";
                        //if new file is sucessfully copy to destination status is "Successful",else status is failed
                        if (System.IO.File.Exists(newFilePath))
                        {
                            log.status = "Successful";
                        }
                        else
                        {
                            log.status = "Failed";
                        }

                        log.folderName = Path.GetFileName(e.FullPath);
                        
                        log.fileSize = fileInfo1.Length;
                        log.destinationPath = newFilePath;
                        log.destinationFolderName= Path.GetFileName(toolSetting.destinateFolderPath);


                        //loglist.Add(log);

                        //WriteLogToBinaryFile("Log.bin", loglist);
                        

                        // create a txt file to record the log
                        // create text file if not exist
                        if (!System.IO.File.Exists(toolSetting.LogFilePath))
                        {
                            using (FileStream fs = new FileStream(toolSetting.LogFilePath, FileMode.Create, FileAccess.Write))
                            {
                                // 
                            }
                        }
                        // write log to txt file
                        using (StreamWriter sw = new StreamWriter(toolSetting.LogFilePath, true))
                        {
                            //sw.WriteLine($"Time: {log.actionTime}, Status: {log.status}, Year: {log.year}, Month: {log.month}, Day: {log.day}, Folder Name: {log.folderName}, File Name: {log.fileName}, File Path: {log.filePath}, File Size: {log.fileSize}, Destination Path: {log.destinationPath}, Destination Folder Name: {log.destinationFolderName}");
                            sw.WriteLine("Date : " + log.year + "/" + log.month + "/" + log.day + ", Time: " + log.actionTime +", Line:"+toolSetting.line+", Model:"+model+ ", File Name: " + log.fileName + ", File Size: " + log.fileSize + "(bytes)" + ", Status: " + log.status + ", From Folder: " + log.folderName + ", To Destination Folder: " + log.destinationFolderName);
                            //sw.WriteLine(" ");
                            //sw.WriteLine("-------------------------------------------------------------------------------------------------------------------------");
                        }

                        

                    }
                }
            }
        }


       /*
        private void btnClose_Click(object sender, EventArgs e)
        {
            // stop monitoring
            if (watcher != null)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
            
            this.Close();
        }
       */
        private void btnChangeSetting_Click(object sender, EventArgs e)
        {
            linkPlantListFile.Enabled = true;
            btnLogPath.Enabled = true;

            numericUpDownLine.Enabled = true;
            numericUpDownLine.ReadOnly = false;

            txtBoxMonitorFolder.ReadOnly = false;
            txtBoxMonitorFolder.Enabled = true;
            btnMonitorFolder.Enabled = true;

            txtTargetFileName.ReadOnly = false;
            txtTargetFileName.Enabled = true;

            txtDestinationFolder.ReadOnly = false;
            txtDestinationFolder.Enabled = true;
            btnDestinationFolder.Enabled = true;

            btnStart.Enabled = false;
            btnStop.Enabled = false;
            btnStart.BackColor = Color.Gray;
            btnStop.BackColor = Color.Gray;

            btnChangeSetting.Enabled = false;
            btnApply.Enabled = true;
            btnCancelSetting.Enabled = true;

            btnApply.Show();
            btnCancelSetting.Show();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {

            if (!Directory.Exists(txtBoxMonitorFolder.Text))
            {
                MessageBox.Show("Please enter a valid Monitor folder path!");
                return;
            }
            else if (!Directory.Exists(txtDestinationFolder.Text))
            {
                MessageBox.Show("Please enter a valid Destination folder path!");
                return;
            }
            else if (txtTargetFileName.Text == "")
            {
                MessageBox.Show("Please enter a valid Target file name!");
                return;
            }
            else if (numericUpDownLine.Value <= 0)
            {
                MessageBox.Show("Please enter a valid Line number!");
                return;
            }
            else if (txtLogPath.Text=="")
            {
                MessageBox.Show("Please enter a valid path!");
                return;
            }
            else
            {


                if (!System.IO.File.Exists(toolSetting.LogFilePath))
                {
                    using (FileStream fs = new FileStream(toolSetting.LogFilePath, FileMode.Create, FileAccess.Write))
                    {
                        // 
                    }
                }
                // write log to txt file
                using (StreamWriter sw = new StreamWriter(toolSetting.LogFilePath, true))
                {
                    sw.WriteLine(" ");
                    //sw.WriteLine("-------------------------------------------------------------------------------------------------------------------------");
                    sw.WriteLine("App \"Change Setting\" Time(" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")+")");
                    sw.WriteLine("Line Number        > From:" + toolSetting.line);
                    sw.WriteLine("                     To  :" + "L" + numericUpDownLine.Value.ToString());
                    sw.WriteLine("Monitor Folder     > From:" + toolSetting.monitorFolderPath);
                    sw.WriteLine("                     To  :" + txtBoxMonitorFolder.Text);
                    sw.WriteLine("Target File Name   > From:" + toolSetting.targetFileName);
                    sw.WriteLine("                     To  :" + txtTargetFileName.Text);
                    sw.WriteLine("Destination Folder > From:" + toolSetting.destinateFolderPath);
                    sw.WriteLine("                     To  :" + txtDestinationFolder.Text);
                    sw.WriteLine("Log File Path      > From:" + toolSetting.LogFilePath);
                    sw.WriteLine("                     To  :" + txtLogPath.Text);
                    
                    sw.WriteLine(" ");
                    //sw.WriteLine("-------------------------------------------------------------------------------------------------------------------------");
                }

                // if log file path changed, copy the old log file to new path
                if(toolSetting.LogFilePath != txtLogPath.Text)
                {
                    System.IO.File.Copy(toolSetting.LogFilePath, txtLogPath.Text, true);
                }
                




                toolSetting.monitorFolderPath = txtBoxMonitorFolder.Text;
                toolSetting.targetFileName = txtTargetFileName.Text;
                toolSetting.destinateFolderPath = txtDestinationFolder.Text;
                toolSetting.line = "L" + numericUpDownLine.Value.ToString();
                toolSetting.LogFilePath = txtLogPath.Text;
                initializaion = true;

                btnStart.PerformClick();

            }


            WriteStructToBinaryFile(binDataFilePath, toolSetting);
            toolSetting = ReadStructFromBinaryFile(binDataFilePath);

            btnLogPath.Enabled = false;
            linkPlantListFile.Enabled = false;

            numericUpDownLine.ReadOnly = true;
            numericUpDownLine.Enabled = false;

            txtBoxMonitorFolder.ReadOnly = true;
            txtBoxMonitorFolder.Enabled = false;
            btnMonitorFolder.Enabled = false;
            txtTargetFileName.ReadOnly = true;
            txtTargetFileName.Enabled = false;
            txtDestinationFolder.ReadOnly = true;
            txtDestinationFolder.Enabled = false;
            btnDestinationFolder.Enabled = false;
            btnChangeSetting.Enabled = true;
            btnStart.Enabled = true;
            btnStart.BackColor = Color.Green;
            
            btnChangeSetting.Enabled = true;
            btnApply.Enabled = false;
            btnCancelSetting.Enabled = false;

            btnCancelSetting.Hide();
            btnApply.Hide();
            btnStart.PerformClick();

            

        }

        private void txtBoxMonitorFolder_TextChanged(object sender, EventArgs e)
        {

            //toolSetting.monitorFolderPath = txtBoxMonitorFolder.Text;
            
        }

        private void btnCancelSetting_Click(object sender, EventArgs e)
        {
            toolSetting = ReadStructFromBinaryFile(binDataFilePath);
            linkPlantListFile.Enabled = false;
            
            btnLogPath.Enabled = false;
            numericUpDownLine.ReadOnly = true;
            numericUpDownLine.Enabled = false;

            txtBoxMonitorFolder.ReadOnly = true;
            txtBoxMonitorFolder.Enabled = false;
            btnMonitorFolder.Enabled = false;
            txtTargetFileName.ReadOnly = true;
            txtTargetFileName.Enabled = false;
            txtDestinationFolder.ReadOnly = true;
            txtDestinationFolder.Enabled = false;
            btnDestinationFolder.Enabled = false;
            btnChangeSetting.Enabled = true;
            btnStart.Enabled = true;
            btnStart.BackColor = Color.Green;
            
            btnChangeSetting.Enabled = true;
            btnApply.Enabled = false;
            btnCancelSetting.Enabled = false;

            txtBoxMonitorFolder.Text = toolSetting.monitorFolderPath;
            txtTargetFileName.Text = toolSetting.targetFileName;
            txtDestinationFolder.Text = toolSetting.destinateFolderPath;
            numericUpDownLine.Value = ExtractNumberFromLine(toolSetting.line);
            txtLogPath.Text = toolSetting.LogFilePath;
            if(initializaion == false)
            {
                btnStart.Enabled = false;
                btnStart.BackColor = Color.Gray;
            }
            else
            {
                btnStart.Enabled = true;
                btnStart.BackColor = Color.Green;
            }

            btnCancelSetting.Hide();
            btnApply.Hide();
            if(initializaion == true)
            {
                btnStart.PerformClick();
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void linkLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

        /*
        static void WriteLogToBinaryFile(string filePath, List<LogInfo> data)
        {
            using (BinaryWriter writer = new BinaryWriter(System.IO.File.Open(filePath, FileMode.Create)))
            {
                foreach (var log in data)
                {
                    // write 
                    writer.Write(log.actionTime);
                    writer.Write(log.status);
                    writer.Write(log.year);
                    writer.Write(log.month);
                    writer.Write(log.day);
                    writer.Write(log.folderName);
                    writer.Write(log.fileName);
                    writer.Write(log.filePath);
                    writer.Write(log.fileSize);
                    writer.Write(log.destinationPath);
                    writer.Write(log.destinationFolderName);
                }
            }
        }
        */
        /*
        static List<LogInfo> ReadLogFromBinaryFile(string filePath)
        {
            List<LogInfo> data = new List<LogInfo>();

            using (BinaryReader reader = new BinaryReader(System.IO.File.Open(filePath, FileMode.Open)))
            {
                // read
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    LogInfo log = new LogInfo
                    {
                        actionTime = reader.ReadString(),
                        status = reader.ReadString(),
                        year = reader.ReadString(),
                        month = reader.ReadString(),
                        day = reader.ReadString(),
                        folderName = reader.ReadString(),
                        fileName = reader.ReadString(),
                        filePath = reader.ReadString(),
                        fileSize = reader.ReadInt64(),
                        destinationPath = reader.ReadString(),
                        destinationFolderName = reader.ReadString()
                    };

                    // add to list
                    data.Add(log);
                }
            }

            return data;
        }
        */
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            // 当用户点击主窗体关闭按钮时，只隐藏而不关闭
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                ToggleMainWindow();
            }

            /*
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (!System.IO.File.Exists(toolSetting.LogFilePath))
                {
                    using (FileStream fs = new FileStream(toolSetting.LogFilePath, FileMode.Create, FileAccess.Write))
                    {
                        // 
                    }
                }
                // write log to txt file
                using (StreamWriter sw = new StreamWriter(toolSetting.LogFilePath, true))
                {
                    sw.WriteLine("App Close time: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    sw.WriteLine(" ");
                }
            }
            */
        }


        static int ExtractNumberFromLine(string line)
        {
            
            string numberStr = line.Substring(1);

            
            if (int.TryParse(numberStr, out int extractedNumber))
            {
                return extractedNumber;
            }
            return -1;
        }

        static string FindAndExtract(string filePath, string keyword)
        {
            try
            {
                // read all lines from file
                string[] lines = System.IO.File.ReadAllLines(filePath);

                // search keyword in each line
                foreach (string currentLine in lines)
                {
                    int startIndex = currentLine.IndexOf(keyword);
                    if (startIndex != -1)
                    {
                        // find the keyword, extract the number
                        int commaIndex = currentLine.IndexOf(",", startIndex);
                        if (commaIndex != -1)
                        {
                            return currentLine.Substring(startIndex + keyword.Length, commaIndex - (startIndex + keyword.Length));
                        }
                    }
                }

                
                return "ModelNotFound"; // 
            }
            catch (Exception ex)
            {
               
                return "Empty"; // 
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnLogPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                // dialog description
                folderBrowserDialog.Description = "Select Folder Location that you want to place Log file";

                // show dialog and get user operation result
                DialogResult result = folderBrowserDialog.ShowDialog();

                // check if user click "OK" button
                if (result == DialogResult.OK)
                {
                    //toolSetting.LogFilePath = folderBrowserDialog.SelectedPath+@"\Log.txt";
                    //txtLogPath.Text = toolSetting.LogFilePath;

                    txtLogPath.Text = folderBrowserDialog.SelectedPath+@"\Log.txt";
                }
            }
        }

        private void txtLogFilePath_change(object sender, EventArgs e)
        {
            
        }

        private void numericUpDownLine_ValueChanged(object sender, EventArgs e)
        {

        }

        private void InitializeTrayIcon()
        {
            // 创建系统托盘图标
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new Icon("icon.ico"); // 替换为你的图标文件路径
            notifyIcon.Text = "FetchUpload"; // 替换为你的应用程序名称
            notifyIcon.Visible = true;


            // 添加双击事件处理程序，用于显示/隐藏主窗体
            notifyIcon.DoubleClick += (sender, e) => ToggleMainWindow();

            // 创建右键菜单
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add("Exit", (sender, e) => ExitApplication());

            // 将右键菜单分配给系统托盘图标
            notifyIcon.ContextMenu = contextMenu;
        }

        private void ToggleMainWindow()
        {
            // 显示/隐藏主窗体
            if (WindowState == FormWindowState.Minimized)
            {
                Show();
                WindowState = FormWindowState.Normal;
            }
            else
            {
                Hide();
                WindowState = FormWindowState.Minimized;
            }
        }

        private void ExitApplication()
        {
            // 关闭应用程序
            notifyIcon.Visible = false;
            if (!System.IO.File.Exists(toolSetting.LogFilePath))
            {
                using (FileStream fs = new FileStream(toolSetting.LogFilePath, FileMode.Create, FileAccess.Write))
                {
                    // 
                }
            }
            // write log to txt file
            using (StreamWriter sw = new StreamWriter(toolSetting.LogFilePath, true))
            {
                sw.WriteLine("App Close time: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                sw.WriteLine(" ");
            }
            Application.Exit();
        }
        private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            // 在这里添加处理系统关机事件的代码
            if (!System.IO.File.Exists(toolSetting.LogFilePath))
            {
                using (FileStream fs = new FileStream(toolSetting.LogFilePath, FileMode.Create, FileAccess.Write))
                {
                    // 
                }
            }
            // write log to txt file
            using (StreamWriter sw = new StreamWriter(toolSetting.LogFilePath, true))
            {
                sw.WriteLine("App Close time: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                sw.WriteLine(" ");
            }
        }

        static void CreateShortcut(string shortcutPath, string targetPath)
        {
            // 创建 WshShell 对象
            WshShell shell = new WshShell();

            // 创建快捷方式对象
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);

            // 设置快捷方式属性
            shortcut.TargetPath = targetPath;
            shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);
            shortcut.Description = $"{Path.GetFileNameWithoutExtension(targetPath)} Shortcut";
            shortcut.IconLocation = targetPath;  // 使用应用程序图标

            // 保存快捷方式
            shortcut.Save();
            //Console.WriteLine($"快捷方式已创建：{shortcutPath}");
        }

        private void linkPlantListFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string filePath = "PlantList.txt";

            try
            {
                // 使用 Process.Start 打开文件
                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("发生错误: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("File not found!");
            }
        }
    }
}


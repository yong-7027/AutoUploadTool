using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using IWshRuntimeLibrary;
using System.Runtime.CompilerServices;
using System.Threading;

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

namespace SMTUploadTool
{

    public partial class Form1 : Form
    {Boolean newMonthlyFolderCreated;
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
        private FileSystemWatcher watcher1;
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


            string appName = "SMTUploadTool";  // 替换为你的应用程序名称
            string executablePath = AppDomain.CurrentDomain.BaseDirectory + "SMTUploadTool.exe";  // 获取应用程序的可执行文件路径

            string startupFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string shortcutPath = Path.Combine(startupFolderPath, $"{appName}.lnk");

            CreateShortcut(shortcutPath, executablePath);
            // clean the memory
            //GC.Collect();
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
            toolTip1.SetToolTip(this.btnStart, "Start to monitor");
            toolTip1.SetToolTip(this.btnStop, "Stop to monitor");
            toolTip1.SetToolTip(this.btnChangeSetting, "Change the setting");
            toolTip1.SetToolTip(this.btnApply, "Apply the setting");
            toolTip1.SetToolTip(this.btnCancelSetting, "Cancel the setting");
            toolTip1.SetToolTip(this.btnLogPath, "Change the log file path");
            toolTip1.SetToolTip(this.btnDestinationFolder, "Change the destination folder path");
            toolTip1.SetToolTip(this.btnMonitorFolder, "Change the monitor folder path");
            toolTip1.SetToolTip(this.linkPlantListFile, "Open the PlantList.txt");
            toolTip1.SetToolTip(this.checkLogFunc, "Uncheck the box if you want the program to handle" +
                                                   "\na huge number of folders at the same time");
            toolTip1.SetToolTip(this.numericUpDownLine, "Set the Line Number");
            toolTip1.SetToolTip(this.btnReupload, "Reupload all the files " +
                                                "\nform:" +toolSetting.monitorFolderPath+
                                                "\nto     : "+toolSetting.destinateFolderPath);
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
            
            
            toolSetting = ReadStructFromBinaryFile(binDataFilePath);
            // if toolSetting.monitorFolderPath is doesn't exist, show error message, click btnstop
            if (!Directory.Exists(toolSetting.monitorFolderPath))
            {
                // show warning messge box
                MessageBox.Show("Warning: Monitor Folder path doesn't exist !");
                // show the form on window screen and put center from minimized
                
                //this.StartPosition = FormStartPosition.CenterScreen;
                this.WindowState = FormWindowState.Normal;
                btnStop.PerformClick();
                return;
            }


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


            
            //MessageBox.Show(grandParentDirectory);

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





            // find the grandparent folder of the monitor folder
            string parentDirectory = Path.GetDirectoryName(toolSetting.monitorFolderPath); //
            string grandParentDirectory = Path.GetDirectoryName(parentDirectory);// ..\BACKUP
            watcher1 = new FileSystemWatcher(grandParentDirectory);
            watcher1.NotifyFilter = NotifyFilters.DirectoryName;
            watcher1.Created += OnNewMonthlyCreated;
            watcher1.EnableRaisingEvents = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            // stop monitoring
            if (watcher != null)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
                //Console.WriteLine("Monitoring stopped.");
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                btnStop.BackColor = Color.Gray;
                btnStart.BackColor = Color.Green;
               
            }
            if(watcher1 != null)
            {
                watcher1.EnableRaisingEvents = false;
                //watcher1.Dispose();
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
       
        private static async void OnNewMonthlyCreated(object sender, FileSystemEventArgs e)
        {
            // get watcher from outside
            ToolSetting toolSetting = new ToolSetting();
            string binDataFilePath = "settings.bin";
            toolSetting = ReadStructFromBinaryFile(binDataFilePath);
            
            //MessageBox.Show(e.FullPath);
            if(Directory.Exists(e.FullPath))
            {
                // get folder name
                string folderName = Path.GetFileName(e.FullPath);  // e.fullpath = ..\BACKUP\202401   , folderName = 202401
                
                // check id the new folder is the now monthly folder
                //get the current year and month(yyyyMM)
                string currentYearMonth = DateTime.Now.ToString("yyyyMM");
                // get the folder name
                if(folderName == currentYearMonth)
                {
                    
                    //MessageBox.Show(e.FullPath+"\n"+folderName);
                    
                    // check if the folder "newMonthlyFolder" is have any folder, if empty, wait for 1s and check again
                    while (!Directory.GetDirectories(e.FullPath).Any())
                    {
                        await Task.Delay(1000);
                        //MessageBox.Show(e.FullPath+" is empty");

                        //check if the folder still exist
                        if (!Directory.Exists(e.FullPath))
                        {
                            // write log to txt file
                            using (StreamWriter sw = new StreamWriter(toolSetting.LogFilePath, true))
                            {
                                // record the folder is deleted
                                sw.WriteLine("  WARNING>> (" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + ")Folder \"" + Path.GetFileName(e.FullPath) + "\" is deleted, please check the folder!");
                                sw.WriteLine(" ");
                            }
                            // return
                            //MessageBox.Show("Folder \"" + Path.GetFileName(e.FullPath) + "\" is deleted, please check the folder!");
                            return;
                            //break;
                        }
                    }
                    


                    // get the new monthly folder's all folder
                    string[] newMonitorFolders = Directory.GetDirectories(e.FullPath);
                    //get the directory in newMionitorFolders[0]
                    string[] childNewMonitorFolders = Directory.GetDirectories(newMonitorFolders[0]);

                    // check the length of childNewMonitorFolders, if length is 0
                    if(childNewMonitorFolders.Length > 0)
                    {
                        //MessageBox.Show(childNewMonitorFolders[0]);
                        // get the all files in childNewMonitorFolders[0]
                        string[] files = Directory.GetFiles(childNewMonitorFolders[0]);


                        // get folder's all files
                        //string[] files = Directory.GetFiles(e.FullPath);


                        string monFolder;
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //get the folder name like "20240131151056" get the first 6 digits "202401" store to monFolder and create a folder with the name "202401" in the destination folder
                        if (Path.GetFileName(childNewMonitorFolders[0]).Length < 6)
                        {
                            monFolder="Unknown_Folder";
                        }
                        else
                        {
                            monFolder = Path.GetFileName(childNewMonitorFolders[0]).Substring(0, 6);
                        }
                        
                        // check if the folder is exist in destination folder
                        if (!Directory.Exists(toolSetting.destinateFolderPath + @"\" + monFolder))
                        {
                            // create the folder
                            Directory.CreateDirectory(toolSetting.destinateFolderPath + @"\" + monFolder);
                        }
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




                        // search file, find and copy the insp_pad.txt and change file name then paste to specify folder
                        foreach (string file in files)
                        {
                            if (string.Equals(Path.GetFileName(file), toolSetting.targetFileName, StringComparison.OrdinalIgnoreCase))
                            {
                                //check if the file content is empty, if empty, refresh the file
                                FileInfo fileInfo = new FileInfo(file);
                                if (fileInfo.Length == 0)
                                {


                                    while (fileInfo.Length == 0)
                                    {
                                        //await Task.Delay(1);
                                        fileInfo.Refresh();
                                        //if time is more than 10s, break the loop
                                        if ((DateTime.Now - fileInfo.LastWriteTime).TotalSeconds > 10)
                                        {
                                            // write log to txt file
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
                                                // record the file content is empty,and time 

                                                sw.WriteLine("  WARNING>> (" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + ")File \"" + Path.GetFileName(file) + "\" is empty, please check the file!");


                                            }
                                            break;
                                        }



                                    }
                                }
                                // get model
                                FileInfo fileInfo1 = new FileInfo(file);
                                string model = "";
                                if (fileInfo1.Length != 0)
                                {

                                    
                                    string keyword = "CTM|";
                                    try
                                    {

                                        using (StreamReader reader = new StreamReader(file))
                                        {

                                            string firstLine = reader.ReadLine();


                                            if (!string.IsNullOrEmpty(firstLine))
                                            {
                                                int startIndex = firstLine.IndexOf(keyword);
                                                if (startIndex != -1)
                                                {
                                                    int commaIndex = firstLine.IndexOf(",", startIndex);
                                                    if (commaIndex != -1)
                                                    {
                                                        model = firstLine.Substring(startIndex + keyword.Length, commaIndex - (startIndex + keyword.Length));
                                                    }
                                                }
                                            }
                                        }

                                        //model = "ModelNotFound";
                                    }
                                    catch (Exception ex)
                                    {
                                        // delay 1s
                                        await Task.Delay(1);
                                        // try again
                                        try
                                        {
                                            using (StreamReader reader = new StreamReader(file))
                                            {

                                                string firstLine = reader.ReadLine();


                                                if (!string.IsNullOrEmpty(firstLine))
                                                {
                                                    int startIndex = firstLine.IndexOf(keyword);
                                                    if (startIndex != -1)
                                                    {
                                                        int commaIndex = firstLine.IndexOf(",", startIndex);
                                                        if (commaIndex != -1)
                                                        {
                                                            model = firstLine.Substring(startIndex + keyword.Length, commaIndex - (startIndex + keyword.Length));
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex1)
                                        {
                                            // write log to txt file
                                            using (StreamWriter sw = new StreamWriter(toolSetting.LogFilePath, true))
                                            {
                                                // record the exception
                                                sw.WriteLine("  WARNING>> (" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + ")Exception: " + ex1.Message);


                                            }
                                            model = "Empty";
                                        }

                                    }

                                }
                                else
                                {
                                    model = "EmptyData";
                                }
                                //MessageBox.Show(e.FullPath);
                                string fileName = Path.GetFileName(file);
                                // cut the "." and words after the "." of fileName
                                fileName = fileName.Substring(0, fileName.IndexOf("."));

                                string newFileName = toolSetting.line + "_" + model + "_" + Path.GetFileName(childNewMonitorFolders[0]) + ".txt";

                                //////////////////////////////
                                ///
                                string plantFilePath = "PlantList.txt";  //
                                string newFilePath = "";
                                string des = string.Empty;
                                // check if the Plantslist file exist
                                if (System.IO.File.Exists(plantFilePath))
                                {
                                    // read line by line
                                    string[] plants = System.IO.File.ReadAllLines(plantFilePath);

                                    string newFolderPath;
                                    Boolean plantExist = false;
                                    foreach (string plant in plants)
                                    {
                                        if (model.Contains(plant))
                                        {
                                            des = plant;
                                            //create new folder in destination folder
                                            newFolderPath = toolSetting.destinateFolderPath + @"\" + monFolder + @"\" + plant;
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
                                    if (plantExist == false)
                                    {
                                        //newFolderPath = toolSetting.destinateFolderPath + @"\other";
                                        //newFilePath = newFolderPath + @"\" + newFileName;
                                        newFilePath = toolSetting.destinateFolderPath + @"\" + monFolder + @"\" + newFileName;
                                        System.IO.File.Copy(file, newFilePath, true);
                                    }
                                }
                                else
                                {
                                    newFilePath = toolSetting.destinateFolderPath + @"\" + monFolder + @"\" + newFileName;
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
                                log.filePath = childNewMonitorFolders[0];
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

                                log.folderName = Path.GetFileName(childNewMonitorFolders[0]);

                                log.fileSize = fileInfo1.Length;
                                log.destinationPath = newFilePath;
                                log.destinationFolderName = Path.GetFileName(toolSetting.destinateFolderPath);


                                //loglist.Add(log);

                                //WriteLogToBinaryFile("Log.bin", loglist);
                                // check if the des is Empty
                                if (des != string.Empty)
                                {
                                    des = @"\" + monFolder+@"\" + des;
                                }
                                else
                                {
                                    des = @"\" + monFolder;
                                }
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
                                    sw.WriteLine("  Result>> Date : " + log.year + "/" + log.month + "/" + log.day + ", Time: " + log.actionTime + ", Line:" + toolSetting.line + ", Model:" + model + ", File Name: " + log.fileName + ", File Size: " + log.fileSize + "(bytes)" + ", Status: " + log.status + ", From Folder: " + log.folderName + ", To Destination Folder: " + log.destinationFolderName + des);
                                    sw.WriteLine(" ");

                                }
                            }
                        }
                        /////
                    }
                    else
                    {
                        //MessageBox.Show("0");
                    }
                    




                    //MessageBox.Show(newMonitorFolders[0]+"folder0 ");
                    
                    toolSetting.monitorFolderPath = newMonitorFolders[0];
                    // write the new monitor folder path to binary file
                    WriteStructToBinaryFile(binDataFilePath, toolSetting);
                    // get the form object
                    Form1 form1 = (Form1)Application.OpenForms["Form1"];
                    // change the monitor folder path in the form
                    form1.Invoke((MethodInvoker)delegate {
                        // Running on the UI thread
                        Boolean toggle = true;
                        form1.txtBoxMonitorFolder.Text = toolSetting.monitorFolderPath;
                        // open the UI on window screen/要打开代码才能点击按钮
                        // if the form is minimized, open the form
                        if (form1.WindowState == FormWindowState.Minimized)
                            toggle = true;
                        else
                            toggle = false;

                        if (toggle)
                        {
                            form1.ToggleMainWindow();
                        }
                        

                        form1.WindowState = FormWindowState.Normal;
                        form1.btnStop.PerformClick();
                        form1.btnChangeSetting.PerformClick();
                        form1.btnApply.PerformClick();
                        //form1.btnStart.PerformClick();
                        if (toggle)
                        {
                            form1.ToggleMainWindow();
                        }
                            
                        //form1.ToggleMainWindow();
                        //form1.Refresh();
                        //dispose the watcher1
                        

                      
                    });
                    



                }
                else
                {
                    return;
                }
                


            }

        }
        private static async void OnFolderCreated(object sender, FileSystemEventArgs e)
        {
            ToolSetting toolSetting = new ToolSetting();
            string binDataFilePath = "settings.bin";
            toolSetting = ReadStructFromBinaryFile(binDataFilePath);
            Boolean logCheck=false;
            //create form1 object
            Form1 form1 = (Form1)Application.OpenForms["Form1"];
            // check if the form1 checkbox is checked
            if (form1.checkLogFunc.Checked == true)
            {
                logCheck = true;
            }
            //form1.Close();
          

            if (e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Renamed)
            {


                if (logCheck)
                {
                    // write log to txt file
                    if (!System.IO.File.Exists(toolSetting.LogFilePath))
                    {
                        using (FileStream fs = new FileStream(toolSetting.LogFilePath, FileMode.Create, FileAccess.Write))
                        {
                            // 
                        }
                    }
                    using (StreamWriter sw = new StreamWriter(toolSetting.LogFilePath, true))
                    {

                        sw.WriteLine("(" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + ")Checking Folder <" + Path.GetFileName(e.FullPath) + ">...");

                    }
                }

                // loop until the folder exist the target file
                while (!Directory.GetFiles(e.FullPath).Any(file =>
                string.Equals(Path.GetFileName(file), toolSetting.targetFileName, StringComparison.OrdinalIgnoreCase)))
                {
                    await Task.Delay(1); // every ? second check once, can be adjusted according to the actual situation
                    //check if the folder still exist
                    if (!Directory.Exists(e.FullPath))
                    {

                        // write log to txt file
                        using (StreamWriter sw = new StreamWriter(toolSetting.LogFilePath, true))
                        {
                            // record the folder is deleted
                            sw.WriteLine("  WARNING>> (" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + ")Folder \"" + Path.GetFileName(e.FullPath) + "\" is deleted, please check the folder!");
                            sw.WriteLine(" ");
                        }
                        // return
                        //MessageBox.Show("Folder \"" + Path.GetFileName(e.FullPath) + "\" is deleted, please check the folder!");
                        return;
                        //break;
                    }
                }
                string monFolder;
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //get the folder name like "20240131151056" get the first 6 digits "202401" store to monFolder and create a folder with the name "202401" in the destination folder
                if(Path.GetFileName(e.FullPath).Length < 6)
                {
                    monFolder = "Unknown_Folder";
                }
                else
                {
                    monFolder = Path.GetFileName(e.FullPath).Substring(0, 6);
                }
                //monFolder = Path.GetFileName(e.FullPath).Substring(0, 6);
                // check if the folder is exist in destination folder
                if (!Directory.Exists(toolSetting.destinateFolderPath + @"\" + monFolder))
                {
                    // create the folder
                    Directory.CreateDirectory(toolSetting.destinateFolderPath + @"\" + monFolder);
                }
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



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
                                if ((DateTime.Now - fileInfo.LastWriteTime).TotalSeconds > 10)
                                {

                                    // write log to txt file
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
                                        // record the file content is empty,and time 

                                        sw.WriteLine("  WARNING>> (" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + ")File \"" + Path.GetFileName(file) + "\" is empty, please check the file!");

                                        
                                    }
                                    break;
                                }



                            }
                        }


                        // get model
                        FileInfo fileInfo1 = new FileInfo(file);
                        string model = "";
                        if (fileInfo1.Length != 0)
                        {
                            
                            
                            string keyword= "CTM|";
                            try
                            {
                                
                                using (StreamReader reader = new StreamReader(file))
                                {
                                    
                                    string firstLine = reader.ReadLine();

                                    
                                    if (!string.IsNullOrEmpty(firstLine))
                                    {
                                        int startIndex = firstLine.IndexOf(keyword);
                                        if (startIndex != -1)
                                        {
                                            int commaIndex = firstLine.IndexOf(",", startIndex);
                                            if (commaIndex != -1)
                                            {
                                                model= firstLine.Substring(startIndex + keyword.Length, commaIndex - (startIndex + keyword.Length));
                                            }
                                        }
                                    }
                                }

                                //model = "ModelNotFound";
                            }
                            catch (Exception ex)
                            {
                                // delay 1s
                                await Task.Delay(1);
                                // try again
                                try
                                {
                                    using (StreamReader reader = new StreamReader(file))
                                    {

                                        string firstLine = reader.ReadLine();


                                        if (!string.IsNullOrEmpty(firstLine))
                                        {
                                            int startIndex = firstLine.IndexOf(keyword);
                                            if (startIndex != -1)
                                            {
                                                int commaIndex = firstLine.IndexOf(",", startIndex);
                                                if (commaIndex != -1)
                                                {
                                                    model = firstLine.Substring(startIndex + keyword.Length, commaIndex - (startIndex + keyword.Length));
                                                }
                                            }
                                        }
                                    }
                                }
                                catch(Exception ex1)
                                {
                                        // write log to txt file
                                        using (StreamWriter sw = new StreamWriter(toolSetting.LogFilePath, true))
                                        {
                                            // record the exception
                                            sw.WriteLine("  WARNING>> (" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + ")Exception: " + ex1.Message);
                                    

                                        }
                                        model = "Empty";
                                }
                               
                            }

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
                        string plantFilePath = "PlantList.txt";  //
                        string newFilePath="";
                        string des=string.Empty;
                        // check if the Plantslist file exist
                        if (System.IO.File.Exists(plantFilePath))
                        { 
                            // read line by line
                            string[] plants = System.IO.File.ReadAllLines(plantFilePath);
                            
                            string newFolderPath;
                            Boolean plantExist = false;
                            foreach (string plant in plants)
                            {
                                if (model.Contains(plant))
                                {
                                    des = plant;
                                    //create new folder in destination folder
                                    newFolderPath = toolSetting.destinateFolderPath + @"\" + monFolder + @"\" + plant;
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
                                
                                newFilePath = toolSetting.destinateFolderPath + @"\" + monFolder + @"\" + newFileName;
                                System.IO.File.Copy(file, newFilePath, true);
                            }
                            
                            
                        }
                        else
                        {
                            //if doesn't have the PlantList.txt, copy the file to the destination folder
                            newFilePath = toolSetting.destinateFolderPath +@"\"+monFolder+ @"\" + newFileName;
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
                        // check if the des is Empty
                        if (des != string.Empty)
                        {
                            des = @"\" + monFolder+@"\" + des;
                        }
                        else
                        {
                            des = @"\" + monFolder;
                        }


                        if (logCheck)
                        {

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

                                sw.WriteLine("  Result>> Date : " + log.year + "/" + log.month + "/" + log.day + ", Time: " + log.actionTime + ", Line:" + toolSetting.line + ", Model:" + model + ", File Name: " + log.fileName + ", File Size: " + log.fileSize + "(bytes)" + ", Status: " + log.status + ", From Folder: " + log.folderName + ", To Destination Folder: " + log.destinationFolderName + des);
                                sw.WriteLine(" ");

                            }
                        }

                        
                       

                    }
                }
            }
        }


       
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
                toolTip1.SetToolTip(this.btnReupload, "Reupload all the files " +
                                                "\nform:" + toolSetting.monitorFolderPath +
                                                "\nto     : " + toolSetting.destinateFolderPath);

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

            // when user click the "X" button, hide the window instead of close the application
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
        
        /*
        static string FindAndExtract(string filePath, string keyword) ///CTM|
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
        */
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
            // create a new tray icon
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new Icon("icon.ico"); // replace the icon
            notifyIcon.Text = "SMTUpload"; // replace the text
            notifyIcon.Visible = true;


            // add double click event, when user double click the icon, show the main window
            notifyIcon.DoubleClick += (sender, e) => ToggleMainWindow();

            // create a context menu when user right click the icon
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add("Exit", (sender, e) => ExitApplication());

            // assign the context menu to the tray icon
            notifyIcon.ContextMenu = contextMenu;
        }

        private void ToggleMainWindow()
        {
            // change the window state
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
            // close the application
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
            // here you can handle session ending event
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
            
            WshShell shell = new WshShell();

            //
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);

            // set shortcut's properties
            shortcut.TargetPath = targetPath;
            shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);
            shortcut.Description = $"{Path.GetFileNameWithoutExtension(targetPath)} Shortcut";
            shortcut.IconLocation = targetPath;  // use target's icon

            // save shortcut
            shortcut.Save();
            
            
        }

        private void linkPlantListFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string filePath = "PlantList.txt";

            try
            {
          
                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("File not found!");
            }
        }

       

        private void logCheckBox_Click(object sender, EventArgs e)
        {
            // get datetime (yyyy/MM/dd HH:mm:ss)
            string dateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            string status = "ON";
            if (checkLogFunc.Checked == true)
            {
                status = "ON";
            }
            else
            {
                status = "OFF";
            }
            using (StreamWriter sw = new StreamWriter(toolSetting.LogFilePath, true))
            {
                // record the folder is deleted
                sw.WriteLine("  Notification>> Log Record Function : "+status+" ("+dateTime+")");
                sw.WriteLine(" ");
            }
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void btnReupload_Click(object sender, EventArgs e)
        {
            long totalFileCount = 0;
            // get all the folders in monitor folder
            string[] folders = Directory.GetDirectories(toolSetting.monitorFolderPath);
            progressBar1.Maximum = folders.Length;
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Visible = true;
            btnReupload.Enabled = false;
            btnReupload.IsAccessible = false;

            // loop all the folders
            foreach (string folder in folders)
            {
                progressBar1.Value++;
                
                
                Boolean logCheck = false;
                //if (checkLogFunc.Checked == true)
                //{
                //    logCheck = true;
                //}
                // get the folder name
                string folderName = Path.GetFileName(folder);
                // get the all files in the folder
                string[] files = Directory.GetFiles(folder);
                //get path of the folder
                string folderPath = Path.GetDirectoryName(folder);


                string monFolder;
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //get the folder name like "20240131151056" get the first 6 digits "202401" store to monFolder and create a folder with the name "202401" in the destination folder
                if (Path.GetFileName(folder).Length < 6)
                    monFolder = "Unknown_Folder";
                else
                    monFolder = Path.GetFileName(folder).Substring(0, 6);
                // check if the folder is exist in destination folder
                if (!Directory.Exists(toolSetting.destinateFolderPath + @"\" + monFolder))
                {
                    // create the folder
                    Directory.CreateDirectory(toolSetting.destinateFolderPath + @"\" + monFolder);
                }
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




                // search file, find and copy the insp_pad.txt and change file name then paste to specify folder
                foreach (string file in files)
                {
                    if (string.Equals(Path.GetFileName(file), toolSetting.targetFileName, StringComparison.OrdinalIgnoreCase))
                    {
                        //check if the file content is empty, if empty, refresh the file
                        FileInfo fileInfo = new FileInfo(file);
                        


                        // get model
                        FileInfo fileInfo1 = new FileInfo(file);
                        string model = "";
                        if (fileInfo1.Length != 0)
                        {

                            //MessageBox.Show(fileInfo1.Length.ToString());
                            //model = FindAndExtract(file, "CTM|");
                            string keyword = "CTM|";
                            try
                            {

                                using (StreamReader reader = new StreamReader(file))
                                {

                                    string firstLine = reader.ReadLine();


                                    if (!string.IsNullOrEmpty(firstLine))
                                    {
                                        int startIndex = firstLine.IndexOf(keyword);
                                        if (startIndex != -1)
                                        {
                                            int commaIndex = firstLine.IndexOf(",", startIndex);
                                            if (commaIndex != -1)
                                            {
                                                model = firstLine.Substring(startIndex + keyword.Length, commaIndex - (startIndex + keyword.Length));
                                            }
                                        }
                                    }
                                }

                                //model = "ModelNotFound";
                            }
                            catch (Exception ex)
                            {
                                // delay 1s
                                //await Task.Delay(1);
                                // try again
                                try
                                {
                                    using (StreamReader reader = new StreamReader(file))
                                    {

                                        string firstLine = reader.ReadLine();


                                        if (!string.IsNullOrEmpty(firstLine))
                                        {
                                            int startIndex = firstLine.IndexOf(keyword);
                                            if (startIndex != -1)
                                            {
                                                int commaIndex = firstLine.IndexOf(",", startIndex);
                                                if (commaIndex != -1)
                                                {
                                                    model = firstLine.Substring(startIndex + keyword.Length, commaIndex - (startIndex + keyword.Length));
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex1)
                                {
                                    // write log to txt file
                                    using (StreamWriter sw = new StreamWriter(toolSetting.LogFilePath, true))
                                    {
                                        // record the exception
                                        sw.WriteLine("  WARNING>> (" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + ")Exception: " + ex1.Message);


                                    }
                                    model = "Empty";
                                }

                            }

                        }
                        else
                        {
                            model = "EmptyData";
                        }
                        //MessageBox.Show(e.FullPath);
                        string fileName = Path.GetFileName(file);
                        // cut the "." and words after the "." of fileName
                        fileName = fileName.Substring(0, fileName.IndexOf("."));

                        string newFileName = toolSetting.line + "_" + model + "_" + Path.GetFileName(folder) + ".txt";

                        //////////////////////////////
                        ///
                        string plantFilePath = "PlantList.txt";  //
                        string newFilePath = "";
                        string des = string.Empty;
                        // check if the Plantslist file exist
                        if (System.IO.File.Exists(plantFilePath))
                        {
                            // read line by line
                            string[] plants = System.IO.File.ReadAllLines(plantFilePath);

                            string newFolderPath;
                            Boolean plantExist = false;
                            foreach (string plant in plants)
                            {
                                if (model.Contains(plant))
                                {
                                    des = plant;
                                    //create new folder in destination folder
                                    newFolderPath = toolSetting.destinateFolderPath + @"\" + monFolder + @"\" + plant;
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
                            if (plantExist == false)
                            {
                                //newFolderPath = toolSetting.destinateFolderPath + @"\other";
                                //newFilePath = newFolderPath + @"\" + newFileName;
                                newFilePath = toolSetting.destinateFolderPath + @"\" + monFolder + @"\" + newFileName;
                                System.IO.File.Copy(file, newFilePath, true);
                                totalFileCount++;
                            }


                        }
                        else
                        {
                            newFilePath = toolSetting.destinateFolderPath + @"\" + monFolder + @"\" + newFileName;
                            System.IO.File.Copy(file, newFilePath, true);
                            totalFileCount++;
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
                        log.filePath = folderPath;
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

                        log.folderName = Path.GetFileName(folderPath);

                        log.fileSize = fileInfo1.Length;
                        log.destinationPath = newFilePath;
                        log.destinationFolderName = Path.GetFileName(toolSetting.destinateFolderPath);


                        //loglist.Add(log);

                        //WriteLogToBinaryFile("Log.bin", loglist);
                        // check if the des is Empty
                        if (des != string.Empty)
                        {
                            des = @"\" + monFolder+@"\" + des;
                        }
                        else
                        {
                            des = @"\" + monFolder;
                        }


                        if (logCheck)
                        {

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

                                sw.WriteLine("  Result>> Date : " + log.year + "/" + log.month + "/" + log.day + ", Time: " + log.actionTime + ", Line:" + toolSetting.line + ", Model:" + model + ", File Name: " + log.fileName + ", File Size: " + log.fileSize + "(bytes)" + ", Status: " + log.status + ", From Folder: " + log.folderName + ", To Destination Folder: " + log.destinationFolderName + des);
                                sw.WriteLine(" ");

                            }
                        }




                    }
                }
            }
            MessageBox.Show("Reupload " + totalFileCount + " files successfully!");
            btnReupload.Enabled = true;
            btnReupload.IsAccessible = true;
            progressBar1.Visible = false;
            
        }
    }
}


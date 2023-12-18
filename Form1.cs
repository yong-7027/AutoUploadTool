using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public struct ToolSetting
{
    public string monitorFolderPath;
    public string targetFileName;
    public string destinateFolderPath;
}

namespace FetchUploadTool
{
    
    public partial class Form1 : Form
    {
        ToolSetting defaultSettings = new ToolSetting { 
            monitorFolderPath = "", 
            targetFileName = "", 
            destinateFolderPath = "" 
        };
        ToolSetting toolSetting = new ToolSetting();
        string binDataFilePath = "settings.bin";

        private FileSystemWatcher watcher;
        Boolean initializaion;




        public Form1()
        { 
            InitializeComponent();
            

            // check if the binary file exist, if not, create one
            if (!File.Exists(binDataFilePath))
            {
                // write default settings to binary file
                WriteStructToBinaryFile(binDataFilePath, defaultSettings);
            }

            // read settings from binary file
            toolSetting = ReadStructFromBinaryFile(binDataFilePath);

            // show settings in text box
            //Console.WriteLine($"MonitorFolderPath: {readSettings.monitorFolderPath}, TargetFileName: {readSettings.targetFileName}, DestinateFolderPath: {readSettings.destinateFolderPath}");
            //message box
            //MessageBox.Show($"MonitorFolderPath: {toolSetting.monitorFolderPath}, TargetFileName: {toolSetting.targetFileName}, DestinateFolderPath: {toolSetting.destinateFolderPath}");
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
                    toolSetting.monitorFolderPath = folderBrowserDialog.SelectedPath;
                    txtBoxMonitorFolder.Text = toolSetting.monitorFolderPath;                                        
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            CenterToScreen();
           
            //string content = reader.ReadToEnd();
            //txtBoxMonitorFolder.Text = content;
            txtBoxMonitorFolder.Text = toolSetting.monitorFolderPath;
            txtTargetFileName.Text = toolSetting.targetFileName;
            txtDestinationFolder.Text = toolSetting.destinateFolderPath;
            btnStop.Enabled = false;
            btnStop.BackColor = Color.Gray;

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
            }
            btnApply.Hide();
            btnCancelSetting.Hide();
            
        }
        

        // write the whole struct to binary file
        static void WriteStructToBinaryFile(string filePath, ToolSetting data)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                // write the whole struct to binary file
                writer.Write(data.monitorFolderPath);
                writer.Write(data.targetFileName);
                writer.Write(data.destinateFolderPath);
            }
        }

        // read the whole struct from binary file
        static ToolSetting ReadStructFromBinaryFile(string filePath)
        {
            ToolSetting data = new ToolSetting();

            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                // read the whole struct from binary file
                data.monitorFolderPath = reader.ReadString();
                data.targetFileName = reader.ReadString();
                data.destinateFolderPath = reader.ReadString();
            }

            return data;
        }

        private void txtTargetFileName_TextChanged(object sender, EventArgs e)
        {
            toolSetting.targetFileName = txtTargetFileName.Text;
           
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
                    toolSetting.destinateFolderPath = folderBrowserDialog.SelectedPath;
                    txtDestinationFolder.Text = toolSetting.destinateFolderPath;
                }
            }
        }

        private void txtDestinationFolder_TextChanged(object sender, EventArgs e)
        {
            toolSetting.destinateFolderPath = txtDestinationFolder.Text;
            
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
                        File.Copy(file, newFilePath,true);
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
                        File.Copy(file, newFilePath, true);
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
                // execute the function you want to trigger here
                // Console.WriteLine($"New folder created or renamed: {e.FullPath}");
                //MessageBox.Show($"New folder created or renamed: {e.FullPath}");

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
                            }
                        }

                        string fileName = Path.GetFileName(file);
                        // cut the "." and words after the "." of fileName
                        fileName = fileName.Substring(0, fileName.IndexOf("."));

                        string newFileName = fileName + "_" + Path.GetFileName(e.FullPath) + ".txt";
                        string newFilePath = toolSetting.destinateFolderPath + @"\" + newFileName;
                        File.Copy(file, newFilePath, true);
                        //Console.WriteLine($"New file created: {newFilePath}");
                    }
                }
            }
        }



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

        private void btnChangeSetting_Click(object sender, EventArgs e)
        {
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
            else
            {
                toolSetting.monitorFolderPath = txtBoxMonitorFolder.Text;
                toolSetting.targetFileName = txtTargetFileName.Text;
                toolSetting.destinateFolderPath = txtDestinationFolder.Text;
                initializaion = true;


            }


            WriteStructToBinaryFile(binDataFilePath, toolSetting);
            toolSetting = ReadStructFromBinaryFile(binDataFilePath);
            
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

        }

        private void txtBoxMonitorFolder_TextChanged(object sender, EventArgs e)
        {

            toolSetting.monitorFolderPath = txtBoxMonitorFolder.Text;
        }

        private void btnCancelSetting_Click(object sender, EventArgs e)
        {
            toolSetting = ReadStructFromBinaryFile(binDataFilePath);

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

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}

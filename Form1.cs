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

namespace AutoUploadTool
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


        
        public Form1()
        { 
            InitializeComponent();
            

            // 检查文件是否存在，如果不存在，则创建文件并写入数据
            if (!File.Exists(binDataFilePath))
            {
                // 将整个结构体写入二进制文件
                WriteStructToBinaryFile(binDataFilePath, defaultSettings);
            }

            // 从二进制文件中读取整个结构体
            toolSetting = ReadStructFromBinaryFile(binDataFilePath);

            // 输出读取的结构体数据
            //Console.WriteLine($"MonitorFolderPath: {readSettings.monitorFolderPath}, TargetFileName: {readSettings.targetFileName}, DestinateFolderPath: {readSettings.destinateFolderPath}");
            //message box
            //MessageBox.Show($"MonitorFolderPath: {toolSetting.monitorFolderPath}, TargetFileName: {toolSetting.targetFileName}, DestinateFolderPath: {toolSetting.destinateFolderPath}");
        }

        private void btnMonitorFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                // 设置对话框的描述
                folderBrowserDialog.Description = "选择文件夹位置";

                // 显示对话框并获取用户的操作结果
                DialogResult result = folderBrowserDialog.ShowDialog();

                // 检查用户是否点击了“确定”按钮
                if (result == DialogResult.OK)
                {
                    // 获取用户选择的文件夹路径并显示在文本框中
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
            
        }
        

        // 将整个结构体写入二进制文件
        static void WriteStructToBinaryFile(string filePath, ToolSetting data)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                // 将整个结构体写入二进制文件
                writer.Write(data.monitorFolderPath);
                writer.Write(data.targetFileName);
                writer.Write(data.destinateFolderPath);
            }
        }

        // 从二进制文件中读取整个结构体
        static ToolSetting ReadStructFromBinaryFile(string filePath)
        {
            ToolSetting data = new ToolSetting();

            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                // 从二进制文件中读取整个结构体
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
                // 设置对话框的描述
                folderBrowserDialog.Description = "选择文件夹位置";

                // 显示对话框并获取用户的操作结果
                DialogResult result = folderBrowserDialog.ShowDialog();

                // 检查用户是否点击了“确定”按钮
                if (result == DialogResult.OK)
                {
                    // 获取用户选择的文件夹路径并显示在文本框中
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
            // 指定要监控的文件夹路径
            //string folderPathToMonitor = @"C:\Users\tcy70\Desktop\checkFolder";

            // 创建一个新的 FileSystemWatcher 对象
            watcher = new FileSystemWatcher(toolSetting.monitorFolderPath);

            // 监控所有类型的文件变化，包括子文件夹
            watcher.IncludeSubdirectories = true;

            // 设置要监控的事件类型
            watcher.NotifyFilter = NotifyFilters.DirectoryName;

            // 事件处理程序，当文件夹被创建时触发
            watcher.Created += OnFolderCreated;

            // 启动监控
            watcher.EnableRaisingEvents = true;

            //Console.WriteLine($"Monitoring folder: {toolSetting.monitorFolderPath}");
            MessageBox.Show($"Monitoring folder: {toolSetting.monitorFolderPath}");
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            btnStop.BackColor = Color.Red;
            btnStart.BackColor = Color.Gray;

            btnChangeSetting.Enabled = false;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            // 停止监控
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

        private static async void OnFolderCreated(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                // 在这里执行您想要触发的函数
                Console.WriteLine($"New folder created: {e.FullPath}");


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
                    if (file.Contains("insp_pad.txt"))
                    {
                        string fileName = Path.GetFileName(file);
                        // cut the "." and words after the "." of fileName
                        fileName = fileName.Substring(0, fileName.IndexOf("."));


                        //Console.WriteLine(file);
                        string newFileName = fileName + "_" + folderName + ".txt";
                        string newFilePath = @"C:\Users\tcy70\OneDrive - student.tarc.edu.my\Destination\" + newFileName;
                        File.Copy(file, newFilePath);
                        Console.WriteLine($"New file created: {newFilePath}");
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // 停止监控
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
            else
            {
                toolSetting.monitorFolderPath = txtBoxMonitorFolder.Text;
                toolSetting.targetFileName = txtTargetFileName.Text;
                toolSetting.destinateFolderPath = txtDestinationFolder.Text;
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


        }
    }
}

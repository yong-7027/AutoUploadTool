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


        private string monitorFolderPathStore = "MonitorFolderPath.txt";
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
            MessageBox.Show($"MonitorFolderPath: {toolSetting.monitorFolderPath}, TargetFileName: {toolSetting.targetFileName}, DestinateFolderPath: {toolSetting.destinateFolderPath}");
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
                    WriteStructToBinaryFile(binDataFilePath, toolSetting);
                    

                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (StreamReader reader = new StreamReader(monitorFolderPathStore))
            {
                //string content = reader.ReadToEnd();
                //txtBoxMonitorFolder.Text = content;
                txtBoxMonitorFolder.Text = toolSetting.monitorFolderPath;
                txtTargetFileName.Text = toolSetting.targetFileName;
                txtDestinationFolder.Text = toolSetting.destinateFolderPath;
            }
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
            WriteStructToBinaryFile(binDataFilePath, toolSetting);
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
                    WriteStructToBinaryFile(binDataFilePath, toolSetting);


                }
            }
        }

        private void txtDestinationFolder_TextChanged(object sender, EventArgs e)
        {
            toolSetting.destinateFolderPath = txtDestinationFolder.Text;
            WriteStructToBinaryFile(binDataFilePath, toolSetting);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

        }

        private void btnStop_Click(object sender, EventArgs e)
        {

        }
    }
}

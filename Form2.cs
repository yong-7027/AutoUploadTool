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
    public partial class Form2 : Form
    {
        string year;
        string month;
        string day;
        public Form2()
        {
            InitializeComponent();
            
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            txtLogLocation.Text = "Log Location: " + Directory.GetCurrentDirectory() + "\\log.txt";
            // set dateTimePicker to today
            dateTimePicker1.Value = DateTime.Today;
            // get year, month, day from dateTimePicker
            year = dateTimePicker1.Value.Year.ToString();
            month = dateTimePicker1.Value.Month.ToString();
            day = dateTimePicker1.Value.Day.ToString();
            List<LogInfo> logList = ReadLogFromBinaryFile("log.bin");
            //check if logList is empty
            if (logList.Count == 0)
            {
                MessageBox.Show("No log found");
                this.Close();
                return;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                //display logList in listview
                //get Hour btnHour.Value
                //get Minute btnMinute.Value
                

                foreach (var log in logList)
                {
                   

                    
                    DateTime dateTime = DateTime.ParseExact(log.actionTime, "HH:mm:ss", null);

                    
                    string hoursString = dateTime.Hour.ToString("00"); // 
                    string minutesString = dateTime.Minute.ToString("00");


                    //search for log with the same date as dateTimePicker
                    if (log.year != year || log.month != month || log.day != day)
                    {
                        continue;
                    }
                    sb.AppendLine("Date : " + log.year + "/" + log.month + "/" + log.day);
                    sb.AppendLine("Upload Time: " + log.actionTime);
                    sb.AppendLine("Upload Status: " + log.status);
                    sb.AppendLine("File Name: " + log.fileName);
                    sb.AppendLine("From :");
                    sb.AppendLine("Monitor Folder: " + log.folderName);
                    sb.AppendLine("File Path: " + log.filePath);
                    sb.AppendLine("To :");
                    sb.AppendLine("Destination Folder: " + log.destinationFolderName);
                    sb.AppendLine("Destination Path: " + log.destinationPath);
                    
                    sb.AppendLine("File Size: " + log.fileSize);
                    
                    
                    sb.AppendLine("--------------------------------------------------------------------------------------------------------------------------------------------");
                }
                // check if sb is empty
                if (sb.Length == 0)
                {
                    
                }
                else
                {
                    richTextBox1.Text = sb.ToString();
                }
                //richTextBox1.Text = sb.ToString();
            }

        }



        static List<LogInfo> ReadLogFromBinaryFile(string filePath)
        {
            List<LogInfo> data = new List<LogInfo>();

            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
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

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            year = dateTimePicker1.Value.Year.ToString();
            month = dateTimePicker1.Value.Month.ToString();
            day = dateTimePicker1.Value.Day.ToString();
            List<LogInfo> logList = ReadLogFromBinaryFile("log.bin");
            //check if logList is empty
            if (logList.Count == 0)
            {
                MessageBox.Show("No log found");
                this.Close();
                return;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                //display logList in listview
                foreach (var log in logList)
                {


                    //search for log with the same date as dateTimePicker
                    if (log.year != year || log.month != month || log.day != day)
                    {
                        
                        continue;
                    }
                    sb.AppendLine("Date : " + log.year + "/" + log.month + "/" + log.day);
                    sb.AppendLine("Upload Time: " + log.actionTime);
                    sb.AppendLine("Upload Status: " + log.status);
                    sb.AppendLine("File Name: " + log.fileName);
                    sb.AppendLine("From :");
                    sb.AppendLine("Monitor Folder: " + log.folderName);
                    sb.AppendLine("File Path: " + log.filePath);
                    sb.AppendLine("To :");
                    sb.AppendLine("Destination Folder: " + log.destinationFolderName);
                    sb.AppendLine("Destination Path: " + log.destinationPath);

                    sb.AppendLine("File Size: " + log.fileSize+ "(bytes)");


                    sb.AppendLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                }
                // check if sb is empty
                if (sb.Length == 0)
                {
                    
                }
                else
                {
                    richTextBox1.Text = sb.ToString();
                }
                //richTextBox1.Text = sb.ToString();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            year = dateTimePicker1.Value.Year.ToString();
            month = dateTimePicker1.Value.Month.ToString();
            day = dateTimePicker1.Value.Day.ToString();
            List<LogInfo> logList = ReadLogFromBinaryFile("log.bin");
            //check if logList is empty
            if (logList.Count == 0)
            {
                
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                //display logList in listview
                foreach (var log in logList)
                {


                    //search for log with the same date as dateTimePicker
                    if (log.year != year || log.month != month || log.day != day)
                    {
                        
                        continue;
                    }
                    sb.AppendLine("Date : " + log.year + "/" + log.month + "/" + log.day);
                    sb.AppendLine("Upload Time: " + log.actionTime);
                    sb.AppendLine("Upload Status: " + log.status);
                    sb.AppendLine("File Name: " + log.fileName);
                    sb.AppendLine("From :");
                    sb.AppendLine("Monitor Folder: " + log.folderName);
                    sb.AppendLine("File Path: " + log.filePath);
                    sb.AppendLine("To :");
                    sb.AppendLine("Destination Folder: " + log.destinationFolderName);
                    sb.AppendLine("Destination Path: " + log.destinationPath);

                    sb.AppendLine("File Size: " + log.fileSize);


                    sb.AppendLine("--------------------------------------------------------------------------------");
                }
                // check if sb is empty
                if (sb.Length == 0)
                {
                    
                    richTextBox1.Text = sb.ToString();
                }
                else
                {
                    richTextBox1.Text = sb.ToString();
                }
                //richTextBox1.Text = sb.ToString();
            }


        }

        private void btnHour_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void txtLogLocation_Click(object sender, EventArgs e)
        {

        }
    }


    public static class LogFunc
    {
        static void WriteLogToBinaryFile(string filePath, List<LogInfo> data)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
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

        static List<LogInfo> ReadLogFromBinaryFile(string filePath)
        {
            List<LogInfo> data = new List<LogInfo>();

            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
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
    }
}

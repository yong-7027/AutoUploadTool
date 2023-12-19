namespace FetchUploadTool
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.txtBoxMonitorFolder = new System.Windows.Forms.TextBox();
            this.btnMonitorFolder = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTargetFileName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDestinationFolder = new System.Windows.Forms.TextBox();
            this.btnDestinationFolder = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnChangeSetting = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancelSetting = new System.Windows.Forms.Button();
            this.linkLog = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownLine = new System.Windows.Forms.NumericUpDown();
            this.txtLogPath = new System.Windows.Forms.Label();
            this.btnLogPath = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLine)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(22, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Monitoring Folder :";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtBoxMonitorFolder
            // 
            this.txtBoxMonitorFolder.Location = new System.Drawing.Point(25, 59);
            this.txtBoxMonitorFolder.Name = "txtBoxMonitorFolder";
            this.txtBoxMonitorFolder.Size = new System.Drawing.Size(433, 20);
            this.txtBoxMonitorFolder.TabIndex = 1;
            this.txtBoxMonitorFolder.TextChanged += new System.EventHandler(this.txtBoxMonitorFolder_TextChanged);
            // 
            // btnMonitorFolder
            // 
            this.btnMonitorFolder.Location = new System.Drawing.Point(464, 56);
            this.btnMonitorFolder.Name = "btnMonitorFolder";
            this.btnMonitorFolder.Size = new System.Drawing.Size(75, 23);
            this.btnMonitorFolder.TabIndex = 2;
            this.btnMonitorFolder.Text = "Browser";
            this.btnMonitorFolder.UseVisualStyleBackColor = true;
            this.btnMonitorFolder.Click += new System.EventHandler(this.btnMonitorFolder_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(22, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(367, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Target File Name (Include file name extensions e.g.  example.txt) :";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // txtTargetFileName
            // 
            this.txtTargetFileName.Location = new System.Drawing.Point(25, 98);
            this.txtTargetFileName.Name = "txtTargetFileName";
            this.txtTargetFileName.Size = new System.Drawing.Size(375, 20);
            this.txtTargetFileName.TabIndex = 4;
            this.txtTargetFileName.TextChanged += new System.EventHandler(this.txtTargetFileName_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(22, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Destination Folder :";
            // 
            // txtDestinationFolder
            // 
            this.txtDestinationFolder.Location = new System.Drawing.Point(25, 137);
            this.txtDestinationFolder.Name = "txtDestinationFolder";
            this.txtDestinationFolder.Size = new System.Drawing.Size(433, 20);
            this.txtDestinationFolder.TabIndex = 6;
            this.txtDestinationFolder.TextChanged += new System.EventHandler(this.txtDestinationFolder_TextChanged);
            // 
            // btnDestinationFolder
            // 
            this.btnDestinationFolder.Location = new System.Drawing.Point(464, 134);
            this.btnDestinationFolder.Name = "btnDestinationFolder";
            this.btnDestinationFolder.Size = new System.Drawing.Size(75, 23);
            this.btnDestinationFolder.TabIndex = 7;
            this.btnDestinationFolder.Text = "Browser";
            this.btnDestinationFolder.UseVisualStyleBackColor = true;
            this.btnDestinationFolder.Click += new System.EventHandler(this.btnDestinationFolder_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.btnStart.Location = new System.Drawing.Point(25, 235);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 8;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.Red;
            this.btnStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStop.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.btnStop.Location = new System.Drawing.Point(124, 235);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 9;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(224, 235);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnChangeSetting
            // 
            this.btnChangeSetting.Location = new System.Drawing.Point(25, 203);
            this.btnChangeSetting.Name = "btnChangeSetting";
            this.btnChangeSetting.Size = new System.Drawing.Size(95, 23);
            this.btnChangeSetting.TabIndex = 11;
            this.btnChangeSetting.Text = "Change Setting";
            this.btnChangeSetting.UseVisualStyleBackColor = true;
            this.btnChangeSetting.Click += new System.EventHandler(this.btnChangeSetting_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(280, 203);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 12;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancelSetting
            // 
            this.btnCancelSetting.Location = new System.Drawing.Point(361, 203);
            this.btnCancelSetting.Name = "btnCancelSetting";
            this.btnCancelSetting.Size = new System.Drawing.Size(75, 23);
            this.btnCancelSetting.TabIndex = 13;
            this.btnCancelSetting.Text = "Cancel";
            this.btnCancelSetting.UseVisualStyleBackColor = true;
            this.btnCancelSetting.Click += new System.EventHandler(this.btnCancelSetting_Click);
            // 
            // linkLog
            // 
            this.linkLog.AutoSize = true;
            this.linkLog.Location = new System.Drawing.Point(476, 240);
            this.linkLog.Name = "linkLog";
            this.linkLog.Size = new System.Drawing.Size(51, 13);
            this.linkLog.TabIndex = 14;
            this.linkLog.TabStop = true;
            this.linkLog.Text = "View Log";
            this.linkLog.Visible = false;
            this.linkLog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLog_LinkClicked);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Line  :";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // numericUpDownLine
            // 
            this.numericUpDownLine.Location = new System.Drawing.Point(64, 16);
            this.numericUpDownLine.Name = "numericUpDownLine";
            this.numericUpDownLine.Size = new System.Drawing.Size(43, 20);
            this.numericUpDownLine.TabIndex = 16;
            this.numericUpDownLine.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownLine.ValueChanged += new System.EventHandler(this.numericUpDownLine_ValueChanged);
            // 
            // txtLogPath
            // 
            this.txtLogPath.AutoSize = true;
            this.txtLogPath.Location = new System.Drawing.Point(119, 173);
            this.txtLogPath.Name = "txtLogPath";
            this.txtLogPath.Size = new System.Drawing.Size(69, 13);
            this.txtLogPath.TabIndex = 17;
            this.txtLogPath.Text = "Log File Path";
            this.txtLogPath.Click += new System.EventHandler(this.txtLogFilePath_change);
            // 
            // btnLogPath
            // 
            this.btnLogPath.Location = new System.Drawing.Point(464, 168);
            this.btnLogPath.Name = "btnLogPath";
            this.btnLogPath.Size = new System.Drawing.Size(75, 23);
            this.btnLogPath.TabIndex = 18;
            this.btnLogPath.Text = "Browser";
            this.btnLogPath.UseVisualStyleBackColor = true;
            this.btnLogPath.Click += new System.EventHandler(this.btnLogPath_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 173);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Log File Location:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 282);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnLogPath);
            this.Controls.Add(this.txtLogPath);
            this.Controls.Add(this.numericUpDownLine);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.linkLog);
            this.Controls.Add(this.btnCancelSetting);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnChangeSetting);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnDestinationFolder);
            this.Controls.Add(this.txtDestinationFolder);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTargetFileName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnMonitorFolder);
            this.Controls.Add(this.txtBoxMonitorFolder);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Fetch&Upload Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLine)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBoxMonitorFolder;
        private System.Windows.Forms.Button btnMonitorFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTargetFileName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDestinationFolder;
        private System.Windows.Forms.Button btnDestinationFolder;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnChangeSetting;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancelSetting;
        private System.Windows.Forms.LinkLabel linkLog;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownLine;
        private System.Windows.Forms.Label txtLogPath;
        private System.Windows.Forms.Button btnLogPath;
        private System.Windows.Forms.Label label5;
    }
}


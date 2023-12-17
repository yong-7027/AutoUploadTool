namespace AutoUploadTool
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
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Monitoring Folder";
            // 
            // txtBoxMonitorFolder
            // 
            this.txtBoxMonitorFolder.Location = new System.Drawing.Point(25, 54);
            this.txtBoxMonitorFolder.Name = "txtBoxMonitorFolder";
            this.txtBoxMonitorFolder.Size = new System.Drawing.Size(433, 20);
            this.txtBoxMonitorFolder.TabIndex = 1;
            // 
            // btnMonitorFolder
            // 
            this.btnMonitorFolder.Location = new System.Drawing.Point(487, 54);
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
            this.label2.Location = new System.Drawing.Point(25, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Target File Name";
            // 
            // txtTargetFileName
            // 
            this.txtTargetFileName.Location = new System.Drawing.Point(25, 111);
            this.txtTargetFileName.Name = "txtTargetFileName";
            this.txtTargetFileName.Size = new System.Drawing.Size(205, 20);
            this.txtTargetFileName.TabIndex = 4;
            this.txtTargetFileName.TextChanged += new System.EventHandler(this.txtTargetFileName_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Destination Folder";
            // 
            // txtDestinationFolder
            // 
            this.txtDestinationFolder.Location = new System.Drawing.Point(25, 175);
            this.txtDestinationFolder.Name = "txtDestinationFolder";
            this.txtDestinationFolder.Size = new System.Drawing.Size(433, 20);
            this.txtDestinationFolder.TabIndex = 6;
            this.txtDestinationFolder.TextChanged += new System.EventHandler(this.txtDestinationFolder_TextChanged);
            // 
            // btnDestinationFolder
            // 
            this.btnDestinationFolder.Location = new System.Drawing.Point(487, 175);
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
            this.btnStart.Location = new System.Drawing.Point(25, 243);
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
            this.btnStop.Location = new System.Drawing.Point(154, 242);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 9;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(349, 242);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
            this.Name = "Form1";
            this.Text = "Auto Upload Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
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
    }
}


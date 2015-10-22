namespace UserAdminApp
{
    partial class AppUI
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
            this.browseExcel = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.AddData = new System.Windows.Forms.Button();
            this.DeleteData = new System.Windows.Forms.Button();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.logPanel = new System.Windows.Forms.Panel();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.headerPanel.SuspendLayout();
            this.logPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // browseExcel
            // 
            this.browseExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.browseExcel.Location = new System.Drawing.Point(234, 279);
            this.browseExcel.Name = "browseExcel";
            this.browseExcel.Size = new System.Drawing.Size(83, 26);
            this.browseExcel.TabIndex = 0;
            this.browseExcel.Text = "Browse File";
            this.browseExcel.UseVisualStyleBackColor = true;
            this.browseExcel.Click += new System.EventHandler(this.browseExcel_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // AddData
            // 
            this.AddData.Enabled = false;
            this.AddData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddData.Location = new System.Drawing.Point(234, 311);
            this.AddData.Name = "AddData";
            this.AddData.Size = new System.Drawing.Size(83, 26);
            this.AddData.TabIndex = 1;
            this.AddData.Text = "Add User(s)";
            this.AddData.UseVisualStyleBackColor = true;
            this.AddData.Click += new System.EventHandler(this.AddData_Click);
            // 
            // DeleteData
            // 
            this.DeleteData.Enabled = false;
            this.DeleteData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DeleteData.Location = new System.Drawing.Point(143, 311);
            this.DeleteData.Name = "DeleteData";
            this.DeleteData.Size = new System.Drawing.Size(85, 26);
            this.DeleteData.TabIndex = 2;
            this.DeleteData.Text = "Delete Data";
            this.DeleteData.UseVisualStyleBackColor = true;
            this.DeleteData.Click += new System.EventHandler(this.DeleteData_Click);
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.Gold;
            this.headerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.headerPanel.Controls.Add(this.richTextBox1);
            this.headerPanel.Location = new System.Drawing.Point(13, 12);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(304, 66);
            this.headerPanel.TabIndex = 4;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.Gold;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.richTextBox1.Location = new System.Drawing.Point(4, 17);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(293, 35);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "     User Admin Application";
            // 
            // txtFilePath
            // 
            this.txtFilePath.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilePath.Location = new System.Drawing.Point(13, 279);
            this.txtFilePath.Multiline = true;
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(215, 26);
            this.txtFilePath.TabIndex = 5;
            this.txtFilePath.WordWrap = false;
            // 
            // logPanel
            // 
            this.logPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logPanel.Controls.Add(this.txtLog);
            this.logPanel.Location = new System.Drawing.Point(13, 85);
            this.logPanel.Name = "logPanel";
            this.logPanel.Size = new System.Drawing.Size(304, 188);
            this.logPanel.TabIndex = 6;
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtLog.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtLog.Location = new System.Drawing.Point(-1, -1);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(304, 188);
            this.txtLog.TabIndex = 0;
            this.txtLog.Text = "Application Running..";
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearLogs.Location = new System.Drawing.Point(62, 311);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(75, 26);
            this.btnClearLogs.TabIndex = 7;
            this.btnClearLogs.Text = "Clear Logs";
            this.btnClearLogs.UseVisualStyleBackColor = true;
            this.btnClearLogs.Click += new System.EventHandler(this.btnClearLogs_Click);
            // 
            // AppUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(329, 349);
            this.Controls.Add(this.btnClearLogs);
            this.Controls.Add(this.logPanel);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.DeleteData);
            this.Controls.Add(this.AddData);
            this.Controls.Add(this.browseExcel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AppUI";
            this.Text = "L&T Infotech";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CloseForm);
            this.headerPanel.ResumeLayout(false);
            this.logPanel.ResumeLayout(false);
            this.logPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button browseExcel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button AddData;
        private System.Windows.Forms.Button DeleteData;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Panel logPanel;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnClearLogs;
    }
}


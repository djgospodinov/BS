namespace BS.Client
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtCreatedLicenseId = new System.Windows.Forms.TextBox();
            this.btnCreateLicense = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtRequest = new System.Windows.Forms.TextBox();
            this.txtRealOutput = new System.Windows.Forms.TextBox();
            this.txtJson = new System.Windows.Forms.TextBox();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.txtLicenseId = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(1, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(937, 670);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.txtRequest);
            this.tabPage1.Controls.Add(this.txtCreatedLicenseId);
            this.tabPage1.Controls.Add(this.btnCreateLicense);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(929, 644);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Създаване";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtRealOutput);
            this.tabPage2.Controls.Add(this.txtJson);
            this.tabPage2.Controls.Add(this.txtInfo);
            this.tabPage2.Controls.Add(this.txtLicenseId);
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(929, 644);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Информация";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtCreatedLicenseId
            // 
            this.txtCreatedLicenseId.Location = new System.Drawing.Point(484, 520);
            this.txtCreatedLicenseId.Name = "txtCreatedLicenseId";
            this.txtCreatedLicenseId.Size = new System.Drawing.Size(230, 20);
            this.txtCreatedLicenseId.TabIndex = 2;
            // 
            // btnCreateLicense
            // 
            this.btnCreateLicense.Location = new System.Drawing.Point(52, 522);
            this.btnCreateLicense.Name = "btnCreateLicense";
            this.btnCreateLicense.Size = new System.Drawing.Size(152, 23);
            this.btnCreateLicense.TabIndex = 0;
            this.btnCreateLicense.Text = "Създай лиценз";
            this.btnCreateLicense.UseVisualStyleBackColor = true;
            this.btnCreateLicense.Click += new System.EventHandler(this.btnCreateLicense_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(371, 527);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Лиценз ID";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(836, 271);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Създаване";
            // 
            // txtRequest
            // 
            this.txtRequest.Location = new System.Drawing.Point(13, 18);
            this.txtRequest.Multiline = true;
            this.txtRequest.Name = "txtRequest";
            this.txtRequest.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRequest.Size = new System.Drawing.Size(892, 460);
            this.txtRequest.TabIndex = 7;
            // 
            // txtRealOutput
            // 
            this.txtRealOutput.Location = new System.Drawing.Point(13, 71);
            this.txtRealOutput.Multiline = true;
            this.txtRealOutput.Name = "txtRealOutput";
            this.txtRealOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRealOutput.Size = new System.Drawing.Size(897, 165);
            this.txtRealOutput.TabIndex = 14;
            // 
            // txtJson
            // 
            this.txtJson.Location = new System.Drawing.Point(13, 394);
            this.txtJson.Multiline = true;
            this.txtJson.Name = "txtJson";
            this.txtJson.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtJson.Size = new System.Drawing.Size(897, 244);
            this.txtJson.TabIndex = 13;
            this.txtJson.TextChanged += new System.EventHandler(this.txtJson_TextChanged);
            // 
            // txtInfo
            // 
            this.txtInfo.Location = new System.Drawing.Point(13, 242);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInfo.Size = new System.Drawing.Size(897, 136);
            this.txtInfo.TabIndex = 12;
            // 
            // txtLicenseId
            // 
            this.txtLicenseId.Location = new System.Drawing.Point(249, 21);
            this.txtLicenseId.Name = "txtLicenseId";
            this.txtLicenseId.Size = new System.Drawing.Size(230, 20);
            this.txtLicenseId.TabIndex = 11;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(13, 18);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(205, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Информация за лиценз";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 674);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "License Api Tester v 0.0.3";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRequest;
        private System.Windows.Forms.TextBox txtCreatedLicenseId;
        private System.Windows.Forms.Button btnCreateLicense;
        private System.Windows.Forms.TextBox txtRealOutput;
        private System.Windows.Forms.TextBox txtJson;
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.TextBox txtLicenseId;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}


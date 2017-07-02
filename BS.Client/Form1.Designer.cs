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
            this.btnCreateLicense = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtCreatedLicenseId = new System.Windows.Forms.TextBox();
            this.txtLicenseId = new System.Windows.Forms.TextBox();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCreateLicense
            // 
            this.btnCreateLicense.Location = new System.Drawing.Point(25, 12);
            this.btnCreateLicense.Name = "btnCreateLicense";
            this.btnCreateLicense.Size = new System.Drawing.Size(75, 23);
            this.btnCreateLicense.TabIndex = 0;
            this.btnCreateLicense.Text = "Create";
            this.btnCreateLicense.UseVisualStyleBackColor = true;
            this.btnCreateLicense.Click += new System.EventHandler(this.btnCreateLicense_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(25, 54);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Get License";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtCreatedLicenseId
            // 
            this.txtCreatedLicenseId.Location = new System.Drawing.Point(136, 15);
            this.txtCreatedLicenseId.Name = "txtCreatedLicenseId";
            this.txtCreatedLicenseId.Size = new System.Drawing.Size(230, 20);
            this.txtCreatedLicenseId.TabIndex = 2;
            // 
            // txtLicenseId
            // 
            this.txtLicenseId.Location = new System.Drawing.Point(136, 56);
            this.txtLicenseId.Name = "txtLicenseId";
            this.txtLicenseId.Size = new System.Drawing.Size(230, 20);
            this.txtLicenseId.TabIndex = 3;
            // 
            // txtInfo
            // 
            this.txtInfo.Location = new System.Drawing.Point(25, 103);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.Size = new System.Drawing.Size(341, 112);
            this.txtInfo.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 261);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.txtLicenseId);
            this.Controls.Add(this.txtCreatedLicenseId);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnCreateLicense);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreateLicense;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtCreatedLicenseId;
        private System.Windows.Forms.TextBox txtLicenseId;
        private System.Windows.Forms.TextBox txtInfo;
    }
}


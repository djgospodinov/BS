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
            this.label1 = new System.Windows.Forms.Label();
            this.checkedListModules = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbSSL = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCompanyId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtContactPerson = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.chkCompany = new System.Windows.Forms.CheckBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnAuthenticate = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCreateLicense
            // 
            this.btnCreateLicense.Location = new System.Drawing.Point(52, 153);
            this.btnCreateLicense.Name = "btnCreateLicense";
            this.btnCreateLicense.Size = new System.Drawing.Size(205, 23);
            this.btnCreateLicense.TabIndex = 0;
            this.btnCreateLicense.Text = "Създай лиценз";
            this.btnCreateLicense.UseVisualStyleBackColor = true;
            this.btnCreateLicense.Click += new System.EventHandler(this.btnCreateLicense_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(25, 250);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(205, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Информация за лиценз";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtCreatedLicenseId
            // 
            this.txtCreatedLicenseId.Location = new System.Drawing.Point(368, 156);
            this.txtCreatedLicenseId.Name = "txtCreatedLicenseId";
            this.txtCreatedLicenseId.Size = new System.Drawing.Size(230, 20);
            this.txtCreatedLicenseId.TabIndex = 2;
            // 
            // txtLicenseId
            // 
            this.txtLicenseId.Location = new System.Drawing.Point(261, 253);
            this.txtLicenseId.Name = "txtLicenseId";
            this.txtLicenseId.Size = new System.Drawing.Size(230, 20);
            this.txtLicenseId.TabIndex = 3;
            // 
            // txtInfo
            // 
            this.txtInfo.Location = new System.Drawing.Point(25, 299);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInfo.Size = new System.Drawing.Size(814, 163);
            this.txtInfo.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(291, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Лиценз ID";
            // 
            // checkedListModules
            // 
            this.checkedListModules.FormattingEnabled = true;
            this.checkedListModules.Location = new System.Drawing.Point(510, 19);
            this.checkedListModules.Name = "checkedListModules";
            this.checkedListModules.Size = new System.Drawing.Size(307, 109);
            this.checkedListModules.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAuthenticate);
            this.groupBox1.Controls.Add(this.cbSSL);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtCompanyId);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtContactPerson);
            this.groupBox1.Controls.Add(this.txtEmail);
            this.groupBox1.Controls.Add(this.txtPhone);
            this.groupBox1.Controls.Add(this.chkCompany);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.checkedListModules);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnCreateLicense);
            this.groupBox1.Controls.Add(this.txtCreatedLicenseId);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(836, 195);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Създаване";
            // 
            // cbSSL
            // 
            this.cbSSL.AutoSize = true;
            this.cbSSL.Location = new System.Drawing.Point(363, 52);
            this.cbSSL.Name = "cbSSL";
            this.cbSSL.Size = new System.Drawing.Size(46, 17);
            this.cbSSL.TabIndex = 18;
            this.cbSSL.Text = "SSL";
            this.cbSSL.UseVisualStyleBackColor = true;
            this.cbSSL.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "БУЛСТАТ";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // txtCompanyId
            // 
            this.txtCompanyId.Location = new System.Drawing.Point(140, 121);
            this.txtCompanyId.Name = "txtCompanyId";
            this.txtCompanyId.Size = new System.Drawing.Size(210, 20);
            this.txtCompanyId.TabIndex = 16;
            this.txtCompanyId.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Лице за контакти";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "E-mail";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Телефон";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Име/Фирма";
            // 
            // txtContactPerson
            // 
            this.txtContactPerson.Location = new System.Drawing.Point(140, 96);
            this.txtContactPerson.Name = "txtContactPerson";
            this.txtContactPerson.Size = new System.Drawing.Size(210, 20);
            this.txtContactPerson.TabIndex = 11;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(140, 70);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(210, 20);
            this.txtEmail.TabIndex = 10;
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(140, 44);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(210, 20);
            this.txtPhone.TabIndex = 9;
            // 
            // chkCompany
            // 
            this.chkCompany.AutoSize = true;
            this.chkCompany.Location = new System.Drawing.Point(363, 21);
            this.chkCompany.Name = "chkCompany";
            this.chkCompany.Size = new System.Drawing.Size(63, 17);
            this.chkCompany.TabIndex = 8;
            this.chkCompany.Text = "Фирма";
            this.chkCompany.UseVisualStyleBackColor = true;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(140, 18);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(210, 20);
            this.txtName.TabIndex = 7;
            // 
            // btnAuthenticate
            // 
            this.btnAuthenticate.Location = new System.Drawing.Point(363, 75);
            this.btnAuthenticate.Name = "btnAuthenticate";
            this.btnAuthenticate.Size = new System.Drawing.Size(93, 23);
            this.btnAuthenticate.TabIndex = 19;
            this.btnAuthenticate.Text = "Login";
            this.btnAuthenticate.UseVisualStyleBackColor = true;
            this.btnAuthenticate.Click += new System.EventHandler(this.btnAuthenticate_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 474);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.txtLicenseId);
            this.Controls.Add(this.button2);
            this.Name = "Form1";
            this.Text = "License Api Tester v 0.0.1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreateLicense;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtCreatedLicenseId;
        private System.Windows.Forms.TextBox txtLicenseId;
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox checkedListModules;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkCompany;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtContactPerson;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCompanyId;
        private System.Windows.Forms.CheckBox cbSSL;
        private System.Windows.Forms.Button btnAuthenticate;
    }
}


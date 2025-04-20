using Vibely_App.Controls;

namespace Vibely_App.View
{
    partial class RegisterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterForm));
            txtRegisterUsername = new TextBox();
            txtRegisterNames = new TextBox();
            txtRegisterPassword = new TextBox();
            txtRegisterPhoneNumber = new TextBox();
            txtRegisterEmail = new TextBox();
            lblRegisterUsername = new Label();
            lblRegisterPassword = new Label();
            lblRegisterNames = new Label();
            lblRegisterPhoneNumber = new Label();
            lblRegisterEmail = new Label();
            lblRegisterProfilePicture = new Label();
            btnRegisterUpload = new Button();
            btnRegisterRegister = new Button();
            lblRegisterTitle = new Label();
            gradientPanel1 = new GradientPanel();
            pctrUser = new PictureBox();
            fileDlg = new OpenFileDialog();
            gradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pctrUser).BeginInit();
            SuspendLayout();
            // 
            // txtRegisterUsername
            // 
            txtRegisterUsername.BackColor = Color.Purple;
            txtRegisterUsername.BorderStyle = BorderStyle.None;
            txtRegisterUsername.Font = new Font("Arial Rounded MT Bold", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtRegisterUsername.ForeColor = SystemColors.ButtonHighlight;
            txtRegisterUsername.Location = new Point(119, 106);
            txtRegisterUsername.Margin = new Padding(3, 3, 3, 2);
            txtRegisterUsername.Name = "txtRegisterUsername";
            txtRegisterUsername.Size = new Size(194, 17);
            txtRegisterUsername.TabIndex = 0;
            // 
            // txtRegisterNames
            // 
            txtRegisterNames.BackColor = Color.Purple;
            txtRegisterNames.BorderStyle = BorderStyle.None;
            txtRegisterNames.Font = new Font("Arial Rounded MT Bold", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtRegisterNames.ForeColor = SystemColors.ButtonHighlight;
            txtRegisterNames.Location = new Point(388, 106);
            txtRegisterNames.Name = "txtRegisterNames";
            txtRegisterNames.Size = new Size(194, 17);
            txtRegisterNames.TabIndex = 1;
            // 
            // txtRegisterPassword
            // 
            txtRegisterPassword.BackColor = Color.Purple;
            txtRegisterPassword.BorderStyle = BorderStyle.None;
            txtRegisterPassword.Font = new Font("Arial Rounded MT Bold", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtRegisterPassword.ForeColor = SystemColors.ButtonHighlight;
            txtRegisterPassword.Location = new Point(119, 175);
            txtRegisterPassword.Name = "txtRegisterPassword";
            txtRegisterPassword.Size = new Size(194, 17);
            txtRegisterPassword.TabIndex = 2;
            txtRegisterPassword.UseSystemPasswordChar = true;
            // 
            // txtRegisterPhoneNumber
            // 
            txtRegisterPhoneNumber.BackColor = Color.Purple;
            txtRegisterPhoneNumber.BorderStyle = BorderStyle.None;
            txtRegisterPhoneNumber.Font = new Font("Arial Rounded MT Bold", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtRegisterPhoneNumber.ForeColor = SystemColors.ButtonHighlight;
            txtRegisterPhoneNumber.Location = new Point(388, 175);
            txtRegisterPhoneNumber.Name = "txtRegisterPhoneNumber";
            txtRegisterPhoneNumber.Size = new Size(194, 17);
            txtRegisterPhoneNumber.TabIndex = 3;
            // 
            // txtRegisterEmail
            // 
            txtRegisterEmail.BackColor = Color.Purple;
            txtRegisterEmail.BorderStyle = BorderStyle.None;
            txtRegisterEmail.Font = new Font("Arial Rounded MT Bold", 10.2F);
            txtRegisterEmail.ForeColor = SystemColors.ButtonHighlight;
            txtRegisterEmail.Location = new Point(388, 235);
            txtRegisterEmail.Name = "txtRegisterEmail";
            txtRegisterEmail.Size = new Size(194, 16);
            txtRegisterEmail.TabIndex = 4;
            // 
            // lblRegisterUsername
            // 
            lblRegisterUsername.AutoSize = true;
            lblRegisterUsername.BackColor = Color.Transparent;
            lblRegisterUsername.Font = new Font("Arial Rounded MT Bold", 10.2F);
            lblRegisterUsername.ForeColor = SystemColors.ButtonHighlight;
            lblRegisterUsername.Location = new Point(116, 88);
            lblRegisterUsername.Name = "lblRegisterUsername";
            lblRegisterUsername.Size = new Size(76, 16);
            lblRegisterUsername.TabIndex = 5;
            lblRegisterUsername.Text = "Username";
            // 
            // lblRegisterPassword
            // 
            lblRegisterPassword.AutoSize = true;
            lblRegisterPassword.BackColor = Color.Transparent;
            lblRegisterPassword.Font = new Font("Arial Rounded MT Bold", 10.2F);
            lblRegisterPassword.ForeColor = SystemColors.ButtonHighlight;
            lblRegisterPassword.Location = new Point(116, 157);
            lblRegisterPassword.Name = "lblRegisterPassword";
            lblRegisterPassword.Size = new Size(74, 16);
            lblRegisterPassword.TabIndex = 6;
            lblRegisterPassword.Text = "Password";
            // 
            // lblRegisterNames
            // 
            lblRegisterNames.AutoSize = true;
            lblRegisterNames.BackColor = Color.Transparent;
            lblRegisterNames.Font = new Font("Arial Rounded MT Bold", 10.2F);
            lblRegisterNames.ForeColor = SystemColors.ButtonHighlight;
            lblRegisterNames.Location = new Point(385, 88);
            lblRegisterNames.Name = "lblRegisterNames";
            lblRegisterNames.Size = new Size(136, 16);
            lblRegisterNames.TabIndex = 7;
            lblRegisterNames.Text = "First and last name";
            // 
            // lblRegisterPhoneNumber
            // 
            lblRegisterPhoneNumber.AutoSize = true;
            lblRegisterPhoneNumber.BackColor = Color.Transparent;
            lblRegisterPhoneNumber.Font = new Font("Arial Rounded MT Bold", 10.2F);
            lblRegisterPhoneNumber.ForeColor = SystemColors.ButtonHighlight;
            lblRegisterPhoneNumber.Location = new Point(385, 157);
            lblRegisterPhoneNumber.Name = "lblRegisterPhoneNumber";
            lblRegisterPhoneNumber.Size = new Size(103, 16);
            lblRegisterPhoneNumber.TabIndex = 8;
            lblRegisterPhoneNumber.Text = "Phone number";
            // 
            // lblRegisterEmail
            // 
            lblRegisterEmail.AutoSize = true;
            lblRegisterEmail.BackColor = Color.Transparent;
            lblRegisterEmail.Font = new Font("Arial Rounded MT Bold", 10.2F);
            lblRegisterEmail.ForeColor = SystemColors.ButtonHighlight;
            lblRegisterEmail.Location = new Point(385, 217);
            lblRegisterEmail.Name = "lblRegisterEmail";
            lblRegisterEmail.Size = new Size(49, 16);
            lblRegisterEmail.TabIndex = 9;
            lblRegisterEmail.Text = "E-mail";
            // 
            // lblRegisterProfilePicture
            // 
            lblRegisterProfilePicture.AutoSize = true;
            lblRegisterProfilePicture.BackColor = Color.Transparent;
            lblRegisterProfilePicture.Font = new Font("Arial Rounded MT Bold", 10.2F);
            lblRegisterProfilePicture.ForeColor = SystemColors.ButtonHighlight;
            lblRegisterProfilePicture.Location = new Point(130, 217);
            lblRegisterProfilePicture.Name = "lblRegisterProfilePicture";
            lblRegisterProfilePicture.Size = new Size(107, 16);
            lblRegisterProfilePicture.TabIndex = 10;
            lblRegisterProfilePicture.Text = "Profile picture:";
            // 
            // btnRegisterUpload
            // 
            btnRegisterUpload.BackColor = Color.Purple;
            btnRegisterUpload.FlatAppearance.BorderColor = Color.Black;
            btnRegisterUpload.FlatStyle = FlatStyle.Flat;
            btnRegisterUpload.Font = new Font("Arial Rounded MT Bold", 10.2F);
            btnRegisterUpload.ForeColor = SystemColors.ButtonHighlight;
            btnRegisterUpload.Location = new Point(130, 235);
            btnRegisterUpload.Name = "btnRegisterUpload";
            btnRegisterUpload.Size = new Size(115, 26);
            btnRegisterUpload.TabIndex = 11;
            btnRegisterUpload.Text = "Upload";
            btnRegisterUpload.UseVisualStyleBackColor = false;
            btnRegisterUpload.Click += btnRegisterUpload_Click;
            // 
            // btnRegisterRegister
            // 
            btnRegisterRegister.BackColor = Color.Purple;
            btnRegisterRegister.FlatAppearance.BorderColor = Color.Black;
            btnRegisterRegister.FlatStyle = FlatStyle.Flat;
            btnRegisterRegister.Font = new Font("Arial Rounded MT Bold", 10.2F);
            btnRegisterRegister.ForeColor = SystemColors.ButtonHighlight;
            btnRegisterRegister.Location = new Point(293, 282);
            btnRegisterRegister.Name = "btnRegisterRegister";
            btnRegisterRegister.Size = new Size(115, 26);
            btnRegisterRegister.TabIndex = 12;
            btnRegisterRegister.Text = "Register";
            btnRegisterRegister.UseVisualStyleBackColor = false;
            btnRegisterRegister.Click += btnRegisterRegister_Click;
            // 
            // lblRegisterTitle
            // 
            lblRegisterTitle.AutoSize = true;
            lblRegisterTitle.BackColor = Color.Transparent;
            lblRegisterTitle.Font = new Font("Arial Rounded MT Bold", 22.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblRegisterTitle.ForeColor = SystemColors.ButtonHighlight;
            lblRegisterTitle.Location = new Point(275, 29);
            lblRegisterTitle.Name = "lblRegisterTitle";
            lblRegisterTitle.Size = new Size(140, 34);
            lblRegisterTitle.TabIndex = 13;
            lblRegisterTitle.Text = "Register";
            // 
            // gradientPanel1
            // 
            gradientPanel1.Angle = 45F;
            gradientPanel1.BackColor = Color.Transparent;
            gradientPanel1.Controls.Add(pctrUser);
            gradientPanel1.Controls.Add(btnRegisterRegister);
            gradientPanel1.Controls.Add(lblRegisterEmail);
            gradientPanel1.Controls.Add(lblRegisterPhoneNumber);
            gradientPanel1.Controls.Add(lblRegisterNames);
            gradientPanel1.Controls.Add(txtRegisterEmail);
            gradientPanel1.Controls.Add(txtRegisterNames);
            gradientPanel1.Controls.Add(txtRegisterPhoneNumber);
            gradientPanel1.Controls.Add(lblRegisterUsername);
            gradientPanel1.Controls.Add(lblRegisterPassword);
            gradientPanel1.Controls.Add(lblRegisterTitle);
            gradientPanel1.Controls.Add(lblRegisterProfilePicture);
            gradientPanel1.Controls.Add(txtRegisterUsername);
            gradientPanel1.Controls.Add(btnRegisterUpload);
            gradientPanel1.Controls.Add(txtRegisterPassword);
            gradientPanel1.EndColor = Color.FromArgb(0, 0, 64);
            gradientPanel1.Location = new Point(0, 0);
            gradientPanel1.Margin = new Padding(3, 2, 3, 2);
            gradientPanel1.Name = "gradientPanel1";
            gradientPanel1.Size = new Size(701, 338);
            gradientPanel1.StartColor = Color.FromArgb(64, 0, 64);
            gradientPanel1.TabIndex = 14;
            // 
            // pctrUser
            // 
            pctrUser.Location = new Point(251, 208);
            pctrUser.Name = "pctrUser";
            pctrUser.Size = new Size(53, 53);
            pctrUser.SizeMode = PictureBoxSizeMode.StretchImage;
            pctrUser.TabIndex = 14;
            pctrUser.TabStop = false;
            // 
            // fileDlg
            // 
            fileDlg.FileName = "fileDlg";
            // 
            // RegisterForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonShadow;
            ClientSize = new Size(700, 338);
            Controls.Add(gradientPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "RegisterForm";
            Text = "Vibely";
            TopMost = true;
            gradientPanel1.ResumeLayout(false);
            gradientPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pctrUser).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TextBox txtRegisterUsername;
        private TextBox txtRegisterNames;
        private TextBox txtRegisterPassword;
        private TextBox txtRegisterPhoneNumber;
        private TextBox txtRegisterEmail;
        private Label lblRegisterUsername;
        private Label lblRegisterPassword;
        private Label lblRegisterNames;
        private Label lblRegisterPhoneNumber;
        private Label lblRegisterEmail;
        private Label lblRegisterProfilePicture;
        private Button btnRegisterUpload;
        private Button btnRegisterRegister;
        private Label lblRegisterTitle;
        private GradientPanel gradientPanel1;
        private OpenFileDialog fileDlg;
        private PictureBox pctrUser;
    }
}
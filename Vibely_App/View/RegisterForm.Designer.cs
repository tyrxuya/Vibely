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
            gradientPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // txtRegisterUsername
            // 
            txtRegisterUsername.BackColor = Color.Purple;
            txtRegisterUsername.BorderStyle = BorderStyle.None;
            txtRegisterUsername.Font = new Font("Arial Rounded MT Bold", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtRegisterUsername.ForeColor = SystemColors.ButtonHighlight;
            txtRegisterUsername.Location = new Point(136, 141);
            txtRegisterUsername.Margin = new Padding(3, 4, 3, 3);
            txtRegisterUsername.Name = "txtRegisterUsername";
            txtRegisterUsername.Size = new Size(222, 21);
            txtRegisterUsername.TabIndex = 0;
            // 
            // txtRegisterNames
            // 
            txtRegisterNames.BackColor = Color.Purple;
            txtRegisterNames.BorderStyle = BorderStyle.None;
            txtRegisterNames.Font = new Font("Arial Rounded MT Bold", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtRegisterNames.ForeColor = SystemColors.ButtonHighlight;
            txtRegisterNames.Location = new Point(443, 141);
            txtRegisterNames.Margin = new Padding(3, 4, 3, 4);
            txtRegisterNames.Name = "txtRegisterNames";
            txtRegisterNames.Size = new Size(222, 21);
            txtRegisterNames.TabIndex = 1;
            // 
            // txtRegisterPassword
            // 
            txtRegisterPassword.BackColor = Color.Purple;
            txtRegisterPassword.BorderStyle = BorderStyle.None;
            txtRegisterPassword.Font = new Font("Arial Rounded MT Bold", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtRegisterPassword.ForeColor = SystemColors.ButtonHighlight;
            txtRegisterPassword.Location = new Point(136, 233);
            txtRegisterPassword.Margin = new Padding(3, 4, 3, 4);
            txtRegisterPassword.Name = "txtRegisterPassword";
            txtRegisterPassword.Size = new Size(222, 21);
            txtRegisterPassword.TabIndex = 2;
            txtRegisterPassword.UseSystemPasswordChar = true;
            // 
            // txtRegisterPhoneNumber
            // 
            txtRegisterPhoneNumber.BackColor = Color.Purple;
            txtRegisterPhoneNumber.BorderStyle = BorderStyle.None;
            txtRegisterPhoneNumber.Font = new Font("Arial Rounded MT Bold", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtRegisterPhoneNumber.ForeColor = SystemColors.ButtonHighlight;
            txtRegisterPhoneNumber.Location = new Point(443, 233);
            txtRegisterPhoneNumber.Margin = new Padding(3, 4, 3, 4);
            txtRegisterPhoneNumber.Name = "txtRegisterPhoneNumber";
            txtRegisterPhoneNumber.Size = new Size(222, 21);
            txtRegisterPhoneNumber.TabIndex = 3;
            // 
            // txtRegisterEmail
            // 
            txtRegisterEmail.BackColor = Color.Purple;
            txtRegisterEmail.BorderStyle = BorderStyle.None;
            txtRegisterEmail.Font = new Font("Arial Rounded MT Bold", 10.2F);
            txtRegisterEmail.Location = new Point(443, 313);
            txtRegisterEmail.Margin = new Padding(3, 4, 3, 4);
            txtRegisterEmail.Name = "txtRegisterEmail";
            txtRegisterEmail.Size = new Size(222, 20);
            txtRegisterEmail.TabIndex = 4;
            // 
            // lblRegisterUsername
            // 
            lblRegisterUsername.AutoSize = true;
            lblRegisterUsername.BackColor = Color.Transparent;
            lblRegisterUsername.Font = new Font("Arial Rounded MT Bold", 10.2F);
            lblRegisterUsername.ForeColor = SystemColors.ButtonHighlight;
            lblRegisterUsername.Location = new Point(136, 117);
            lblRegisterUsername.Name = "lblRegisterUsername";
            lblRegisterUsername.Size = new Size(93, 20);
            lblRegisterUsername.TabIndex = 5;
            lblRegisterUsername.Text = "Username";
            // 
            // lblRegisterPassword
            // 
            lblRegisterPassword.AutoSize = true;
            lblRegisterPassword.BackColor = Color.Transparent;
            lblRegisterPassword.Font = new Font("Arial Rounded MT Bold", 10.2F);
            lblRegisterPassword.ForeColor = SystemColors.ButtonHighlight;
            lblRegisterPassword.Location = new Point(136, 209);
            lblRegisterPassword.Name = "lblRegisterPassword";
            lblRegisterPassword.Size = new Size(90, 20);
            lblRegisterPassword.TabIndex = 6;
            lblRegisterPassword.Text = "Password";
            // 
            // lblRegisterNames
            // 
            lblRegisterNames.AutoSize = true;
            lblRegisterNames.BackColor = Color.Transparent;
            lblRegisterNames.Font = new Font("Arial Rounded MT Bold", 10.2F);
            lblRegisterNames.ForeColor = SystemColors.ButtonHighlight;
            lblRegisterNames.Location = new Point(443, 117);
            lblRegisterNames.Name = "lblRegisterNames";
            lblRegisterNames.Size = new Size(164, 20);
            lblRegisterNames.TabIndex = 7;
            lblRegisterNames.Text = "First and last name";
            // 
            // lblRegisterPhoneNumber
            // 
            lblRegisterPhoneNumber.AutoSize = true;
            lblRegisterPhoneNumber.BackColor = Color.Transparent;
            lblRegisterPhoneNumber.Font = new Font("Arial Rounded MT Bold", 10.2F);
            lblRegisterPhoneNumber.ForeColor = SystemColors.ButtonHighlight;
            lblRegisterPhoneNumber.Location = new Point(443, 209);
            lblRegisterPhoneNumber.Name = "lblRegisterPhoneNumber";
            lblRegisterPhoneNumber.Size = new Size(127, 20);
            lblRegisterPhoneNumber.TabIndex = 8;
            lblRegisterPhoneNumber.Text = "Phone number";
            // 
            // lblRegisterEmail
            // 
            lblRegisterEmail.AutoSize = true;
            lblRegisterEmail.BackColor = Color.Transparent;
            lblRegisterEmail.Font = new Font("Arial Rounded MT Bold", 10.2F);
            lblRegisterEmail.ForeColor = SystemColors.ButtonHighlight;
            lblRegisterEmail.Location = new Point(443, 289);
            lblRegisterEmail.Name = "lblRegisterEmail";
            lblRegisterEmail.Size = new Size(61, 20);
            lblRegisterEmail.TabIndex = 9;
            lblRegisterEmail.Text = "E-mail";
            // 
            // lblRegisterProfilePicture
            // 
            lblRegisterProfilePicture.AutoSize = true;
            lblRegisterProfilePicture.BackColor = Color.Transparent;
            lblRegisterProfilePicture.Font = new Font("Arial Rounded MT Bold", 10.2F);
            lblRegisterProfilePicture.ForeColor = SystemColors.ButtonHighlight;
            lblRegisterProfilePicture.Location = new Point(159, 289);
            lblRegisterProfilePicture.Name = "lblRegisterProfilePicture";
            lblRegisterProfilePicture.Size = new Size(131, 20);
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
            btnRegisterUpload.Location = new Point(159, 313);
            btnRegisterUpload.Margin = new Padding(3, 4, 3, 4);
            btnRegisterUpload.Name = "btnRegisterUpload";
            btnRegisterUpload.Size = new Size(131, 29);
            btnRegisterUpload.TabIndex = 11;
            btnRegisterUpload.Text = "Upload";
            btnRegisterUpload.UseVisualStyleBackColor = false;
            // 
            // btnRegisterRegister
            // 
            btnRegisterRegister.BackColor = Color.Purple;
            btnRegisterRegister.FlatAppearance.BorderColor = Color.Black;
            btnRegisterRegister.FlatStyle = FlatStyle.Flat;
            btnRegisterRegister.Font = new Font("Arial Rounded MT Bold", 10.2F);
            btnRegisterRegister.ForeColor = SystemColors.ButtonHighlight;
            btnRegisterRegister.Location = new Point(335, 376);
            btnRegisterRegister.Margin = new Padding(3, 4, 3, 4);
            btnRegisterRegister.Name = "btnRegisterRegister";
            btnRegisterRegister.Size = new Size(131, 32);
            btnRegisterRegister.TabIndex = 12;
            btnRegisterRegister.Text = "Register";
            btnRegisterRegister.UseVisualStyleBackColor = false;
            // 
            // lblRegisterTitle
            // 
            lblRegisterTitle.AutoSize = true;
            lblRegisterTitle.BackColor = Color.Transparent;
            lblRegisterTitle.Font = new Font("Arial Rounded MT Bold", 22.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblRegisterTitle.ForeColor = SystemColors.ButtonHighlight;
            lblRegisterTitle.Location = new Point(314, 39);
            lblRegisterTitle.Name = "lblRegisterTitle";
            lblRegisterTitle.Size = new Size(172, 43);
            lblRegisterTitle.TabIndex = 13;
            lblRegisterTitle.Text = "Register";
            // 
            // gradientPanel1
            // 
            gradientPanel1.Angle = 45F;
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
            gradientPanel1.Name = "gradientPanel1";
            gradientPanel1.Size = new Size(801, 451);
            gradientPanel1.StartColor = Color.FromArgb(64, 0, 64);
            gradientPanel1.TabIndex = 14;
            // 
            // RegisterForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonShadow;
            ClientSize = new Size(800, 451);
            Controls.Add(gradientPanel1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "RegisterForm";
            Text = "RegisterForm";
            TopMost = true;
            Load += RegisterForm_Load;
            gradientPanel1.ResumeLayout(false);
            gradientPanel1.PerformLayout();
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
    }
}
using Vibely_App.Controls;

namespace Vibely_App.View
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            btnRegister = new Button();
            lblNoAccount = new Label();
            btnLogin = new Button();
            lblPassword = new Label();
            txtLoginPassword = new TextBox();
            lblHeading = new Label();
            txtLoginUsername = new TextBox();
            lblUsername = new Label();
            gradientPanel1 = new GradientPanel();
            gradientPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // btnRegister
            // 
            btnRegister.BackColor = Color.Purple;
            btnRegister.FlatAppearance.BorderColor = Color.Black;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.Font = new Font("Arial Rounded MT Bold", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRegister.ForeColor = SystemColors.ButtonHighlight;
            btnRegister.Location = new Point(290, 275);
            btnRegister.Margin = new Padding(3, 2, 3, 2);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(96, 26);
            btnRegister.TabIndex = 4;
            btnRegister.Text = "Register";
            btnRegister.UseVisualStyleBackColor = false;
            btnRegister.Click += btnRegister_Click;
            // 
            // lblNoAccount
            // 
            lblNoAccount.AutoSize = true;
            lblNoAccount.BackColor = Color.Transparent;
            lblNoAccount.Font = new Font("Arial Rounded MT Bold", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNoAccount.ForeColor = SystemColors.ButtonFace;
            lblNoAccount.Location = new Point(274, 256);
            lblNoAccount.Name = "lblNoAccount";
            lblNoAccount.Size = new Size(136, 12);
            lblNoAccount.TabIndex = 7;
            lblNoAccount.Text = "Don't have an account?";
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.Purple;
            btnLogin.FlatAppearance.BorderColor = Color.Black;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Arial Rounded MT Bold", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLogin.ForeColor = SystemColors.ButtonHighlight;
            btnLogin.Location = new Point(290, 224);
            btnLogin.Margin = new Padding(3, 2, 3, 2);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(96, 26);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.BackColor = Color.Transparent;
            lblPassword.Font = new Font("Arial Rounded MT Bold", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPassword.ForeColor = SystemColors.ButtonHighlight;
            lblPassword.Location = new Point(256, 161);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(74, 16);
            lblPassword.TabIndex = 6;
            lblPassword.Text = "Password";
            // 
            // txtLoginPassword
            // 
            txtLoginPassword.BackColor = Color.Purple;
            txtLoginPassword.BorderStyle = BorderStyle.None;
            txtLoginPassword.Font = new Font("Arial Rounded MT Bold", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtLoginPassword.ForeColor = SystemColors.ButtonHighlight;
            txtLoginPassword.Location = new Point(259, 179);
            txtLoginPassword.Margin = new Padding(3, 2, 3, 2);
            txtLoginPassword.Name = "txtLoginPassword";
            txtLoginPassword.PasswordChar = '*';
            txtLoginPassword.Size = new Size(152, 16);
            txtLoginPassword.TabIndex = 2;
            // 
            // lblHeading
            // 
            lblHeading.AutoSize = true;
            lblHeading.BackColor = Color.Transparent;
            lblHeading.FlatStyle = FlatStyle.Flat;
            lblHeading.Font = new Font("Arial Rounded MT Bold", 22.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblHeading.ForeColor = SystemColors.ButtonHighlight;
            lblHeading.Location = new Point(198, 40);
            lblHeading.Name = "lblHeading";
            lblHeading.Size = new Size(285, 34);
            lblHeading.TabIndex = 0;
            lblHeading.Text = "Welcome to Vibely";
            lblHeading.TextAlign = ContentAlignment.TopCenter;
            // 
            // txtLoginUsername
            // 
            txtLoginUsername.BackColor = Color.Purple;
            txtLoginUsername.BorderStyle = BorderStyle.None;
            txtLoginUsername.Font = new Font("Arial Rounded MT Bold", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtLoginUsername.ForeColor = SystemColors.ButtonHighlight;
            txtLoginUsername.Location = new Point(259, 126);
            txtLoginUsername.Margin = new Padding(3, 2, 3, 2);
            txtLoginUsername.Name = "txtLoginUsername";
            txtLoginUsername.Size = new Size(152, 17);
            txtLoginUsername.TabIndex = 1;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.BackColor = Color.Transparent;
            lblUsername.Font = new Font("Arial Rounded MT Bold", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblUsername.ForeColor = SystemColors.ButtonHighlight;
            lblUsername.Location = new Point(256, 108);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(76, 16);
            lblUsername.TabIndex = 5;
            lblUsername.Text = "Username";
            // 
            // gradientPanel1
            // 
            gradientPanel1.Angle = 45F;
            gradientPanel1.Controls.Add(btnRegister);
            gradientPanel1.Controls.Add(lblNoAccount);
            gradientPanel1.Controls.Add(btnLogin);
            gradientPanel1.Controls.Add(lblPassword);
            gradientPanel1.Controls.Add(txtLoginPassword);
            gradientPanel1.Controls.Add(lblHeading);
            gradientPanel1.Controls.Add(txtLoginUsername);
            gradientPanel1.Controls.Add(lblUsername);
            gradientPanel1.Dock = DockStyle.Top;
            gradientPanel1.EndColor = Color.FromArgb(0, 0, 64);
            gradientPanel1.Location = new Point(0, 0);
            gradientPanel1.Name = "gradientPanel1";
            gradientPanel1.Size = new Size(700, 338);
            gradientPanel1.StartColor = Color.FromArgb(64, 0, 64);
            gradientPanel1.TabIndex = 0;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.MediumOrchid;
            ClientSize = new Size(700, 338);
            Controls.Add(gradientPanel1);
            ForeColor = SystemColors.ActiveCaptionText;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            Name = "LoginForm";
            Text = "Vibely";
            gradientPanel1.ResumeLayout(false);
            gradientPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GradientPanel gradientPanel1;
        private Button btnRegister;
        private Label lblNoAccount;
        private Button btnLogin;
        private Label lblPassword;
        private TextBox txtLoginPassword;
        private Label lblHeading;
        private TextBox txtLoginUsername;
        private Label lblUsername;
    }
}
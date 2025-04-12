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
            lblHeading = new Label();
            gradientPanel1 = new GradientPanel();
            txtLoginUsername = new TextBox();
            txtLoginPassword = new TextBox();
            btnLogin = new Button();
            btnRegister = new Button();
            lblUsername = new Label();
            lblPassword = new Label();
            lblNoAccount = new Label();
            gradientPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // lblHeading
            // 
            lblHeading.AutoSize = true;
            lblHeading.BackColor = Color.Transparent;
            lblHeading.FlatStyle = FlatStyle.Flat;
            lblHeading.Font = new Font("Arial Rounded MT Bold", 22.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblHeading.ForeColor = SystemColors.ButtonHighlight;
            lblHeading.Location = new Point(208, 31);
            lblHeading.Name = "lblHeading";
            lblHeading.Size = new Size(285, 34);
            lblHeading.TabIndex = 0;
            lblHeading.Text = "Welcome to Vibely";
            lblHeading.TextAlign = ContentAlignment.TopCenter;
            // 
            // gradientPanel1
            // 
            gradientPanel1.Angle = 45F;
            gradientPanel1.Controls.Add(lblHeading);
            gradientPanel1.Dock = DockStyle.Top;
            gradientPanel1.EndColor = Color.Black;
            gradientPanel1.Location = new Point(0, 0);
            gradientPanel1.Name = "gradientPanel1";
            gradientPanel1.Size = new Size(700, 338);
            gradientPanel1.StartColor = Color.Purple;
            gradientPanel1.TabIndex = 0;
            // 
            // txtLoginUsername
            // 
            txtLoginUsername.BackColor = Color.DarkMagenta;
            txtLoginUsername.BorderStyle = BorderStyle.None;
            txtLoginUsername.Font = new Font("Arial Rounded MT Bold", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtLoginUsername.ForeColor = SystemColors.ButtonHighlight;
            txtLoginUsername.Location = new Point(272, 110);
            txtLoginUsername.Margin = new Padding(3, 2, 3, 2);
            txtLoginUsername.Name = "txtLoginUsername";
            txtLoginUsername.Size = new Size(152, 17);
            txtLoginUsername.TabIndex = 1;
            // 
            // txtLoginPassword
            // 
            txtLoginPassword.BackColor = Color.Purple;
            txtLoginPassword.BorderStyle = BorderStyle.None;
            txtLoginPassword.Font = new Font("Arial Rounded MT Bold", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtLoginPassword.ForeColor = SystemColors.ButtonHighlight;
            txtLoginPassword.Location = new Point(272, 164);
            txtLoginPassword.Margin = new Padding(3, 2, 3, 2);
            txtLoginPassword.Name = "txtLoginPassword";
            txtLoginPassword.PasswordChar = '*';
            txtLoginPassword.Size = new Size(152, 16);
            txtLoginPassword.TabIndex = 2;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.Purple;
            btnLogin.FlatAppearance.BorderColor = Color.Black;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Arial Rounded MT Bold", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLogin.ForeColor = SystemColors.ButtonHighlight;
            btnLogin.Location = new Point(292, 206);
            btnLogin.Margin = new Padding(3, 2, 3, 2);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(102, 26);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnRegister
            // 
            btnRegister.BackColor = Color.Purple;
            btnRegister.FlatAppearance.BorderColor = Color.Black;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.Font = new Font("Arial Rounded MT Bold", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRegister.ForeColor = SystemColors.ButtonHighlight;
            btnRegister.Location = new Point(292, 260);
            btnRegister.Margin = new Padding(3, 2, 3, 2);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(102, 26);
            btnRegister.TabIndex = 4;
            btnRegister.Text = "Register";
            btnRegister.UseVisualStyleBackColor = false;
            btnRegister.Click += btnRegister_Click;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.BackColor = Color.Transparent;
            lblUsername.Font = new Font("Arial Rounded MT Bold", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblUsername.ForeColor = SystemColors.ButtonHighlight;
            lblUsername.Location = new Point(272, 92);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(76, 16);
            lblUsername.TabIndex = 5;
            lblUsername.Text = "Username";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.BackColor = Color.Transparent;
            lblPassword.Font = new Font("Arial Rounded MT Bold", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPassword.ForeColor = SystemColors.ButtonHighlight;
            lblPassword.Location = new Point(275, 146);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(74, 16);
            lblPassword.TabIndex = 6;
            lblPassword.Text = "Password";
            // 
            // lblNoAccount
            // 
            lblNoAccount.AutoSize = true;
            lblNoAccount.BackColor = Color.Transparent;
            lblNoAccount.Font = new Font("Arial Rounded MT Bold", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNoAccount.ForeColor = SystemColors.ButtonFace;
            lblNoAccount.Location = new Point(275, 242);
            lblNoAccount.Name = "lblNoAccount";
            lblNoAccount.Size = new Size(136, 12);
            lblNoAccount.TabIndex = 7;
            lblNoAccount.Text = "Don't have an account?";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.MediumOrchid;
            ClientSize = new Size(700, 338);
            Controls.Add(gradientPanel1);
            Controls.Add(lblNoAccount);
            Controls.Add(lblPassword);
            Controls.Add(lblUsername);
            Controls.Add(btnRegister);
            Controls.Add(btnLogin);
            Controls.Add(txtLoginPassword);
            Controls.Add(txtLoginUsername);
            ForeColor = SystemColors.ActiveCaptionText;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            Name = "LoginForm";
            Text = "Vibely";
            gradientPanel1.ResumeLayout(false);
            gradientPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion


        private Label lblHeading;
        private TextBox txtLoginUsername;
        private TextBox txtLoginPassword;
        private Button btnLogin;
        private Button btnRegister;
        private Label lblUsername;
        private Label lblPassword;
        private Label lblNoAccount;
        private GradientPanel gradientPanel1;
    }
}
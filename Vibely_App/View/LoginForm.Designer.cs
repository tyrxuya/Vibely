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
            txtLoginUsername = new TextBox();
            txtLoginPassword = new TextBox();
            btnLogin = new Button();
            btnRegister = new Button();
            lblUsername = new Label();
            lblPassword = new Label();
            lblNoAccount = new Label();
            SuspendLayout();
            // 
            // lblHeading
            // 
            lblHeading.AutoSize = true;
            lblHeading.FlatStyle = FlatStyle.Flat;
            lblHeading.Font = new Font("Arial Rounded MT Bold", 22.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblHeading.ForeColor = SystemColors.ButtonHighlight;
            lblHeading.Location = new Point(200, 43);
            lblHeading.Name = "lblHeading";
            lblHeading.Size = new Size(348, 43);
            lblHeading.TabIndex = 0;
            lblHeading.Text = "Welcome to Vibely";
            lblHeading.TextAlign = ContentAlignment.TopCenter;
            // 
            // txtLoginUsername
            // 
            txtLoginUsername.BackColor = Color.DarkMagenta;
            txtLoginUsername.Font = new Font("Arial Rounded MT Bold", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtLoginUsername.ForeColor = SystemColors.ButtonHighlight;
            txtLoginUsername.Location = new Point(288, 146);
            txtLoginUsername.Name = "txtLoginUsername";
            txtLoginUsername.Size = new Size(173, 28);
            txtLoginUsername.TabIndex = 1;
            // 
            // txtLoginPassword
            // 
            txtLoginPassword.BackColor = Color.Purple;
            txtLoginPassword.Font = new Font("Arial Rounded MT Bold", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtLoginPassword.ForeColor = SystemColors.ButtonHighlight;
            txtLoginPassword.Location = new Point(288, 218);
            txtLoginPassword.Name = "txtLoginPassword";
            txtLoginPassword.Size = new Size(173, 27);
            txtLoginPassword.TabIndex = 2;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.Purple;
            btnLogin.Font = new Font("Arial Rounded MT Bold", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLogin.ForeColor = SystemColors.ButtonHighlight;
            btnLogin.Location = new Point(311, 274);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(117, 29);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = false;
            // 
            // btnRegister
            // 
            btnRegister.BackColor = Color.Purple;
            btnRegister.Font = new Font("Arial Rounded MT Bold", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRegister.ForeColor = SystemColors.ButtonHighlight;
            btnRegister.Location = new Point(311, 346);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(117, 29);
            btnRegister.TabIndex = 4;
            btnRegister.Text = "Register";
            btnRegister.UseVisualStyleBackColor = false;
            btnRegister.Click += btnRegister_Click;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Arial Rounded MT Bold", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblUsername.ForeColor = SystemColors.ButtonHighlight;
            lblUsername.Location = new Point(288, 123);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(93, 20);
            lblUsername.TabIndex = 5;
            lblUsername.Text = "Username";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Arial Rounded MT Bold", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPassword.ForeColor = SystemColors.ButtonHighlight;
            lblPassword.Location = new Point(291, 195);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(90, 20);
            lblPassword.TabIndex = 6;
            lblPassword.Text = "Password";
            // 
            // lblNoAccount
            // 
            lblNoAccount.AutoSize = true;
            lblNoAccount.Font = new Font("Arial Rounded MT Bold", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNoAccount.ForeColor = SystemColors.ButtonFace;
            lblNoAccount.Location = new Point(291, 318);
            lblNoAccount.Name = "lblNoAccount";
            lblNoAccount.Size = new Size(157, 15);
            lblNoAccount.TabIndex = 7;
            lblNoAccount.Text = "Don't have an account?";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.MediumOrchid;
            ClientSize = new Size(800, 450);
            Controls.Add(lblNoAccount);
            Controls.Add(lblPassword);
            Controls.Add(lblUsername);
            Controls.Add(btnRegister);
            Controls.Add(btnLogin);
            Controls.Add(txtLoginPassword);
            Controls.Add(txtLoginUsername);
            Controls.Add(lblHeading);
            ForeColor = SystemColors.ActiveCaptionText;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "LoginForm";
            Text = "Vibely";
            Load += LoginForm_Load;
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
    }
}
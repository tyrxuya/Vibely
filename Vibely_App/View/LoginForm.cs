using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vibely_App.Controls;

namespace Vibely_App.View
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            GradientPanel gradientPanel = new GradientPanel();
            gradientPanel.Dock = DockStyle.Fill;
            gradientPanel.TopColor = Color.Purple;
            gradientPanel.BottomColor = Color.Black;
            gradientPanel.Angle = 45;

            Label lblWelcome = new Label();
            lblWelcome.Text = "Welcome to Vibely";
            lblWelcome.ForeColor = Color.White;
            lblWelcome.Font = new Font("Arial Rounded MT Bold", 24, FontStyle.Bold);
            lblWelcome.Size = new Size(400, 50);
            lblWelcome.Location = new Point((this.ClientSize.Width - lblWelcome.Width) / 2, 30); 
            lblWelcome.BackColor = Color.Transparent;
            this.Controls.Add(lblWelcome);

            TextBox txtLoginUsername = new TextBox();
            txtLoginUsername.Size = new Size(200, 30);
            txtLoginUsername.Font = new Font("Arial Rounded MT Bold", 10, FontStyle.Bold);
            txtLoginUsername.ForeColor = Color.White;
            txtLoginUsername.BackColor = Color.MediumOrchid;
            txtLoginUsername.Location = new Point((this.ClientSize.Width - txtLoginUsername.Width) / 2, lblWelcome.Bottom + 50); 

            Label lblUsername = new Label();
            lblUsername.Text = "Username";
            lblUsername.ForeColor = Color.White;
            lblUsername.Font = new Font("Arial Rounded MT Bold", 10, FontStyle.Regular);
            lblUsername.Size = new Size(100, 20);
            lblUsername.Location = new Point(txtLoginUsername.Left, txtLoginUsername.Top - 25); 
            lblUsername.BackColor = Color.Transparent;
            this.Controls.Add(lblUsername);

       
            TextBox txtLoginPassword = new TextBox();
            txtLoginPassword.PasswordChar = '*';
            txtLoginPassword.Size = new Size(200, 30);
            txtLoginPassword.Font = new Font("Arial Rounded MT Bold", 10, FontStyle.Regular);
            txtLoginPassword.Location = new Point((this.ClientSize.Width - txtLoginPassword.Width) / 2, txtLoginUsername.Bottom + 40);
            txtLoginPassword.ForeColor = Color.White;
            txtLoginPassword.BackColor = Color.MediumOrchid;
            this.Controls.Add(txtLoginPassword);

            Label lblPassword = new Label();
            lblPassword.Text = "Password";
            lblPassword.ForeColor = Color.White;
            lblPassword.Font = new Font("Arial Rounded MT Bold", 10, FontStyle.Regular);
            lblPassword.Size = new Size(100, 20);
            lblPassword.Location = new Point(txtLoginPassword.Left, txtLoginPassword.Top - 25); 
            lblPassword.BackColor = Color.Transparent;
            this.Controls.Add(lblPassword);

            Button btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.Font = new Font("Arial Rounded MT Bold", 10, FontStyle.Regular);
            btnLogin.Size = new Size(180, 35); 
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.BackColor = Color.Purple;
            btnLogin.Location = new Point((this.ClientSize.Width - btnLogin.Width) / 2, txtLoginPassword.Bottom + 30); 
            this.Controls.Add(btnLogin);

            Button btnRegister = new Button();
            btnRegister.Text = "Register";
            btnRegister.Font = new Font("Arial Rounded MT Bold", 10, FontStyle.Regular);
            btnRegister.Size = new Size(180, 35); 
            btnRegister.ForeColor = Color.White;
            btnRegister.BackColor = Color.Purple;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Location = new Point((this.ClientSize.Width - btnRegister.Width) / 2, btnLogin.Bottom + 20); 
            this.Controls.Add(btnRegister);


            PictureBox picUsername = new PictureBox();
            picUsername.Image = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Vibely_App.Resources.login.png"));
            picUsername.SizeMode = PictureBoxSizeMode.StretchImage;
            picUsername.Size = new Size(20, 20);
            picUsername.Location = new Point(txtLoginUsername.Left - 25, txtLoginUsername.Top + (txtLoginUsername.Height - picUsername.Height) / 2); 
            picUsername.BackColor = Color.Transparent;
            picUsername.Parent = gradientPanel; 

            PictureBox picPassword = new PictureBox();
            picPassword.Image = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Vibely_App.Resources.password.png"));
            picPassword.SizeMode = PictureBoxSizeMode.StretchImage;
            picPassword.Size = new Size(20, 20);
            picPassword.Location = new Point(txtLoginPassword.Left - 25, txtLoginPassword.Top + (txtLoginPassword.Height - picPassword.Height) / 2); 
            picPassword.BackColor = Color.Transparent;
            picPassword.Parent = gradientPanel; 

            this.Controls.Add(gradientPanel);
            lblWelcome.Parent = gradientPanel;
            lblUsername.Parent = gradientPanel;
            lblPassword.Parent = gradientPanel;
            txtLoginUsername.Parent = gradientPanel;
            txtLoginPassword.Parent = gradientPanel;
            btnLogin.Parent = gradientPanel;
            btnRegister.Parent = gradientPanel;

        }        
    }
}
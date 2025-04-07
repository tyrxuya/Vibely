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
using Vibely_App.API;
using Vibely_App.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Vibely_App.View
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

            //Background with gradient panel
            GradientPanel gradientPanel = new GradientPanel();
            gradientPanel.Dock = DockStyle.Fill;
            gradientPanel.StartColor = Color.Purple;
            gradientPanel.EndColor = Color.Black;
            gradientPanel.Angle = 45;
            this.Controls.Add(gradientPanel);

            txtLoginPassword.PasswordChar = '*';

            //Setting the icons
            IconHelper.AddIconToTextBox(gradientPanel, txtLoginUsername, APIConstants.IconLogin);
            IconHelper.AddIconToTextBox(gradientPanel, txtLoginPassword, APIConstants.IconPassword);

            //Setting the background of lables
            lblHeading.BackColor = Color.Transparent;
            lblUsername.BackColor = Color.Transparent;
            lblPassword.BackColor = Color.Transparent;
            lblNoAccount.BackColor = Color.Transparent;
            lblHeading.Parent = gradientPanel;
            lblUsername.Parent = gradientPanel;
            lblPassword.Parent = gradientPanel;
            lblNoAccount.Parent = gradientPanel;
            
            //Formatting borders
            txtLoginUsername.BorderStyle = BorderStyle.None;
            txtLoginPassword.BorderStyle = BorderStyle.None;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderColor = Color.Black;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.FlatAppearance.BorderColor = Color.Black;




        }

        private void btnRegister_Click(object sender, EventArgs e)
        {

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
        }

    }
}
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
using Vibely_App.Business;
using Vibely_App.Controls;
using Vibely_App.Data.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Vibely_App.View
{
    public partial class LoginForm : Form
    {
        public IUserBusiness UserBusiness { get; set; }

        public LoginForm()
        {
            InitializeComponent();
            InitializeControls();
            UserBusiness = new UserBusiness(new Data.VibelyDbContext());
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            new RegisterForm().Show();
            this.Hide();
        }

        private void InitializeControls()
        {
            gradientPanel1.Controls.Add(lblHeading);
            gradientPanel1.Controls.Add(txtLoginUsername);
            gradientPanel1.Controls.Add(txtLoginPassword);
            gradientPanel1.Controls.Add(btnLogin);
            gradientPanel1.Controls.Add(btnRegister);
            gradientPanel1.Controls.Add(lblUsername);
            gradientPanel1.Controls.Add(lblPassword);
            gradientPanel1.Controls.Add(lblNoAccount);
            IconHelper.AddIconToControl(gradientPanel1, txtLoginUsername, APIConstants.IconLogin);
            IconHelper.AddIconToControl(gradientPanel1, txtLoginPassword, APIConstants.IconPassword);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLoginUsername.Text) || string.IsNullOrEmpty(txtLoginPassword.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            User user = UserBusiness.FindByCredentials(txtLoginUsername.Text, txtLoginPassword.Text);

            if (user == null)
            {
                MessageBox.Show("Invalid username or password.");
                return;
            }

            new MainApp(user).Show();
            this.Hide();
        }

        private void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }
    }
}
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
            InitializeControls();

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.Show();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
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
            IconHelper.AddIconToTextBox(gradientPanel1, txtLoginUsername, APIConstants.IconLogin);
            IconHelper.AddIconToTextBox(gradientPanel1, txtLoginPassword, APIConstants.IconPassword);
        }

       
    }
}
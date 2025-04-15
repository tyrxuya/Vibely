using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vibely_App.API;
using Vibely_App.Controls;

namespace Vibely_App.View
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            gradientPanel1.Controls.Add(lblRegisterTitle);
            gradientPanel1.Controls.Add(lblRegisterNames);
            gradientPanel1.Controls.Add(lblRegisterUsername);
            gradientPanel1.Controls.Add(lblRegisterPassword);
            gradientPanel1.Controls.Add(lblRegisterPhoneNumber);
            gradientPanel1.Controls.Add(lblRegisterProfilePicture);
            gradientPanel1.Controls.Add(lblRegisterEmail);
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            
        }

        private void panelCenter_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

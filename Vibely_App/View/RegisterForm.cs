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

        private void btnRegisterUpload_Click(object sender, EventArgs e)
        {
            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                string filePath = fileDlg.FileName;
                if (filePath.EndsWith(".jpg") || filePath.EndsWith(".png") || filePath.EndsWith(".jpeg"))
                {
                    // Load the image into the PictureBox
                    pictureBox1.Image = Image.FromFile(filePath);
                }
                else
                {
                    MessageBox.Show("Please select a valid image file (jpg, png, jpeg).");
                }
            }
        }

        private void btnRegisterRegister_Click(object sender, EventArgs e)
        {
            //Register logic
            new LoginForm().Show();
            this.Hide();
        }
    }
}

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
using Vibely_App.Business;
using Vibely_App.Controls;
using Vibely_App.Data.Models;

namespace Vibely_App.View
{
    public partial class RegisterForm : Form
    {
        public IPasswordHasher PasswordHasher { get; set; }
        public IUserBusiness UserBusiness { get; set; }

        public RegisterForm()
        {
            InitializeComponent();
            InitializeControls();
            PasswordHasher = new PasswordHasher();
            UserBusiness = new UserBusiness(new Data.VibelyDbContext());
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
                    pctrUser.Image = Image.FromFile(filePath);
                }
                else
                {
                    MessageBox.Show("Please select a valid image file (jpg, png, jpeg).");
                }
            }
        }

        private void btnRegisterRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRegisterNames.Text) ||
                string.IsNullOrWhiteSpace(txtRegisterUsername.Text) ||
                string.IsNullOrWhiteSpace(txtRegisterPassword.Text) ||
                string.IsNullOrWhiteSpace(txtRegisterPhoneNumber.Text) ||
                string.IsNullOrWhiteSpace(txtRegisterEmail.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                ClearFields();
                return;
            }

            if (!txtRegisterEmail.Text.Contains("@") || !txtRegisterEmail.Text.Contains("."))
            {
                MessageBox.Show("Please enter a valid email address.");
                ClearFields();
                return;
            }

            if (txtRegisterPhoneNumber.Text.Length != 10)
            {
                MessageBox.Show("Please enter a valid phone number (10 digits).");
                ClearFields();
                return;
            }

            string username = txtRegisterUsername.Text;
            string password = PasswordHasher.Hash(txtRegisterPassword.Text);
            string firstName = txtRegisterNames.Text.Split(" ").First();
            string lastName = txtRegisterNames.Text.Split(" ").Last();
            string phoneNumber = txtRegisterPhoneNumber.Text;
            string email = txtRegisterEmail.Text;
            byte[] profilePicture;

            if (pctrUser.Image == null)
            {
                profilePicture = null;
            }

            else
            {
                using (var ms = new MemoryStream())
                {
                    pctrUser.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    profilePicture = ms.ToArray();
                }
            }

            if (UserBusiness.IsUsernameTaken(username))
            {
                MessageBox.Show("Username is already taken. Please choose another one.");
                ClearFields();
                return;
            }

            User user = new()
            {
                Username = username,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Email = email,
                ProfilePicture = profilePicture,
                IsPremium = false,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now),
                SubscriptionPrice = 11.99,
            };

            UserBusiness.Add(user);

            new LoginForm().Show();
            this.Hide();
        }

        private void ClearFields()
        {
            txtRegisterNames.Clear();
            txtRegisterUsername.Clear();
            txtRegisterPassword.Clear();
            txtRegisterPhoneNumber.Clear();
            txtRegisterEmail.Clear();
            pctrUser.Image = null;
        }
    }
}

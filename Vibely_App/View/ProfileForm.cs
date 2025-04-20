using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Vibely_App.Data.Models;

namespace Vibely_App.View
{
    public partial class ProfileForm : Form
    {
        private readonly User user;
        private bool wasCancelled = true;

        public ProfileForm(User user)
        {
            InitializeComponent();
            this.user = user;
            this.Text = "User Profile";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1080, 720); // Adjusted size
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.BackColor = ColorTranslator.FromHtml("#190028");
            
            InitializeUI();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1080, 720); // Adjusted size
            this.Name = "ProfileForm";
            this.ResumeLayout(false);
        }

        private void InitializeUI()
        {
            // Main container panel
            var mainContainer = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ColorTranslator.FromHtml("#190028"),
                Padding = new Padding(10) // Reduced padding
            };

            // Profile picture (centered circle)
            var profilePicture = new Guna2CirclePictureBox
            {
                Size = new Size(160, 160), 
                SizeMode = PictureBoxSizeMode.StretchImage,
                Anchor = AnchorStyles.Top,
                BackColor = Color.Transparent 
            };

            // Display profile image or default initial
            if (user.ProfilePicture != null && user.ProfilePicture.Length > 0)
            {
                using (var ms = new System.IO.MemoryStream(user.ProfilePicture))
                {
                    try { profilePicture.Image = Image.FromStream(ms); }
                    catch { /* Handle potential image loading error */ 
                          profilePicture.BackColor = ColorTranslator.FromHtml("#D9D9D9"); // Fallback color on error
                    }
                }
            }
            else
            {
                 profilePicture.BackColor = ColorTranslator.FromHtml("#D9D9D9"); // Fallback color if no image
                 profilePicture.Paint += (s, e) =>
                {
                    string initial = "??";
                    if (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName))
                    {
                        initial = (user.FirstName.FirstOrDefault().ToString() + user.LastName.FirstOrDefault().ToString()).ToUpper();
                    }
                    else if (!string.IsNullOrEmpty(user.FirstName))
                    {
                         initial = user.FirstName.Substring(0, Math.Min(user.FirstName.Length, 2)).ToUpper();
                    }
                    else if (!string.IsNullOrEmpty(user.Username))
                    {
                         initial = user.Username.Substring(0, Math.Min(user.Username.Length, 2)).ToUpper();
                    }
                    
                    var font = new Font("Arial", 70, FontStyle.Bold); 
                    var textSize = e.Graphics.MeasureString(initial, font);
                    e.Graphics.DrawString(
                        initial,
                        font,
                        new SolidBrush(ColorTranslator.FromHtml("#190028")),
                        (profilePicture.Width - textSize.Width) / 2,
                        (profilePicture.Height - textSize.Height) / 2
                    );
                };
            }

            // User name label (centered)
            var nameLabel = new Guna2HtmlLabel
            {
                Text = $"{user.FirstName} {user.LastName}",
                Font = new Font("Arial", 28, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                TextAlignment = ContentAlignment.MiddleCenter,
                Anchor = AnchorStyles.Top
            };
            
            // Username label (centered)
            var usernameLabel = new Guna2HtmlLabel
            {
                Text = $"@{user.Username ?? "example_user"}",
                Font = new Font("Arial", 18),
                ForeColor = Color.White,
                AutoSize = true,
                TextAlignment = ContentAlignment.MiddleCenter,
                Anchor = AnchorStyles.Top
            };

            // Info panels
            var leftPanel = CreateInfoPanel("Personal information:", 
                new string[] { 
                    $"E-Mail: {user.Email ?? "example_email@gmail.com"}", 
                    $"Phone: {user.PhoneNumber ?? "+359888777666"}" 
                }); 
            
            var rightPanel = CreateInfoPanel("Profile information:", 
                new string[] { 
                    "Total songs: 12", 
                    "Total playlists: 6" 
                }); 

            // Info panels container
            var infoPanelsContainer = new Guna2Panel
            {
                Height = Math.Max(leftPanel.Height, rightPanel.Height), 
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top
            };
            infoPanelsContainer.Controls.Add(leftPanel); 
            infoPanelsContainer.Controls.Add(rightPanel);

            // --- Button Setup --- 
            // FlowLayoutPanel for top 3 buttons
            var topButtonsFlowPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top // Anchor within the table cell
            };
            
            var createPlaylistBtn = CreatePurpleButton("Create playlist", 180);
            var switchAppearanceBtn = CreatePurpleButton("Switch appearance", 220);
            var logOffBtn = CreatePurpleButton("Log off", 130);
            
            topButtonsFlowPanel.Controls.Add(createPlaylistBtn);
            topButtonsFlowPanel.Controls.Add(switchAppearanceBtn);
            topButtonsFlowPanel.Controls.Add(logOffBtn);

            // Go back button
            var goBackBtn = CreatePurpleButton("Go back", 150);
            goBackBtn.Anchor = AnchorStyles.None; // Allow manual centering within table cell
            goBackBtn.Click += (s, e) => 
            {
                wasCancelled = true;
                this.Close(); 
            };

            // TableLayoutPanel to hold both button rows
            var allButtonsPanel = new TableLayoutPanel
            {
                ColumnCount = 1,
                RowCount = 2,
                AutoSize = true,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top
            };
            allButtonsPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            allButtonsPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            allButtonsPanel.Controls.Add(topButtonsFlowPanel, 0, 0);
            allButtonsPanel.Controls.Add(goBackBtn, 0, 1);
            // Location calculated later and in Resize event

            // Add all controls to main container
            mainContainer.Controls.Add(profilePicture);
            mainContainer.Controls.Add(nameLabel);
            mainContainer.Controls.Add(usernameLabel);
            mainContainer.Controls.Add(infoPanelsContainer);
            mainContainer.Controls.Add(allButtonsPanel); // Add the new table layout panel

            this.Controls.Add(mainContainer);
            
            // --- Centering and Positioning Function ---
            Action centerElements = () =>
            {
                mainContainer.PerformLayout(); 
                
                int containerWidth = mainContainer.ClientSize.Width;
                int verticalSpacing = 20; 
                
                profilePicture.Location = new Point((containerWidth - profilePicture.Width) / 2, 30); 
                nameLabel.Location = new Point((containerWidth - nameLabel.Width) / 2, profilePicture.Bottom + verticalSpacing);
                usernameLabel.Location = new Point((containerWidth - usernameLabel.Width) / 2, nameLabel.Bottom + (verticalSpacing / 2));
                
                // Position Info Panels and Container
                int infoPanelSpacing = 40;
                leftPanel.Location = new Point(0, 0);
                rightPanel.Location = new Point(leftPanel.Right + infoPanelSpacing, 0);
                infoPanelsContainer.Width = rightPanel.Right;
                infoPanelsContainer.Location = new Point((containerWidth - infoPanelsContainer.Width) / 2, usernameLabel.Bottom + (verticalSpacing * 2));
                infoPanelsContainer.Height = Math.Max(leftPanel.Height, rightPanel.Height);

                // Center Button Rows within the TableLayoutPanel
                // (The TableLayoutPanel itself will be centered horizontally below)
                if (allButtonsPanel.GetControlFromPosition(0, 0) is Control topRow)
                {
                    // Center the FlowLayoutPanel in the first row
                    var cellWidth = allButtonsPanel.GetColumnWidths()[0];
                    topRow.Location = new Point((cellWidth - topRow.Width) / 2, topRow.Location.Y);
                    topRow.Anchor = AnchorStyles.None; // Override FlowLayoutPanel default anchor if needed
                }
                 if (allButtonsPanel.GetControlFromPosition(0, 1) is Control bottomRowButton)
                {
                    // Center the 'Go Back' button in the second row
                     var cellWidth = allButtonsPanel.GetColumnWidths()[0];
                    bottomRowButton.Location = new Point((cellWidth - bottomRowButton.Width) / 2, bottomRowButton.Location.Y);
                }
                allButtonsPanel.PerformLayout(); // Recalculate layout after positioning controls inside

                // Center the entire TableLayoutPanel
                allButtonsPanel.Location = new Point((containerWidth - allButtonsPanel.Width) / 2, infoPanelsContainer.Bottom + (verticalSpacing + 5));
            };

            // Initial centering when form loads
            this.Load += (s, e) => centerElements();

            // Adjust positions on resize
            mainContainer.Resize += (s, e) => centerElements();

            // Log off button functionality
            logOffBtn.Click += (s, e) =>
            {
                wasCancelled = false;
                this.Close();
            };
        }

        private Guna2Panel CreateInfoPanel(string title, string[] items)
        {
             var panel = new Guna2Panel
            {
                // Width calculated dynamically
                BackColor = ColorTranslator.FromHtml("#393144"),
                BorderRadius = 20,
                Padding = new Padding(20)
            };

            var titleLabel = new Guna2HtmlLabel
            {
                Text = title,
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 20)
            };
            panel.Controls.Add(titleLabel);
            
            int yPos = titleLabel.Bottom + 20;
            int maxWidth = titleLabel.Width; // Start with title width
            
            foreach (var item in items)
            {
                var label = new Guna2HtmlLabel
                {
                    Text = item,
                    Font = new Font("Arial", 16),
                    ForeColor = Color.White,
                    AutoSize = true,
                    Location = new Point(20, yPos)
                };
                panel.Controls.Add(label);
                yPos += label.Height + 20;
                maxWidth = Math.Max(maxWidth, label.Width); // Track max label width
            }
            
            panel.Width = maxWidth + panel.Padding.Left + panel.Padding.Right; // Set width based on max content + padding
            panel.Height = yPos + 10; // Auto-adjust height based on content + padding
            return panel;
        }

        private Guna2Button CreatePurpleButton(string text, int width)
        {
            return new Guna2Button
            {
                Text = text,
                Width = width,
                Height = 45,
                BorderRadius = 22,
                FillColor = ColorTranslator.FromHtml("#D0BFFF"),
                ForeColor = Color.Black,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(8)
            };
        }

        public bool WasCancelled => wasCancelled;
    }
} 
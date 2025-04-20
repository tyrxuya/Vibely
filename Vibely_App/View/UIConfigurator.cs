using System;
using System.Drawing;
using System.Windows.Forms;
using Vibely_App.Data.Models;
using Vibely_App.Controls;
using Guna.UI2.WinForms;

namespace Vibely_App.View
{
    public class UIConfigurator
    {
        private const int SIDEBAR_WIDTH = 280;
        private const int PLAYER_HEIGHT = 100;

        private readonly MainApp mainApp;
        private readonly User activeUser;

        public UIConfigurator(MainApp mainApp, User activeUser)
        {
            this.mainApp = mainApp;
            this.activeUser = activeUser;
        }

        public void InitializeUI()
        {
            mainApp.MinimumSize = new Size(1000, 600);
            mainApp.BackColor = ColorTranslator.FromHtml("#000000");
            mainApp.Padding = new Padding(0);

            ConfigureSidePanel();
            ConfigureMainPanel();
            ConfigurePlayerPanel();
            PopulateWithSampleData();
        }

        private void ConfigureSidePanel()
        {
            mainApp.SidePanel.Controls.Clear(); // Clear all controls first
            
            mainApp.SidePanel.Width = SIDEBAR_WIDTH;
            mainApp.SidePanel.BackColor = ColorTranslator.FromHtml("#190028");
            mainApp.SidePanel.Dock = DockStyle.Left;
            mainApp.SidePanel.Padding = new Padding(0);

            // Create a container panel for better layout control
            var containerPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            // Set row styles
            containerPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F)); // Profile panel height + spacing
            containerPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // Playlists panel takes remaining space

            var profilePanel = ConfigureProfilePanel();
            var playlistsPanel = ConfigurePlaylistPanel();

            // Add panels to container
            containerPanel.Controls.Add(profilePanel, 0, 0);
            containerPanel.Controls.Add(playlistsPanel, 0, 1);

            // Add container to side panel
            mainApp.SidePanel.Controls.Add(containerPanel);
        }

        private Panel ConfigureProfilePanel()
        {
            var clickableProfilePanel = new Guna2Panel
            {
                Size = new Size(SIDEBAR_WIDTH, 100),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Dock = DockStyle.Top
            };

            // Configure profile picture
            mainApp.PctrUser.Size = new Size(60, 60);
            mainApp.PctrUser.SizeMode = PictureBoxSizeMode.StretchImage;
            mainApp.PctrUser.BackColor = Color.Transparent;
            mainApp.PctrUser.Location = new Point(20, 20);
            mainApp.PctrUser.Cursor = Cursors.Hand;

            // Set profile picture if available
            if (activeUser.ProfilePicture != null)
            {
                using (var ms = new System.IO.MemoryStream(activeUser.ProfilePicture))
                {
                    mainApp.PctrUser.Image = Image.FromStream(ms);
                }
            }
            else
            {
                // Set a default profile picture or initial
                mainApp.PctrUser.BackColor = ColorTranslator.FromHtml("#46325D");
                mainApp.PctrUser.Image = null;
                mainApp.PctrUser.Paint += (s, e) =>
                {
                    var initial = (activeUser.FirstName.FirstOrDefault().ToString() + activeUser.LastName.FirstOrDefault().ToString()).ToUpper();
                    var font = new Font("Arial Rounded MT Bold", 20);
                    var textSize = e.Graphics.MeasureString(initial, font);
                    e.Graphics.DrawString(
                        initial,
                        font,
                        new SolidBrush(ColorTranslator.FromHtml("#C7ADFF")),
                        (mainApp.PctrUser.Width - textSize.Width) / 2,
                        (mainApp.PctrUser.Height - textSize.Height) / 2
                    );
                };
            }

            // Configure username label
            mainApp.LblUserName.Text = $"{activeUser.FirstName} {activeUser.LastName}";
            mainApp.LblUserName.Font = new Font("Arial Rounded MT Bold", 12);
            mainApp.LblUserName.ForeColor = ColorTranslator.FromHtml("#C7ADFF");
            mainApp.LblUserName.AutoSize = true;
            mainApp.LblUserName.Location = new Point(mainApp.PctrUser.Right + 10, mainApp.PctrUser.Top + (mainApp.PctrUser.Height - 20) / 2);
            mainApp.LblUserName.Cursor = Cursors.Hand;

            // Add hover effect
            clickableProfilePanel.MouseEnter += (s, e) =>
            {
                clickableProfilePanel.BackColor = ColorTranslator.FromHtml("#46325D");
            };

            clickableProfilePanel.MouseLeave += (s, e) =>
            {
                clickableProfilePanel.BackColor = Color.Transparent;
            };

            // Remove any existing click handlers to prevent duplicates
            clickableProfilePanel.Click -= profileClick;
            mainApp.PctrUser.Click -= profileClick;
            mainApp.LblUserName.Click -= profileClick;

            // Add click handlers
            clickableProfilePanel.Click += profileClick;

            clickableProfilePanel.Controls.AddRange(new Control[] { mainApp.PctrUser, mainApp.LblUserName });
            return clickableProfilePanel;
        }

        private void profileClick(object sender, EventArgs e)
        {
            // Hide the main form before showing profile
            Form mainForm = mainApp as Form;
            mainForm.Hide();
            
            // Show profile form
            var profileForm = new ProfileForm(activeUser);
            profileForm.ShowDialog();
            
            // Check if user logged off
            if (!profileForm.WasCancelled)
            {
                // User logged off, close the main app
                new LoginForm().Show();
                mainForm.Hide();
            }
            else
            {
                // User just closed the profile, show main form again
                mainForm.Show();
            }
        }

        private FlowLayoutPanel ConfigurePlaylistPanel()
        {
            var playlistsFlowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Padding = new Padding(20, 40, 20, 20)
            };

            var playlistsLabel = new Guna2HtmlLabel
            {
                Text = "PLAYLISTS",
                Font = new Font("Arial Rounded MT Bold", 12),
                ForeColor = ColorTranslator.FromHtml("#C7ADFF"),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            };

            playlistsFlowPanel.Controls.Add(playlistsLabel);

            // Fixed playlist array without duplicates
            // To be used later as reference
            string[] playlists = { 
                "My Favorites", 
                "Rock Collection", 
                "Chill Vibes", 
                "Party Mix", 
                "Study Music" 
            };

            foreach (var playlist in playlists)
            {
                var btn = new Guna2Button
                {
                    Text = playlist,
                    Width = SIDEBAR_WIDTH - 40,
                    Height = 40,
                    FillColor = ColorTranslator.FromHtml("#46325D"),
                    ForeColor = ColorTranslator.FromHtml("#C7ADFF"),
                    BorderRadius = 10,
                    Font = new Font("Arial Rounded MT Bold", 11),
                    TextAlign = HorizontalAlignment.Left,
                    Padding = new Padding(15, 0, 0, 0),
                    Margin = new Padding(0, 0, 0, 10),
                    Cursor = Cursors.Hand
                };
                playlistsFlowPanel.Controls.Add(btn);
            }

            return playlistsFlowPanel;
        }

        private void ConfigureMainPanel()
        {
            mainApp.MainPanel.BackColor = ColorTranslator.FromHtml("#1B1B1B");
            mainApp.MainPanel.Dock = DockStyle.Fill;
            mainApp.MainPanel.Padding = new Padding(20);

            var mainTableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var searchPanel = ConfigureSearchPanel();
            var songsFlowPanel = ConfigureSongsPanel();

            mainTableLayout.Controls.Add(searchPanel, 0, 0);
            mainTableLayout.Controls.Add(songsFlowPanel, 0, 1);

            mainApp.MainPanel.Controls.Clear();
            mainApp.MainPanel.Controls.Add(mainTableLayout);
        }

        private Panel ConfigureSearchPanel()
        {
            var searchPanel = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                Height = 80,
                BackColor = Color.Transparent,
                Padding = new Padding(0)
            };

            const int SEARCH_BAR_WIDTH = 400;
            const int BUTTON_WIDTH = 100;
            const int SPACING = 10;
            const int TOTAL_WIDTH = BUTTON_WIDTH + SPACING + SEARCH_BAR_WIDTH + SPACING + BUTTON_WIDTH;

            mainApp.TxtSearch.Size = new Size(SEARCH_BAR_WIDTH, 40);
            mainApp.TxtSearch.PlaceholderText = "Search for songs...";
            mainApp.TxtSearch.Font = new Font("Arial Rounded MT Bold", 12);
            mainApp.TxtSearch.FillColor = ColorTranslator.FromHtml("#2D1F3D");
            mainApp.TxtSearch.ForeColor = ColorTranslator.FromHtml("#C7ADFF");
            mainApp.TxtSearch.BorderRadius = 20;
            mainApp.TxtSearch.BorderThickness = 0;
            mainApp.TxtSearch.Cursor = Cursors.IBeam;

            mainApp.BtnUpload.Size = new Size(BUTTON_WIDTH, 40);
            mainApp.BtnUpload.FillColor = ColorTranslator.FromHtml("#DAC7FF");
            mainApp.BtnUpload.ForeColor = Color.Black;
            mainApp.BtnUpload.Text = "Upload";
            mainApp.BtnUpload.Font = new Font("Arial Rounded MT Bold", 10);
            mainApp.BtnUpload.BorderRadius = 20;
            mainApp.BtnUpload.Cursor = Cursors.Hand;

            mainApp.BtnSearch.Size = new Size(BUTTON_WIDTH, 40);
            mainApp.BtnSearch.FillColor = ColorTranslator.FromHtml("#DAC7FF");
            mainApp.BtnSearch.ForeColor = Color.Black;
            mainApp.BtnSearch.Text = "Search";
            mainApp.BtnSearch.Font = new Font("Arial Rounded MT Bold", 10);
            mainApp.BtnSearch.BorderRadius = 20;
            mainApp.BtnSearch.Cursor = Cursors.Hand;

            searchPanel.Controls.AddRange(new Control[] { mainApp.BtnUpload, mainApp.TxtSearch, mainApp.BtnSearch });

            void CenterSearchControls()
            {
                int availableWidth = searchPanel.Width;
                int startX = (availableWidth - TOTAL_WIDTH) / 2;
                
                mainApp.BtnUpload.Location = new Point(startX, 20);
                mainApp.TxtSearch.Location = new Point(startX + BUTTON_WIDTH + SPACING, 20);
                mainApp.BtnSearch.Location = new Point(mainApp.TxtSearch.Right + SPACING, 20);
            }

            searchPanel.Layout += (s, e) => CenterSearchControls();
            mainApp.Resize += (s, e) => CenterSearchControls();

            return searchPanel;
        }

        private FlowLayoutPanel ConfigureSongsPanel()
        {
            return new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Padding = new Padding(0)
            };
        }

        private void ConfigurePlayerPanel()
        {
            mainApp.PlayerPanel.Controls.Clear();
            mainApp.PlayerPanel.Height = PLAYER_HEIGHT;
            mainApp.PlayerPanel.BackColor = ColorTranslator.FromHtml("#3F3649");
            mainApp.PlayerPanel.Dock = DockStyle.Bottom;
            mainApp.PlayerPanel.Padding = new Padding(20);

            // Create playback panel
            var playbackPanel = new Panel
            {
                Width = 150,
                Height = 40,
                BackColor = ColorTranslator.FromHtml("#3F3649")
            };

            // Configure player controls
            mainApp.BtnPrev.Size = new Size(40, 40);
            mainApp.BtnPrev.Location = new Point(0, 0);
            mainApp.BtnPrev.FillColor = ColorTranslator.FromHtml("#3F3649");
            mainApp.BtnPrev.ForeColor = ColorTranslator.FromHtml("#C7ADFF");
            mainApp.BtnPrev.Text = "⏮";
            mainApp.BtnPrev.Font = new Font("Arial Rounded MT Bold", 12);
            mainApp.BtnPrev.Cursor = Cursors.Hand;

            mainApp.BtnPlay.Size = new Size(40, 40);
            mainApp.BtnPlay.Location = new Point(55, 0);
            mainApp.BtnPlay.FillColor = ColorTranslator.FromHtml("#3F3649");
            mainApp.BtnPlay.ForeColor = ColorTranslator.FromHtml("#C7ADFF");
            mainApp.BtnPlay.Text = "▶";
            mainApp.BtnPlay.Font = new Font("Arial Rounded MT Bold", 12);
            mainApp.BtnPlay.TextAlign = HorizontalAlignment.Center;
            mainApp.BtnPlay.Cursor = Cursors.Hand;

            mainApp.BtnNext.Size = new Size(40, 40);
            mainApp.BtnNext.Location = new Point(110, 0);
            mainApp.BtnNext.FillColor = ColorTranslator.FromHtml("#3F3649");
            mainApp.BtnNext.ForeColor = ColorTranslator.FromHtml("#C7ADFF");
            mainApp.BtnNext.Text = "⏭";
            mainApp.BtnNext.Font = new Font("Arial Rounded MT Bold", 12);
            mainApp.BtnNext.Cursor = Cursors.Hand;

            playbackPanel.Controls.AddRange(new Control[] { mainApp.BtnPrev, mainApp.BtnPlay, mainApp.BtnNext });
            playbackPanel.Location = new Point((mainApp.PlayerPanel.Width - playbackPanel.Width) / 2, 10);

            // Configure volume controls
            var volumePanel = new Panel
            {
                Height = 40,
                Width = 140,
                BackColor = ColorTranslator.FromHtml("#3F3649")
            };

            mainApp.BtnVolume.Size = new Size(30, 30);
            mainApp.BtnVolume.Location = new Point(0, 3);
            mainApp.BtnVolume.FillColor = ColorTranslator.FromHtml("#3F3649");
            mainApp.BtnVolume.ForeColor = ColorTranslator.FromHtml("#C7ADFF");
            mainApp.BtnVolume.Text = "🔊";
            mainApp.BtnVolume.Font = new Font("Arial Rounded MT Bold", 12);
            mainApp.BtnVolume.TextAlign = HorizontalAlignment.Center;
            mainApp.BtnVolume.BorderRadius = 5;
            mainApp.BtnVolume.Cursor = Cursors.Hand;

            mainApp.TrackBarVolume.Size = new Size(100, 23);
            mainApp.TrackBarVolume.Location = new Point(35, 5);
            mainApp.TrackBarVolume.ThumbColor = ColorTranslator.FromHtml("#C7ADFF");
            mainApp.TrackBarVolume.Value = 50;

            volumePanel.Controls.AddRange(new Control[] { mainApp.BtnVolume, mainApp.TrackBarVolume });
            volumePanel.Location = new Point(mainApp.PlayerPanel.Width - volumePanel.Width - 20, 35);

            // Configure progress bar and labels
            mainApp.TrackBarProgress.Size = new Size(mainApp.PlayerPanel.Width - 600, 23);
            mainApp.TrackBarProgress.Location = new Point(300, 60);
            mainApp.TrackBarProgress.ThumbColor = ColorTranslator.FromHtml("#C7ADFF");

            mainApp.LblCurrentTime.Text = "0:00";
            mainApp.LblCurrentTime.Location = new Point(260, 60);
            mainApp.LblCurrentTime.ForeColor = ColorTranslator.FromHtml("#C7ADFF");
            mainApp.LblCurrentTime.Font = new Font("Arial Rounded MT Bold", 10);

            mainApp.LblTotalTime.Text = "0:00";
            mainApp.LblTotalTime.Location = new Point(mainApp.PlayerPanel.Width - 280, 60);
            mainApp.LblTotalTime.ForeColor = ColorTranslator.FromHtml("#C7ADFF");
            mainApp.LblTotalTime.Font = new Font("Arial Rounded MT Bold", 10);

            mainApp.LblCurrentSong.Text = "No song playing";
            mainApp.LblCurrentSong.Location = new Point(300, 20);
            mainApp.LblCurrentSong.ForeColor = ColorTranslator.FromHtml("#C7ADFF");
            mainApp.LblCurrentSong.Font = new Font("Arial Rounded MT Bold", 12);

            mainApp.LblCurrentArtist.Text = "Unknown artist";
            mainApp.LblCurrentArtist.Location = new Point(300, 40);
            mainApp.LblCurrentArtist.ForeColor = ColorTranslator.FromHtml("#C7ADFF");
            mainApp.LblCurrentArtist.Font = new Font("Arial Rounded MT Bold", 10);

            mainApp.PlayerPanel.Controls.AddRange(new Control[] {
                playbackPanel,
                volumePanel,
                mainApp.TrackBarProgress,
                mainApp.LblCurrentTime,
                mainApp.LblTotalTime,
                mainApp.LblCurrentSong,
                mainApp.LblCurrentArtist
            });

            // Add resize handler
            mainApp.Resize += (s, e) =>
            {
                playbackPanel.Location = new Point((mainApp.PlayerPanel.Width - playbackPanel.Width) / 2, 10);
                
                int progressBarWidth = mainApp.PlayerPanel.Width - 600;
                mainApp.TrackBarProgress.Width = progressBarWidth;
                mainApp.TrackBarProgress.Size = new Size(progressBarWidth, 23);
                mainApp.TrackBarProgress.Location = new Point(300, 60);
                
                mainApp.LblCurrentSong.Location = new Point(300, 20);
                mainApp.LblCurrentArtist.Location = new Point(300, 40);
                mainApp.LblCurrentTime.Location = new Point(260, 60);
                mainApp.LblTotalTime.Location = new Point(mainApp.PlayerPanel.Width - 280, 60);
                
                volumePanel.Location = new Point(mainApp.PlayerPanel.Width - volumePanel.Width - 20, 35);
            };

            // Set up event handlers
            SetupPlayerEventHandlers();
        }

        private void SetupPlayerEventHandlers()
        {
            // Remove any existing handlers first to prevent duplicates
            mainApp.BtnPlay.Click -= PlayButton_Click;
            mainApp.BtnPrev.Click -= PrevButton_Click;
            mainApp.BtnNext.Click -= NextButton_Click;
            mainApp.BtnVolume.Click -= VolumeButton_Click;
            mainApp.TrackBarVolume.ValueChanged -= VolumeSlider_ValueChanged;
            mainApp.TrackBarProgress.ValueChanged -= ProgressBar_ValueChanged;

            // Add the handlers
            mainApp.BtnPlay.Click += PlayButton_Click;
            mainApp.BtnPrev.Click += PrevButton_Click;
            mainApp.BtnNext.Click += NextButton_Click;
            mainApp.BtnVolume.Click += VolumeButton_Click;
            mainApp.TrackBarVolume.ValueChanged += VolumeSlider_ValueChanged;
            mainApp.TrackBarProgress.ValueChanged += ProgressBar_ValueChanged;
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            mainApp.BtnPlay.Text = mainApp.BtnPlay.Text == "▶" ? "⏸" : "▶";
            // TODO: Add actual play/pause functionality
        }

        private void PrevButton_Click(object sender, EventArgs e)
        {
            // TODO: Add previous track functionality
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            // TODO: Add next track functionality
        }

        private void VolumeButton_Click(object sender, EventArgs e)
        {
            if (mainApp.TrackBarVolume.Value > 0)
            {
                mainApp.BtnVolume.Tag = mainApp.TrackBarVolume.Value; // Store current volume
                mainApp.TrackBarVolume.Value = 0;
                mainApp.BtnVolume.Text = "🔈";
            }
            else
            {
                mainApp.TrackBarVolume.Value = mainApp.BtnVolume.Tag != null ? (int)mainApp.BtnVolume.Tag : 50;
                mainApp.BtnVolume.Text = "🔊";
            }
        }

        private void VolumeSlider_ValueChanged(object sender, EventArgs e)
        {
            mainApp.BtnVolume.Text = mainApp.TrackBarVolume.Value == 0 ? "🔈" : "🔊";
            // TODO: Add actual volume control functionality
        }

        private void ProgressBar_ValueChanged(object sender, EventArgs e)
        {
            // TODO: Add seek functionality
        }

        //To be used as reference later
        private void PopulateWithSampleData()
        {
            var mainTableLayout = mainApp.MainPanel.Controls[0] as TableLayoutPanel;
            var songsFlowPanel = mainTableLayout?.GetControlFromPosition(0, 1) as FlowLayoutPanel;

            if (songsFlowPanel != null)
            {
                songsFlowPanel.Controls.Clear();

                var sampleSongs = new[]
                {
                    new Song { Title = "Bohemian Rhapsody", Duration = 354, User = new User { FirstName = "Queen", LastName = "" }, Genre = new Genre { Name = "Rock" } },
                    new Song { Title = "Hotel California", Duration = 391, User = new User { FirstName = "Eagles", LastName = "" }, Genre = new Genre { Name = "Rock" } },
                    new Song { Title = "Sweet Dreams", Duration = 216, User = new User { FirstName = "Eurythmics", LastName = "" }, Genre = new Genre { Name = "Pop" } },
                    new Song { Title = "Stairway to Heaven", Duration = 482, User = new User { FirstName = "Led Zeppelin", LastName = "" }, Genre = new Genre { Name = "Rock" } },
                    new Song { Title = "Billie Jean", Duration = 294, User = new User { FirstName = "Michael", LastName = "Jackson" }, Genre = new Genre { Name = "Pop" } },
                    new Song { Title = "Like a Rolling Stone", Duration = 373, User = new User { FirstName = "Bob", LastName = "Dylan" }, Genre = new Genre { Name = "Rock" } },
                    new Song { Title = "Imagine", Duration = 183, User = new User { FirstName = "John", LastName = "Lennon" }, Genre = new Genre { Name = "Rock" } },
                    new Song { Title = "Purple Rain", Duration = 520, User = new User { FirstName = "Prince", LastName = "" }, Genre = new Genre { Name = "Rock" } },
                    new Song { Title = "Respect", Duration = 148, User = new User { FirstName = "Aretha", LastName = "Franklin" }, Genre = new Genre { Name = "Soul" } },
                    new Song { Title = "Smells Like Teen Spirit", Duration = 301, User = new User { FirstName = "Nirvana", LastName = "" }, Genre = new Genre { Name = "Rock" } }
                };

                foreach (var song in sampleSongs)
                {
                    var songControl = new SongControl(song);
                    songControl.Width = songsFlowPanel.Width - 40;
                    songControl.Margin = new Padding(0, 0, 0, 10);
                    songControl.Cursor = Cursors.Hand;
                    songsFlowPanel.Controls.Add(songControl);
                }
            }
        }
    }
}

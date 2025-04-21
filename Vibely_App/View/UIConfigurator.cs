using System;
using System.Drawing;
using System.Windows.Forms;
using Vibely_App.Data.Models;
using Vibely_App.Controls;
using Guna.UI2.WinForms;
using System.Diagnostics;
using Vibely_App.Business;
using Vibely_App.Data;
using System.Linq; // Added for FirstOrDefault
using System.IO; // For MemoryStream
using NAudio.Wave; // For NAudio playback

namespace Vibely_App.View
{
    public class UIConfigurator : IDisposable
    {
        private const int SIDEBAR_WIDTH = 280;
        private const int PLAYER_HEIGHT = 100;

        private readonly MainApp mainApp;
        private readonly User activeUser;
        private Playlist activePlaylist;
        private FlowLayoutPanel songFlowPanel;
        private SongBusiness songBusiness;

        // Playback related fields
        private System.Windows.Forms.Timer playbackTimer;
        private Song currentPlayingSong;
        private bool isPlaying = false;
        private double currentPositionSeconds = 0;
        private double totalDurationSeconds = 0;
        private bool isDraggingSlider = false; // Flag to prevent timer updates during manual seek

        // NAudio Playback fields
        private WaveOutEvent waveOut;
        private WaveStream audioReader; // Use WaveStream as a base class for different formats
        private MemoryStream audioStream;
        private bool disposedValue; // For IDisposable
        private bool isStoppingForNewSong = false; // Flag to prevent UI reset during song switch
        private readonly object playbackLock = new object(); // Lock for synchronization
        private List<Song> currentSongList; // List of currently loaded songs
        private bool isSkippingTrack = false; // Flag for Next/Prev button clicks
        private System.Windows.Forms.Timer searchDebounceTimer; // Timer for dynamic search
        private const int SEARCH_DEBOUNCE_INTERVAL_MS = 300; // Delay in ms after typing stops

        public static bool IsDarkMode { get; set; } = true;
        public static event EventHandler ThemeToggled;

        public static void ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
            Debug.WriteLine($"Theme toggled. IsDarkMode: {IsDarkMode}");
            ThemeToggled?.Invoke(null, EventArgs.Empty); // Notify listeners
        }

        public UIConfigurator(MainApp mainApp, User activeUser)
        {
            this.mainApp = mainApp;
            this.activeUser = activeUser;
            songBusiness = new SongBusiness(new VibelyDbContext());
            InitializePlaybackTimer(); // Initialize the timer
            InitializeAudioPlaybackEngine(); // Initialize NAudio engine
            InitializeSearchDebounceTimer(); // Initialize the search timer
        }

        public void InitializeUI()
        {
            mainApp.MinimumSize = new Size(1000, 600);
            mainApp.BackColor = ColorTranslator.FromHtml("#000000");
            mainApp.Padding = new Padding(0);

            ConfigureSidePanel();
            ConfigureMainPanel();
            ConfigurePlayerPanel();
            SetupMainContent();
        }

        private void ConfigureSidePanel()
        {
            mainApp.SidePanel.Controls.Clear(); // Clear all controls first
            
            mainApp.SidePanel.Width = SIDEBAR_WIDTH;
            mainApp.SidePanel.BackColor = ColorTranslator.FromHtml(IsDarkMode ? "#190028" : "#DAC7FF");
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
                mainApp.PctrUser.Image = null;
                mainApp.PctrUser.Paint += (s, e) =>
                {
                    var initial = (activeUser.FirstName.FirstOrDefault().ToString() + activeUser.LastName.FirstOrDefault().ToString()).ToUpper();
                    var font = new Font("Arial Rounded MT Bold", 20);
                    var textSize = e.Graphics.MeasureString(initial, font);
                    e.Graphics.DrawString(
                        initial,
                        font,
                        new SolidBrush(ColorTranslator.FromHtml(IsDarkMode ? "#C7ADFF" : "#3F3649")),
                        (mainApp.PctrUser.Width - textSize.Width) / 2,
                        (mainApp.PctrUser.Height - textSize.Height) / 2
                    );
                };
            }

            // Configure username label
            mainApp.LblUserName.Text = $"{activeUser.FirstName} {activeUser.LastName}";
            mainApp.LblUserName.Font = new Font("Arial Rounded MT Bold", 12);
            mainApp.LblUserName.ForeColor = ColorTranslator.FromHtml(IsDarkMode ? "#C7ADFF" : "#3F3649");
            mainApp.LblUserName.AutoSize = true;
            mainApp.LblUserName.Location = new Point(mainApp.PctrUser.Right + 10, mainApp.PctrUser.Top + (mainApp.PctrUser.Height - 20) / 2);
            mainApp.LblUserName.Cursor = Cursors.Hand;

            // Add hover effect
            clickableProfilePanel.MouseEnter += (s, e) =>
            {
                clickableProfilePanel.BackColor = ColorTranslator.FromHtml(IsDarkMode ? "#46325D" : "#AC8BEE");
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
                ForeColor = ColorTranslator.FromHtml(IsDarkMode ? "#C7ADFF" : "#3F3649"),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            };

            playlistsFlowPanel.Controls.Add(playlistsLabel);

            // Fixed playlist array without duplicates
            // To be used later as reference
            List<string> playlists;

            var userPlaylistsBusiness = new UserPlaylistBusiness(new VibelyDbContext());
            var allPlaylists = userPlaylistsBusiness.GetAll();
            playlists = allPlaylists.Where(p => p.UserId == activeUser.Id)
                .Select(p => p.Playlist.Title)
                .ToList();
            List<string> finalPlaylists = new();
            finalPlaylists.Add("All songs");
            foreach (var playlist in playlists)
            {
                if (!finalPlaylists.Contains(playlist))
                {
                    finalPlaylists.Add(playlist);
                }
            }

            foreach (var playlist in finalPlaylists)
            {
                var btn = new Guna2Button
                {
                    Text = playlist,
                    Width = SIDEBAR_WIDTH - 40,
                    Height = 40,
                    FillColor = ColorTranslator.FromHtml(IsDarkMode ? "#46325D" : "#E0AAFF"),
                    ForeColor = ColorTranslator.FromHtml(IsDarkMode ? "#C7ADFF" : "#3F3649"),
                    BorderRadius = 10,
                    Font = new Font("Arial Rounded MT Bold", 11),
                    TextAlign = HorizontalAlignment.Left,
                    Padding = new Padding(15, 0, 0, 0),
                    Margin = new Padding(0, 0, 0, 10),
                    Cursor = Cursors.Hand
                };
                btn.Click += (s, e) => changeActivePlaylist(btn);
                playlistsFlowPanel.Controls.Add(btn);
            }

            return playlistsFlowPanel;
        }

        private void changeActivePlaylist(Guna2Button btn)
        {
            string playlistName = btn.Text;

            if (playlistName == "All songs")
            {
                activePlaylist = null;
                UpdateSongs(null);
                return;
            }

            var playlistBusiness = new PlaylistBusiness(new VibelyDbContext());
            var playlist = playlistBusiness.FindByName(playlistName);
            if (playlist != null)
            {
                activePlaylist = playlist;
                UpdateSongs(playlist);
            }
        }

        private void ConfigureMainPanel()
        {
            mainApp.MainPanel.BackColor = ColorTranslator.FromHtml(IsDarkMode ? "#1B1B1B" : "#E4E4E4");
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
            SetupMainContent();

            mainTableLayout.Controls.Add(searchPanel, 0, 0);
            mainTableLayout.Controls.Add(songFlowPanel, 0, 1);

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
            mainApp.TxtSearch.ForeColor = ColorTranslator.FromHtml(IsDarkMode ? "#C7ADFF" : "#3F3649");
            mainApp.TxtSearch.PlaceholderForeColor = ColorTranslator.FromHtml(IsDarkMode ? "#C7ADFF" : "#3F3649");
            mainApp.TxtSearch.PlaceholderText = "Search for songs...";
            mainApp.TxtSearch.Font = new Font("Arial Rounded MT Bold", 12);
            mainApp.TxtSearch.FillColor = ColorTranslator.FromHtml(IsDarkMode ? "#2D1F3D" : "#D9D9D9");
            mainApp.TxtSearch.BorderRadius = 20;
            mainApp.TxtSearch.BorderThickness = 0;
            mainApp.TxtSearch.Cursor = Cursors.IBeam;

            mainApp.BtnUpload.Size = new Size(BUTTON_WIDTH, 40);
            mainApp.BtnUpload.FillColor = ColorTranslator.FromHtml(IsDarkMode ? "#DAC7FF" : "#46325D");
            mainApp.BtnUpload.ForeColor = IsDarkMode ? Color.Black : Color.White;
            mainApp.BtnUpload.Text = "Upload";
            mainApp.BtnUpload.Font = new Font("Arial Rounded MT Bold", 10);
            mainApp.BtnUpload.BorderRadius = 20;
            mainApp.BtnUpload.Cursor = Cursors.Hand;

            mainApp.BtnSearch.Size = new Size(BUTTON_WIDTH, 40);
            mainApp.BtnSearch.FillColor = ColorTranslator.FromHtml(IsDarkMode ? "#DAC7FF" : "#46325D");
            mainApp.BtnSearch.ForeColor = IsDarkMode ? Color.Black : Color.White;
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

            // --- Update Search Event Handlers ---
            // Keep button click as an option
            mainApp.BtnSearch.Click -= SearchButton_Click;
            mainApp.BtnSearch.Click += SearchButton_Click;

            // Add TextChanged handler for dynamic search
            mainApp.TxtSearch.TextChanged -= TxtSearch_TextChanged; // Remove previous if any
            mainApp.TxtSearch.TextChanged += TxtSearch_TextChanged;
            // ---------------------------------

            return searchPanel;
        }

        // --- Dynamic Search TextChanged Handler ---
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            // Restart the debounce timer every time the text changes
            searchDebounceTimer.Stop();
            searchDebounceTimer.Start();
        }
        // ----------------------------------------

        // --- Timer Tick Handler for Debounced Search ---
        private void SearchDebounceTimer_Tick(object sender, EventArgs e)
        {
            // Timer has ticked, meaning user paused typing
            searchDebounceTimer.Stop(); // Stop the timer
            PerformSearch(); // Perform the actual search
        }
        // ----------------------------------------------

        // Keep SearchButton_Click to allow explicit search
        private void SearchButton_Click(object sender, EventArgs e)
        {
            searchDebounceTimer.Stop(); // Stop timer if running
            PerformSearch();
        }

        // --- Search Logic ---
        private void PerformSearch()
        {
            string searchText = mainApp.TxtSearch.Text.Trim();
            Debug.WriteLine($"Performing search for: '{searchText}'");

            List<Song> baseSongList;
            lock (playbackLock) // Access activePlaylist safely
            {
                 // Get the base list depending on the currently selected playlist
                 if (activePlaylist == null) // "All songs" selected
                 {
                     baseSongList = songBusiness.GetAll();
                 }
                 else
                 {
                     var playlistSongs = new PlaylistSongBusiness(new VibelyDbContext());
                     baseSongList = playlistSongs.GetAllSongsInPlaylist(activePlaylist);
                 }
            }

             List<Song> filteredSongs;
             if (string.IsNullOrWhiteSpace(searchText))
             {
                 filteredSongs = baseSongList; // No search text, show all from current view
             }
             else
             {
                 filteredSongs = baseSongList
                     .Where(s => (s.Title != null && s.Title.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                                 (s.Artist != null && s.Artist.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0))
                     .ToList();
             }

             Debug.WriteLine($"Search found {filteredSongs.Count} songs.");
             DisplaySongs(filteredSongs); // Update the UI with filtered results
        }
        // ------------------

        private void SetupMainContent()
        {
            songFlowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            mainApp.MainPanel.Controls.Add(songFlowPanel);

            UpdateSongs(null);
        }

        private void SongControl_OnDelete(object sender, Song songToDelete)
        {
            if (sender is SongControl controlToDelete)
            {
                var confirmResult = MessageBox.Show($"Are you sure you want to delete '{songToDelete.Title}'?",
                                                     "Confirm Deletion",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        if (activePlaylist == null)
                    {
                        songBusiness.Remove(songToDelete.Id);

                        songFlowPanel?.Controls.Remove(controlToDelete);

                        MessageBox.Show($"Song '{songToDelete.Title}' deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            var playlistSongBusiness = new PlaylistSongBusiness(new VibelyDbContext());
                            var playlistSong = playlistSongBusiness.FindByPlaylistAndSong(activePlaylist, songToDelete);
                            if (playlistSong != null)
                            {
                                playlistSongBusiness.Remove(playlistSong.Id);
                                songFlowPanel?.Controls.Remove(controlToDelete);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting song: {ex.Message}", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        controlToDelete.Dispose();
                    }
                }
            }
        }

        private void SongControl_OnAddToPlaylist(object sender, Song songToAdd)
        {
            // TODO: Replace this with actual playlist fetching logic from your Business layer
            var userPlaylistBusiness = new UserPlaylistBusiness(new VibelyDbContext());
            var playlistBusiness = new PlaylistBusiness(new VibelyDbContext());
            var playlistSongBusiness = new PlaylistSongBusiness(new VibelyDbContext());
            var playlists = userPlaylistBusiness.GetAll()
                .Where(p => p.UserId == activeUser.Id)
                .Select(p => p.Playlist.Title)
                .ToList();

            ContextMenuStrip playlistMenu = new ContextMenuStrip();

            foreach (var playlistName in playlists)
            {
                ToolStripMenuItem playlistItem = new ToolStripMenuItem(playlistName);
                playlistItem.Tag = playlistName; // Store playlist name for the click event
                playlistItem.Click += (itemSender, itemArgs) =>
                {
                    string selectedPlaylist = ((ToolStripMenuItem)itemSender).Tag.ToString();

                    // TODO: Add the actual logic to add songToAdd to the selectedPlaylist
                    MessageBox.Show($"Adding '{songToAdd.Title}' to playlist '{selectedPlaylist}'...", "Add to Playlist", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Example: Call your business logic here
                    // var playlistBusiness = new PlaylistBusiness(new VibelyDbContext());
                    // var playlist = playlistBusiness.FindByName(selectedPlaylist);
                    // if (playlist != null) {
                    //     var songPlaylistBusiness = new SongPlaylistBusiness(new VibelyDbContext());
                    //     songPlaylistBusiness.AddSongToPlaylist(songToAdd.Id, playlist.Id);
                    // }

                    var playlistToAdd = playlistBusiness.FindByName(selectedPlaylist);

                    if (playlistSongBusiness.FindByPlaylistAndSong(playlistToAdd, songToAdd) != null)
                    {
                        MessageBox.Show($"Song '{songToAdd.Title}' is already in playlist '{selectedPlaylist}'.", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    PlaylistSong ps = new()
                    {
                        PlaylistId = playlistToAdd.Id,
                        SongId = songToAdd.Id
                    };
                    
                    playlistSongBusiness.Add(ps);
                };
                playlistMenu.Items.Add(playlistItem);
            }

            // Show the context menu at the current mouse position
            if (sender is Control control) // Ensure sender is a control to get position
            {
                 playlistMenu.Show(control, control.PointToClient(Cursor.Position));
            }
            else // Fallback if sender is not a control (though it should be)
            {
                 playlistMenu.Show(Cursor.Position);
            }
        }

        private void ConfigurePlayerPanel()
        {
            mainApp.PlayerPanel.Controls.Clear();
            mainApp.PlayerPanel.Height = PLAYER_HEIGHT;
            mainApp.PlayerPanel.BackColor = ColorTranslator.FromHtml(IsDarkMode ? "#3F3649" : "#AC8BEE");
            mainApp.PlayerPanel.Dock = DockStyle.Bottom;
            mainApp.PlayerPanel.Padding = new Padding(20);

            // Create playback panel
            var playbackPanel = new Panel
            {
                Width = 150,
                Height = 40,
                BackColor = ColorTranslator.FromHtml(IsDarkMode ? "#3F3649" : "#AC8BEE")
            };

            // Configure player controls
            mainApp.BtnPrev.Size = new Size(40, 40);
            mainApp.BtnPrev.Location = new Point(0, 0);
            mainApp.BtnPrev.FillColor = ColorTranslator.FromHtml(IsDarkMode ? "#3F3649" : "#AC8BEE");
            mainApp.BtnPrev.ForeColor = ColorTranslator.FromHtml(IsDarkMode ? "#C7ADFF" : "#3F3649");
            mainApp.BtnPrev.Text = "⏮";
            mainApp.BtnPrev.Font = new Font("Arial Rounded MT Bold", 12);
            mainApp.BtnPrev.Cursor = Cursors.Hand;

            mainApp.BtnPlay.Size = new Size(40, 40);
            mainApp.BtnPlay.Location = new Point(55, 0);
            mainApp.BtnPlay.FillColor = ColorTranslator.FromHtml(IsDarkMode ? "#3F3649" : "#AC8BEE");
            mainApp.BtnPlay.ForeColor = ColorTranslator.FromHtml(IsDarkMode ? "#C7ADFF" : "#3F3649");
            mainApp.BtnPlay.Text = "▶";
            mainApp.BtnPlay.Font = new Font("Arial Rounded MT Bold", 12);
            mainApp.BtnPlay.TextAlign = HorizontalAlignment.Center;
            mainApp.BtnPlay.Cursor = Cursors.Hand;

            mainApp.BtnNext.Size = new Size(40, 40);
            mainApp.BtnNext.Location = new Point(110, 0);
            mainApp.BtnNext.FillColor = ColorTranslator.FromHtml(IsDarkMode ? "#3F3649" : "#AC8BEE");
            mainApp.BtnNext.ForeColor = ColorTranslator.FromHtml(IsDarkMode ? "#C7ADFF" : "#3F3649");
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
                BackColor = ColorTranslator.FromHtml(IsDarkMode ? "#3F3649" : "#AC8BEE")
            };

            mainApp.BtnVolume.Size = new Size(30, 30);
            mainApp.BtnVolume.Location = new Point(0, 3);
            mainApp.BtnVolume.FillColor = ColorTranslator.FromHtml(IsDarkMode ? "#3F3649" : "#AC8BEE");
            mainApp.BtnVolume.ForeColor = ColorTranslator.FromHtml(IsDarkMode ? "#C7ADFF" : "#3F3649");
            mainApp.BtnVolume.Text = "🔊";
            mainApp.BtnVolume.Font = new Font("Arial Rounded MT Bold", 12);
            mainApp.BtnVolume.TextAlign = HorizontalAlignment.Center;
            mainApp.BtnVolume.BorderRadius = 5;
            mainApp.BtnVolume.Cursor = Cursors.Hand;

            mainApp.TrackBarVolume.Size = new Size(100, 23);
            mainApp.TrackBarVolume.Location = new Point(35, 5);
            mainApp.TrackBarVolume.ThumbColor = ColorTranslator.FromHtml(IsDarkMode ? "#C7ADFF" : "#3F3649");
            mainApp.TrackBarVolume.Value = 50;

            volumePanel.Controls.AddRange(new Control[] { mainApp.BtnVolume, mainApp.TrackBarVolume });
            volumePanel.Location = new Point(mainApp.PlayerPanel.Width - volumePanel.Width - 20, 35);

            // Configure progress bar and labels
            mainApp.TrackBarProgress.Size = new Size(mainApp.PlayerPanel.Width - 600, 23);
            mainApp.TrackBarProgress.Location = new Point(300, 60);
            mainApp.TrackBarProgress.ThumbColor = ColorTranslator.FromHtml(IsDarkMode ? "#C7ADFF" : "#3F3649");

            mainApp.LblCurrentTime.Text = "0:00";
            mainApp.LblCurrentTime.Location = new Point(260, 60);
            mainApp.LblCurrentTime.ForeColor = ColorTranslator.FromHtml(IsDarkMode ? "#C7ADFF" : "#3F3649");
            mainApp.LblCurrentTime.Font = new Font("Arial Rounded MT Bold", 10);

            mainApp.LblTotalTime.Text = "0:00";
            mainApp.LblTotalTime.Location = new Point(mainApp.PlayerPanel.Width - 280, 60);
            mainApp.LblTotalTime.ForeColor = ColorTranslator.FromHtml(IsDarkMode ? "#C7ADFF" : "#3F3649");
            mainApp.LblTotalTime.Font = new Font("Arial Rounded MT Bold", 10);

            mainApp.LblCurrentSong.Text = "No song playing";
            mainApp.LblCurrentSong.Location = new Point(300, 20);
            mainApp.LblCurrentSong.ForeColor = ColorTranslator.FromHtml(IsDarkMode ? "#C7ADFF" : "#3F3649");
            mainApp.LblCurrentSong.Font = new Font("Arial Rounded MT Bold", 12);

            mainApp.LblCurrentArtist.Text = "Unknown artist";
            mainApp.LblCurrentArtist.Location = new Point(300, 40);
            mainApp.LblCurrentArtist.ForeColor = ColorTranslator.FromHtml(IsDarkMode ? "#C7ADFF" : "#3F3649");
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
            mainApp.TrackBarProgress.MouseDown -= ProgressBar_MouseDown;
            mainApp.TrackBarProgress.MouseUp -= ProgressBar_MouseUp;
            mainApp.TrackBarProgress.Scroll -= ProgressBar_Scroll;

            // Add the handlers
            mainApp.BtnPlay.Click += PlayButton_Click;
            mainApp.BtnPrev.Click += PrevButton_Click;
            mainApp.BtnNext.Click += NextButton_Click;
            mainApp.BtnVolume.Click += VolumeButton_Click;
            mainApp.TrackBarVolume.ValueChanged += VolumeSlider_ValueChanged;
            mainApp.TrackBarProgress.MouseDown += ProgressBar_MouseDown;
            mainApp.TrackBarProgress.MouseUp += ProgressBar_MouseUp;
            mainApp.TrackBarProgress.Scroll += ProgressBar_Scroll;
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            TogglePlayPause();
        }

        private void PrevButton_Click(object sender, EventArgs e)
        {
            PlayAdjacentSong(-1); // Play previous
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
             PlayAdjacentSong(1); // Play next
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
            // Volume setting on waveOut is generally thread-safe, but locking doesn't hurt
            lock (playbackLock)
            {
                if (waveOut != null)
                {
                    waveOut.Volume = mainApp.TrackBarVolume.Value / 100f;
                }
            }
            // Update UI (queues to UI thread, safe outside lock)
            Action updateBtn = () => { if (mainApp?.BtnVolume != null) mainApp.BtnVolume.Text = mainApp.TrackBarVolume.Value == 0 ? "🔈" : "🔊"; };
            if (mainApp.InvokeRequired) mainApp.Invoke(updateBtn); else updateBtn();
        }

        private void ProgressBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentPlayingSong != null)
            {
                isDraggingSlider = true;
                // Optional: Pause playback while seeking for smoother experience
                // if (isPlaying) {
                //    playbackTimer.Stop();
                // }
            }
        }

        private void ProgressBar_MouseUp(object sender, MouseEventArgs e)
        {
            lock (playbackLock)
            {
                if (currentPlayingSong != null && audioReader != null && isDraggingSlider)
                {
                    try
                    {
                        double clickPositionRatio = Math.Max(0, Math.Min(1, (double)e.X / mainApp.TrackBarProgress.Width));
                        currentPositionSeconds = clickPositionRatio * totalDurationSeconds;

                        audioReader.CurrentTime = TimeSpan.FromSeconds(currentPositionSeconds);

                        // Update UI labels/slider (queue to UI thread, safe outside lock)
                        Action updateSeekUI = () => {
                            if (mainApp?.TrackBarProgress != null) mainApp.TrackBarProgress.Value = (int)currentPositionSeconds;
                            if (mainApp?.LblCurrentTime != null) mainApp.LblCurrentTime.Text = FormatTime(currentPositionSeconds);
                        };
                        if (mainApp.InvokeRequired) mainApp.Invoke(updateSeekUI); else updateSeekUI();

                        Debug.WriteLine($"Seeked to: {FormatTime(currentPositionSeconds)}");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error seeking: {ex.Message}");
                    }
                }
            }
            isDraggingSlider = false; // Reset flag outside lock
        }

         // Update time label continuously while scrolling if desired
        private void ProgressBar_Scroll(object sender, EventArgs e)
        {
             if (currentPlayingSong != null && isDraggingSlider)
             {
                 // Update label based on slider value directly during scroll
                 mainApp.LblCurrentTime.Text = FormatTime(mainApp.TrackBarProgress.Value);
             }
        }

        private void TogglePlayPause()
        {
            lock (playbackLock)
            {
                if (currentPlayingSong == null || waveOut == null) return;

                if (isPlaying) // Pause
                {
                    if (waveOut.PlaybackState == PlaybackState.Playing)
                    {
                        try
                        {
                            waveOut.Pause();
                            isPlaying = false;
                            playbackTimer.Stop();
                            // Update UI (queues to UI thread, safe outside lock)
                            Action updateBtn = () => { if (mainApp?.BtnPlay != null) mainApp.BtnPlay.Text = "▶"; };
                            if (mainApp.InvokeRequired) mainApp.Invoke(updateBtn); else updateBtn();
                            Debug.WriteLine("Playback Paused");
                        }
                        catch (Exception ex) { Debug.WriteLine($"Error pausing: {ex.Message}"); }
                    }
                }
                else // Play (Resume)
                {
                    if (waveOut.PlaybackState == PlaybackState.Paused)
                    {
                        try
                        {
                            waveOut.Play();
                            isPlaying = true;
                            playbackTimer.Start();
                            // Update UI (queues to UI thread, safe outside lock)
                            Action updateBtn = () => { if (mainApp?.BtnPlay != null) mainApp.BtnPlay.Text = "⏸"; };
                            if (mainApp.InvokeRequired) mainApp.Invoke(updateBtn); else updateBtn();
                            Debug.WriteLine("Playback Resumed");
                        }
                        catch (Exception ex) { Debug.WriteLine($"Error resuming: {ex.Message}"); }
                    }
                    // Potentially handle case where state is Stopped (e.g., error occurred) - maybe replay?
                }
            }
        }

        private void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            if (isPlaying && !isDraggingSlider && currentPlayingSong != null && audioReader != null)
            {
                // Update position from the audio reader's current time
                currentPositionSeconds = audioReader.CurrentTime.TotalSeconds;

                // NAudio's PlaybackStopped event handles the end of the song more reliably
                // if (currentPositionSeconds >= totalDurationSeconds)
                // {
                //     StopPlaybackInternally(); // Use internal stop which doesn't reset UI fully yet
                // }
                // else
                // {
                     UpdatePlaybackUI();
                // }
            }
        }

        private void UpdatePlaybackUI()
        {
             if (mainApp.InvokeRequired)
             {
                 mainApp.Invoke(new Action(UpdatePlaybackUI));
                 return;
             }

            if (currentPlayingSong != null)
            {
                // Update Progress Bar based on audioReader's position
                totalDurationSeconds = audioReader?.TotalTime.TotalSeconds ?? totalDurationSeconds; // Update total duration if reader provides it
                int maxProgress = (int)Math.Max(1, totalDurationSeconds); // Ensure max is at least 1
                if (mainApp.TrackBarProgress.Maximum != maxProgress) {
                    mainApp.TrackBarProgress.Maximum = maxProgress;
                }
                int progressValue = (int)Math.Min(currentPositionSeconds, maxProgress);
                if (progressValue >= mainApp.TrackBarProgress.Minimum && progressValue <= maxProgress)
                {
                    mainApp.TrackBarProgress.Value = progressValue;
                }

                // Update Current Time Label
                mainApp.LblCurrentTime.Text = FormatTime(currentPositionSeconds);
                // Update Total Time Label (might change if reader loaded duration)
                mainApp.LblTotalTime.Text = FormatTime(totalDurationSeconds);
            }
        }

        private void PlaySong(Song song)
        {
            lock (playbackLock) // Lock before modifying playback state
            {
                isStoppingForNewSong = true; // Set flag: Stop is for loading new song
                StopPlaybackInternally(); // This call is now within the lock
                isStoppingForNewSong = false; // Reset flag

                if (song?.Data == null || song.Data.Length == 0)
                {
                    MessageBox.Show("Song data is missing or empty.", "Playback Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ResetPlaybackUI(); // Just ensure UI is reset (safe outside lock as it queues to UI thread)
                    return;
                }

                currentPlayingSong = song;
                currentPositionSeconds = 0;
                isDraggingSlider = false;

                try
                {
                    audioStream = new MemoryStream(song.Data);
                    audioReader = CreateWaveStream(audioStream); // Format detection doesn't need lock

                    if (audioReader == null)
                    {
                        MessageBox.Show("Could not recognize audio format.", "Playback Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                         ResetPlaybackUI(); // Safe outside lock
                        return;
                    }

                    totalDurationSeconds = audioReader.TotalTime.TotalSeconds;

                    // Update UI with new song info (queues to UI thread, safe outside lock)
                    if (mainApp.InvokeRequired)
                    { mainApp.Invoke(new Action(UpdateUIAfterSongLoad)); }
                    else
                    { UpdateUIAfterSongLoad(); }

                    // Initialize and Play within the lock
                    if (waveOut == null) { InitializeAudioPlaybackEngine(); } // Re-initialize if disposed
                    waveOut.Init(audioReader);
                    waveOut.Play();
                    isPlaying = true; // Set playing state *after* successful start
                    playbackTimer.Start(); // Timer start/stop is thread-safe
                    // Update button state (queues to UI thread, safe outside lock)
                    Action updateBtn = () => { if (mainApp?.BtnPlay != null) mainApp.BtnPlay.Text = "⏸"; };
                     if (mainApp.InvokeRequired) mainApp.Invoke(updateBtn); else updateBtn();

                    Debug.WriteLine($"Playing: {song.Title} (Format: {audioReader.GetType().Name})");
                }
                catch (Exception ex)
                {
                     Debug.WriteLine($"NAudio Playback Error in PlaySong: {ex}");
                    // Show error message (queues to UI thread, safe outside lock)
                     Action showError = () => MessageBox.Show($"Error playing song: {ex.Message}", "Playback Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     if (mainApp.InvokeRequired) mainApp.Invoke(showError); else showError();

                    StopPlaybackInternally(); // Clean up within the lock
                    ResetPlaybackUI(); // Reset UI (safe outside lock)
                }
            } // End Lock
        }

        // Helper to update UI elements after song loaded and reader created
        private void UpdateUIAfterSongLoad()
        {
             mainApp.LblCurrentSong.Text = currentPlayingSong.Title ?? "Unknown Title";
             mainApp.LblCurrentArtist.Text = currentPlayingSong.Artist ?? "Unknown Artist";
             mainApp.LblTotalTime.Text = FormatTime(totalDurationSeconds);
             mainApp.LblCurrentTime.Text = FormatTime(currentPositionSeconds);
             mainApp.TrackBarProgress.Minimum = 0;
             int maxProgress = (int)Math.Max(1, totalDurationSeconds); // Ensure max is at least 1
             mainApp.TrackBarProgress.Maximum = maxProgress;
             mainApp.TrackBarProgress.Value = 0;
        }

        // --- Format Detection Logic ---
        private WaveStream CreateWaveStream(MemoryStream stream)
        {
            stream.Position = 0; // Ensure we read from the beginning

            // Try reading as WAV
            try
            {
                var reader = new WaveFileReader(stream);
                if (reader.WaveFormat != null) // Basic check
                {
                    Debug.WriteLine("Detected WAV format.");
                    stream.Position = 0; // Reset position for the reader
                    return reader;
                }
            }
            catch (Exception ex) { Debug.WriteLine($"Not WAV: {ex.Message}"); /* Ignore */ }

            stream.Position = 0; // Reset position

            // Try reading as MP3
            try
            {
                var reader = new Mp3FileReader(stream);
                if (reader.WaveFormat != null) // Basic check
                {
                    Debug.WriteLine("Detected MP3 format.");
                    stream.Position = 0; // Reset position for the reader
                    return reader;
                }
            }
            catch (Exception ex) { Debug.WriteLine($"Not MP3: {ex.Message}"); /* Ignore */ }

            // Add checks for other formats here if needed (e.g., AiffFileReader, WmaFileReader)

            // If no format detected, return null
            stream.Position = 0; // Reset position
            return null;
        }
        // -----------------------------

        // Internal stop to release resources without fully resetting UI immediately
        private void StopPlaybackInternally()
        {
            // Assumes already within lock(playbackLock)
            playbackTimer.Stop();
            isPlaying = false;

            if (waveOut != null && waveOut.PlaybackState != PlaybackState.Stopped)
            {
                 try { waveOut.Stop(); } catch (Exception ex) { Debug.WriteLine($"Error stopping waveOut: {ex.Message}"); }
            }

            audioReader?.Dispose();
            audioReader = null;
            audioStream?.Dispose();
            audioStream = null;

            Debug.WriteLine("Internal Playback Stop");
        }

        // This method acquires the lock itself
        private void StopPlayback()
        {
            lock (playbackLock)
            {
                isStoppingForNewSong = true;
                StopPlaybackInternally(); // Call the internal stop *within* the lock
                isStoppingForNewSong = false;
            }
            // Reset UI outside the lock (queues to UI thread)
            if (mainApp.InvokeRequired) { mainApp.Invoke(new Action(ResetPlaybackUI)); }
            else { ResetPlaybackUI(); }
            Debug.WriteLine("Full Playback Stop and UI Reset");
        }

        private void ResetPlaybackUI()
        {
            // Add null checks for UI elements before accessing them
            if (mainApp?.BtnPlay != null) mainApp.BtnPlay.Text = "▶";
            if (mainApp?.TrackBarProgress != null) mainApp.TrackBarProgress.Value = mainApp.TrackBarProgress.Minimum;
            if (mainApp?.LblCurrentTime != null) mainApp.LblCurrentTime.Text = FormatTime(0);
            // Resetting other labels is optional
            // if (mainApp?.LblCurrentSong != null) mainApp.LblCurrentSong.Text = "No song playing";
            // if (mainApp?.LblCurrentArtist != null) mainApp.LblCurrentArtist.Text = "Unknown Artist";
            // if (mainApp?.LblTotalTime != null) mainApp.LblTotalTime.Text = FormatTime(0);
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
             // This event handler runs outside our lock, so we should avoid
             // modifying shared state directly here if possible, or re-acquire the lock.
             // For now, we focus on safe disposal and UI updates (which are queued).

            Debug.WriteLine("Playback Stopped Event Received");

            // Resource disposal - these are already nulled out in StopPlaybackInternally
            // which should have been called under lock before this event fires, but safe checks are ok.
            // WaveStream localReader = audioReader; // Avoid racing with null assignment? Maybe not needed with lock.
            // MemoryStream localStream = audioStream;
            // localReader?.Dispose();
            // localStream?.Dispose();

            Exception playbackException = args?.Exception;
            bool resetUi = false;
            bool playNext = false; // Default to not autoplaying
            string reason = "unknown";

            // Determine reason and action based on flags checked under lock
            lock (playbackLock)
            {
                if (playbackException != null) {
                    resetUi = true;
                    playNext = false;
                    reason = "playback error";
                }
                else if (isStoppingForNewSong) { // Stopped for user clicking a specific song
                    resetUi = false;
                    playNext = false;
                    reason = "switching songs";
                }
                else if (isSkippingTrack) { // Stopped because Next/Prev was clicked
                     resetUi = false; // Let the new song's PlaySong call handle UI updates
                     playNext = false;
                     reason = "track skipped";
                     // NOTE: isSkippingTrack flag is reset in PlayAdjacentSong
                }
                else { // Assume natural stop if none of the above
                    resetUi = true;
                    playNext = true;
                    reason = "natural stop";
                }
            }

            Debug.WriteLine($"OnPlaybackStopped: Reason='{reason}', NeedsReset={resetUi}, NeedsAutoplay={playNext}, IsPlaying={isPlaying}");

            if (playbackException != null)
            {
                // Show error message (queues to UI thread, safe outside lock)
                Action showError = () => {
                     if (mainApp != null) {
                          MessageBox.Show(mainApp, $"Playback error: {playbackException.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     }
                };
                if (mainApp != null && mainApp.InvokeRequired) { mainApp.Invoke(showError); } else { showError(); }
            }

            // Sanity Check: If reset/autoplay seems needed, but playback is already active again,
            // skip to prevent race conditions (should be less likely now, but keep for safety).
            if ((resetUi || playNext) && isPlaying)
            {
                 Debug.WriteLine("OnPlaybackStopped: Action needed but isPlaying is true. Race condition averted. Skipping UI reset/autoplay.");
                 resetUi = false;
                 playNext = false;
            }

            // Reset UI if needed
            if (resetUi)
            {
                 Debug.WriteLine($"OnPlaybackStopped: Resetting UI due to {reason}.");
                 Action resetAction = ResetPlaybackUI;
                 if (mainApp != null && mainApp.InvokeRequired) { mainApp.Invoke(resetAction); }
                 else if (mainApp != null) { resetAction(); }
            }
            else
            {
                 Debug.WriteLine($"OnPlaybackStopped: Skipping UI reset (Reason: {reason}, IsPlaying: {isPlaying}).");
            }

            // Trigger Autoplay if needed
            if (playNext)
            {
                Debug.WriteLine("OnPlaybackStopped: Natural end detected, playing next song.");
                mainApp?.BeginInvoke(new Action(() => PlayAdjacentSong(1)));
            }
        }

        // Helper to format time from seconds to m:ss
        private string FormatTime(double totalSeconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
            return $"{(int)time.TotalMinutes}:{time.Seconds:D2}";
        }

        // --- Refactored Song Display Logic ---
        private void DisplaySongs(List<Song> songsToDisplay)
        {
            var mainTableLayout = mainApp.MainPanel.Controls.OfType<TableLayoutPanel>().FirstOrDefault();
            var songsFlowPanel = mainTableLayout?.GetControlFromPosition(0, 1) as FlowLayoutPanel;

            if (songsFlowPanel == null) return;

            // --- Store the current list for playback controls --- 
            lock (playbackLock) // Lock when updating the list used by playback controls
            {
                currentSongList = songsToDisplay ?? new List<Song>(); // Ensure not null
            }
            // --------------------------------------------------

            // --- Clear Panel and Dispose Controls ---
            songsFlowPanel.SuspendLayout(); // Suspend layout for performance
            // Remove SizeChanged handler temporarily to avoid issues during clear
            songsFlowPanel.SizeChanged -= SongFlowPanel_SizeChanged;

            // Dispose existing controls properly
            while (songsFlowPanel.Controls.Count > 0)
            {
                Control ctrl = songsFlowPanel.Controls[0];
                if (ctrl is SongControl sc)
                {
                    sc.Click -= SongControl_Click;
                    sc.OnDelete -= SongControl_OnDelete;
                    sc.OnAddToPlaylist -= SongControl_OnAddToPlaylist;
                }
                songsFlowPanel.Controls.RemoveAt(0); // Remove before disposing
                ctrl.Dispose();
            }
            // songsFlowPanel.Controls.Clear(); // Should be empty now
            // ---------------------------------------

            // --- Add New Controls ---
            if (songsToDisplay != null)
            {
                foreach (var song in songsToDisplay)
                {
                    try
                    {
                         var songControl = new SongControl(song);
                         songControl.Width = songsFlowPanel.ClientSize.Width - songControl.Margin.Horizontal; // Initial width
                         songControl.Margin = new Padding(0, 0, 0, 10);
                         songControl.Cursor = Cursors.Hand;

                         songControl.Click += SongControl_Click;
                         songControl.OnDelete += SongControl_OnDelete;
                         songControl.OnAddToPlaylist += SongControl_OnAddToPlaylist;

                         songsFlowPanel.Controls.Add(songControl);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error creating SongControl for {song?.Title}: {ex.Message}");
                        // Optionally show an error or skip the song
                    }
                }
            }
            // -----------------------

            // Re-attach SizeChanged handler and resume layout
            songsFlowPanel.SizeChanged += SongFlowPanel_SizeChanged;
            songsFlowPanel.ResumeLayout(true);
            // Force layout update if needed
            // songsFlowPanel.PerformLayout();
             // Scroll to top after updating
             songsFlowPanel.ScrollControlIntoView(songsFlowPanel.Controls.Count > 0 ? songsFlowPanel.Controls[0] : songsFlowPanel);
        }
        // ------------------------------------

        // UpdateSongs now just gets the list and calls DisplaySongs
        public void UpdateSongs(Playlist playlist)
        {
            List<Song> songsToDisplay;
            var songBusiness = new SongBusiness(new VibelyDbContext());

            if (playlist == null)
            {
                songsToDisplay = songBusiness.GetAll();
            }
            else
            {
                var playlistSongs = new PlaylistSongBusiness(new VibelyDbContext());
                songsToDisplay = playlistSongs.GetAllSongsInPlaylist(playlist);
            }

            // Clear search box when changing playlists
            if(mainApp != null) mainApp.TxtSearch.Text = "";

            DisplaySongs(songsToDisplay);
        }

        // Handler for resizing song controls within the flow panel
        private void SongFlowPanel_SizeChanged(object sender, EventArgs e)
        {
             if(sender is FlowLayoutPanel panel)
             {
                  foreach(SongControl sc in panel.Controls.OfType<SongControl>())
                  {
                     try { sc.Width = panel.ClientSize.Width - sc.Margin.Horizontal; }
                     catch { /* Ignore potential errors during resize storms */ }
                  }
             }
        }

        // --- New handler for playing song on click ---
        private void SongControl_Click(object sender, EventArgs e)
        {
            if (sender is SongControl songControl && songControl.Song != null)
            {
                PlaySong(songControl.Song);
            }
        }
        // ------------------------------------------

        private void InitializePlaybackTimer()
        {
            playbackTimer = new System.Windows.Forms.Timer();
            playbackTimer.Interval = 1000; // 1 second interval
            playbackTimer.Tick += PlaybackTimer_Tick;
        }

        private void InitializeAudioPlaybackEngine()
        {
            waveOut = new WaveOutEvent();
            waveOut.PlaybackStopped += OnPlaybackStopped;
            // Set initial volume from the UI slider
            VolumeSlider_ValueChanged(null, EventArgs.Empty);
        }

        private void InitializeSearchDebounceTimer()
        {
            searchDebounceTimer = new System.Windows.Forms.Timer();
            searchDebounceTimer.Interval = SEARCH_DEBOUNCE_INTERVAL_MS;
            searchDebounceTimer.Tick += SearchDebounceTimer_Tick;
            // No need to start it here, it starts on text change
        }

        private void PlayAdjacentSong(int direction) // direction: 1 for next, -1 for previous
        {
            lock (playbackLock)
            {
                if (currentSongList == null || currentSongList.Count == 0 || currentPlayingSong == null)
                {
                    Debug.WriteLine("Cannot play next/prev: No song list or current song.");
                    return;
                }

                int currentIndex = currentSongList.FindIndex(s => s.Id == currentPlayingSong.Id);
                if (currentIndex == -1)
        {
                    Debug.WriteLine("Cannot play next/prev: Current song not found in list.");
                    return; // Current song isn't in the list for some reason
                }

                int nextIndex = currentIndex + direction;

                // Handle wrap-around
                if (nextIndex < 0)
                {
                    nextIndex = currentSongList.Count - 1; // Wrap to last song
                }
                else if (nextIndex >= currentSongList.Count)
                {
                    nextIndex = 0; // Wrap to first song
                }

                if (nextIndex >= 0 && nextIndex < currentSongList.Count)
                {
                     Song nextSong = currentSongList[nextIndex];
                     Debug.WriteLine($"Playing {(direction > 0 ? "next" : "previous")} song: {nextSong.Title}");

                     isSkippingTrack = true; // Set flag BEFORE calling PlaySong
                     try
                     {
                         PlaySong(nextSong);
                     }
                     finally
                     {
                         // Ensure flag is reset even if PlaySong throws an error
                         // although PlaySong should handle its own cleanup
                         isSkippingTrack = false;
                     }
                }
                 else { /* ... error handling ... */ }
            }
        }

        // --- IDisposable Implementation ---
        protected virtual void Dispose(bool disposing)
        {
             lock (playbackLock) // Ensure disposal is synchronized
             {
                 if (!disposedValue)
                 {
                     if (disposing)
                     {
                         StopPlaybackInternally(); // Clean up streams/reader within lock
                         if (waveOut != null)
                         {
                             waveOut.PlaybackStopped -= OnPlaybackStopped;
                             waveOut.Dispose();
                             waveOut = null;
                         }
                         if (playbackTimer != null)
                         { // Timer disposal is safe outside lock if needed, but fine here
                             playbackTimer.Dispose();
                             playbackTimer = null;
                         }

                         // Dispose search timer
                         if (searchDebounceTimer != null)
                         {
                             searchDebounceTimer.Stop(); // Ensure stopped
                             searchDebounceTimer.Tick -= SearchDebounceTimer_Tick;
                             searchDebounceTimer.Dispose();
                             searchDebounceTimer = null;
                         }
                     }
                     disposedValue = true;
                 }
             }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~UIConfigurator()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        // ----------------------------------
    }
}

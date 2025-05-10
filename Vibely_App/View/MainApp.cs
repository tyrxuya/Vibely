using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vibely_App.Business;
using Vibely_App.Data;
using Vibely_App.Data.Models;
using Vibely_App.Controls;
using Guna.UI2.WinForms;
using System.Diagnostics;

namespace Vibely_App.View
{
    public partial class MainApp : Form
    {
        private UIConfigurator uiConfigurator;
        public User ActiveUser { get; private set; }

        // Properties to expose controls
        public Panel SidePanel => sidePanel;
        public Panel MainPanel => mainPanel;
        public Panel PlayerPanel => playerPanel;
        public Guna2CirclePictureBox PctrUser => pctrUser;
        public Guna2HtmlLabel LblUserName => lblUserName;
        public Guna2TextBox TxtSearch => txtSearch;
        public Guna2Button BtnUpload => btnUpload;
        public Guna2Button BtnSearch => btnSearch;
        public Guna2Button BtnPlay => btnPlay;
        public Guna2Button BtnPrev => btnPrev;
        public Guna2Button BtnNext => btnNext;
        public Guna2Button BtnVolume => btnVolume;
        public Guna2TrackBar TrackBarProgress => trackBarProgress;
        public Guna2TrackBar TrackBarVolume => trackBarVolume;
        public Guna2HtmlLabel LblCurrentTime => lblCurrentTime;
        public Guna2HtmlLabel LblTotalTime => lblTotalTime;
        public Guna2HtmlLabel LblCurrentSong => lblCurrentSong;
        public Guna2HtmlLabel LblCurrentArtist => lblCurrentArtist;

        public MainApp(User user)
        {
            ActiveUser = user;
            InitializeComponent();

            uiConfigurator = new UIConfigurator(this, user);
            uiConfigurator.InitializeUI();

            // Ensure application exits when this main form closes
            this.FormClosed += MainApp_FormClosed;

            this.btnUpload.Click += new EventHandler(this.BtnUpload_Click);
        }

        private void MainApp_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Force the application to exit, terminating any lingering threads
            Environment.Exit(0);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // uiConfigurator?.InitializeUI(); // Initialization is already done in constructor
        }

        private void MainApp_VisibleChanged(object sender, EventArgs e)
        {
            uiConfigurator.InitializeUI();
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                openFileDialog.Filter = "Music Files|*.mp3;*.wav;*.flac;*.m4a;*.aac|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    string fileExtension = Path.GetExtension(selectedFilePath).ToLowerInvariant();

                    // Validate file type
                    string[] validExtensions = { ".mp3", ".wav", ".flac", ".m4a", ".aac" };
                    if (validExtensions.Contains(fileExtension))
                    {
                        try
                        {
                            // Read file into byte array
                            byte[] fileData = File.ReadAllBytes(selectedFilePath);

                            var songBusiness = new SongBusiness(new VibelyDbContext());

                            // Placeholder call to the business layer
                            // Replace this with your actual business logic call
                            // musicService.UploadMusicFile(fileData, Path.GetFileName(selectedFilePath));

                            bool success = songBusiness.UploadSong(ActiveUser, fileData, Path.GetFileName(selectedFilePath));

                            MessageBox.Show($"File '{Path.GetFileName(selectedFilePath)}' selected and read successfully. Size: {fileData.Length} bytes. Ready for business logic processing.", "File Uploaded", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // --- Business Layer Logic ---
                            // Here you would typically call a method in your Business layer
                            // Pass the byte array 'fileData' and perhaps the original filename.
                            // Example:
                            // bool success = _musicUploadService.ProcessUploadedFile(fileData, Path.GetFileName(selectedFilePath));
                            // if (success) {
                            //     // Update UI, maybe refresh a song list
                            // } else {
                            //     MessageBox.Show("Failed to process the uploaded file.", "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            // }
                            // -----------------------------

                            if (success)
                            {
                                uiConfigurator.UpdateSongs(null);
                            }
                            else
                            {
                                Debug.Write("Song not uploaded");
                            }

                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show($"Error reading file: {ex.Message}", "File Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid file type selected. Please select a valid music file (.mp3, .wav, .flac, .m4a, .aac).", "Invalid File Type", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
    }
}

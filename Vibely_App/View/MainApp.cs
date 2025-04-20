using System;
using System.Drawing;
using System.Windows.Forms;
using Vibely_App.Data.Models;
using Vibely_App.Controls;
using Guna.UI2.WinForms;

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
    }
}

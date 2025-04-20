using Guna.UI2.WinForms;
using System.Windows.Forms;

namespace Vibely_App.View
{
    partial class MainApp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            sidePanel = new Guna2Panel();
            profilePanel = new Guna2Panel();
            pctrUser = new Guna2CirclePictureBox();
            lblUserName = new Guna2HtmlLabel();
            mainPanel = new Guna2Panel();
            playerPanel = new Guna2Panel();
            btnUpload = new Guna2Button();
            txtSearch = new Guna2TextBox();
            btnSearch = new Guna2Button();
            btnPlay = new Guna2Button();
            btnPrev = new Guna2Button();
            btnNext = new Guna2Button();
            btnVolume = new Guna2Button();
            trackBarProgress = new Guna2TrackBar();
            trackBarVolume = new Guna2TrackBar();
            lblCurrentTime = new Guna2HtmlLabel();
            lblTotalTime = new Guna2HtmlLabel();
            lblCurrentSong = new Guna2HtmlLabel();
            lblCurrentArtist = new Guna2HtmlLabel();

            SuspendLayout();

            // MainApp
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1064, 681);
            Controls.Add(mainPanel);
            Controls.Add(sidePanel);
            Controls.Add(playerPanel);
            Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F);
            Margin = new Padding(4, 3, 4, 3);
            MinimumSize = new System.Drawing.Size(1080, 720);
            Name = "MainApp";
            Text = "Vibely";
            ResumeLayout(false);
        }

        #endregion

        private Guna2Panel sidePanel;
        private Guna2Panel profilePanel;
        private Guna2Panel mainPanel;
        private Guna2Panel playerPanel;
        private Guna2CirclePictureBox pctrUser;
        private Guna2HtmlLabel lblUserName;
        private Guna2Button btnUpload;
        private Guna2TextBox txtSearch;
        private Guna2Button btnSearch;
        private Guna2Button btnPlay;
        private Guna2Button btnPrev;
        private Guna2Button btnNext;
        private Guna2Button btnVolume;
        private Guna2TrackBar trackBarProgress;
        private Guna2TrackBar trackBarVolume;
        private Guna2HtmlLabel lblCurrentTime;
        private Guna2HtmlLabel lblTotalTime;
        private Guna2HtmlLabel lblCurrentSong;
        private Guna2HtmlLabel lblCurrentArtist;
    }
}
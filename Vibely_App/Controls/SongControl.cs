using System;
using System.Drawing;
using System.Windows.Forms;
using Vibely_App.Data.Models;
using Guna.UI2.WinForms;
using Vibely_App.View;

namespace Vibely_App.Controls
{
    public class SongControl : Panel
    {
        private Label lblTitle;
        private Label lblDuration;
        private Label lblArtist;
        private Label lblGenre;
        private Guna2Button btnAddToPlaylist;
        private Guna2Button btnDelete;

        public Song Song { get; private set; }
        public event EventHandler<Song> OnDelete;
        public event EventHandler<Song> OnAddToPlaylist;

        public SongControl(Song song)
        {
            Song = song;
            InitializeControl();
            SetupEventHandlers();
        }

        private void InitializeControl()
        {
            this.Size = new Size(800, 60);
            this.BackColor = ColorTranslator.FromHtml(UIConfigurator.IsDarkMode ? "#282828" : "#E0AAFF");
            this.Margin = new Padding(0, 0, 0, 5);
            this.Padding = new Padding(10);

            lblTitle = new Label
            {
                Text = Song.Title,
                ForeColor = ColorTranslator.FromHtml(UIConfigurator.IsDarkMode ? "#C7ADFF" : "#3F3649"),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            lblArtist = new Label
            {
                Text = Song.Artist,
                ForeColor = ColorTranslator.FromHtml(UIConfigurator.IsDarkMode ? "#C7ADFF" : "#3F3649"),
                Font = new Font("Segoe UI", 10),
                Location = new Point(10, 35),
                AutoSize = true
            };

            lblGenre = new Label
            {
                Text = Song.Genre.Name,
                ForeColor = ColorTranslator.FromHtml(UIConfigurator.IsDarkMode ? "#C7ADFF" : "#3F3649"),
                Font = new Font("Segoe UI", 10),
                Location = new Point(lblArtist.Right + 20, 35),
                AutoSize = true
            };

            lblDuration = new Label
            {
                Text = TimeSpan.FromSeconds(Song.Duration).ToString(@"mm\:ss"),
                ForeColor = ColorTranslator.FromHtml(UIConfigurator.IsDarkMode ? "#C7ADFF" : "#3F3649"),
                Font = new Font("Segoe UI", 10),
                Location = new Point(this.Width - 200, 20),
                AutoSize = true
            };

            btnAddToPlaylist = new Guna2Button
            {
                Text = "+",
                Size = new Size(32, 32),
                Location = new Point(this.Width - 140, 14),
                FillColor = ColorTranslator.FromHtml(UIConfigurator.IsDarkMode ? "#DAC7FF" : "#46325D"),
                ForeColor = UIConfigurator.IsDarkMode ? Color.Black : Color.White,
                BorderRadius = 16,
                Font = new Font("Arial", 11, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center,
                TextOffset = new Point(1, 1),
                Cursor = Cursors.Hand
            };

            btnDelete = new Guna2Button
            {
                Text = "×",
                Size = new Size(32, 32),
                Location = new Point(this.Width - 80, 14),
                FillColor = ColorTranslator.FromHtml(UIConfigurator.IsDarkMode ? "#DAC7FF" : "#46325D"),
                ForeColor = UIConfigurator.IsDarkMode ? Color.Black : Color.White,
                BorderRadius = 16,
                Font = new Font("Arial", 13, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center,
                TextOffset = new Point(1, -1),
                Cursor = Cursors.Hand
            };

            this.Controls.AddRange(new Control[] {
                lblTitle,
                lblArtist,
                lblGenre,
                lblDuration,
                btnAddToPlaylist,
                btnDelete
            });
        }

        private void SetupEventHandlers()
        {
            this.Resize += (s, e) =>
            {
                lblDuration.Location = new Point(this.Width - 200, 20);
                btnAddToPlaylist.Location = new Point(this.Width - 140, 14);
                btnDelete.Location = new Point(this.Width - 80, 14);
            };

            btnDelete.Click += (s, e) => OnDelete?.Invoke(this, Song);
            btnAddToPlaylist.Click += (s, e) => OnAddToPlaylist?.Invoke(this, Song);

            this.MouseEnter += (s, e) => this.BackColor = ColorTranslator.FromHtml(UIConfigurator.IsDarkMode ? "#383838" : "#B07CCF");
            this.MouseLeave += (s, e) => this.BackColor = ColorTranslator.FromHtml(UIConfigurator.IsDarkMode ? "#282828" : "#E0AAFF");
        }
    }
}


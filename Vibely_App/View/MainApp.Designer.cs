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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainApp));
            playerPanel = new Panel();
            mainPanel = new Panel();
            sidePanel = new FlowLayoutPanel();
            pctrUser = new PictureBox();
            lblUserName = new Label();
            btnUpload = new Button();
            btnSearch = new Button();
            textBox1 = new TextBox();
            mainPanel.SuspendLayout();
            sidePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pctrUser).BeginInit();
            SuspendLayout();
            // 
            // playerPanel
            // 
            playerPanel.BackColor = Color.FromArgb(63, 54, 73);
            playerPanel.Dock = DockStyle.Bottom;
            playerPanel.ForeColor = Color.FromArgb(199, 173, 255);
            playerPanel.Location = new Point(0, 602);
            playerPanel.Name = "playerPanel";
            playerPanel.Size = new Size(1064, 79);
            playerPanel.TabIndex = 0;
            // 
            // mainPanel
            // 
            mainPanel.BackColor = Color.FromArgb(27, 27, 27);
            mainPanel.Controls.Add(sidePanel);
            mainPanel.Controls.Add(btnUpload);
            mainPanel.Controls.Add(btnSearch);
            mainPanel.Controls.Add(textBox1);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.ForeColor = Color.FromArgb(199, 173, 255);
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(1064, 602);
            mainPanel.TabIndex = 2;
            // 
            // sidePanel
            // 
            sidePanel.AutoScroll = true;
            sidePanel.BackColor = Color.FromArgb(25, 0, 40);
            sidePanel.Controls.Add(pctrUser);
            sidePanel.Controls.Add(lblUserName);
            sidePanel.Dock = DockStyle.Left;
            sidePanel.Location = new Point(0, 0);
            sidePanel.Name = "sidePanel";
            sidePanel.Size = new Size(250, 602);
            sidePanel.TabIndex = 3;
            // 
            // pctrUser
            // 
            pctrUser.Location = new Point(20, 50);
            pctrUser.Margin = new Padding(20, 50, 20, 50);
            pctrUser.Name = "pctrUser";
            pctrUser.Size = new Size(70, 70);
            pctrUser.SizeMode = PictureBoxSizeMode.StretchImage;
            pctrUser.TabIndex = 0;
            pctrUser.TabStop = false;
            // 
            // lblUserName
            // 
            lblUserName.AutoSize = true;
            lblUserName.Location = new Point(110, 75);
            lblUserName.Margin = new Padding(0, 75, 0, 0);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(77, 17);
            lblUserName.TabIndex = 1;
            lblUserName.Text = "John Doe";
            // 
            // btnUpload
            // 
            btnUpload.FlatAppearance.BorderSize = 5;
            btnUpload.Location = new Point(304, 25);
            btnUpload.Name = "btnUpload";
            btnUpload.Size = new Size(67, 23);
            btnUpload.TabIndex = 2;
            btnUpload.Text = "Upload";
            btnUpload.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            btnSearch.FlatAppearance.BorderSize = 5;
            btnSearch.Location = new Point(912, 25);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(77, 23);
            btnSearch.TabIndex = 1;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(377, 25);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(529, 25);
            textBox1.TabIndex = 0;
            // 
            // MainApp
            // 
            AutoScaleDimensions = new SizeF(9F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1064, 681);
            Controls.Add(mainPanel);
            Controls.Add(playerPanel);
            Font = new Font("Arial Rounded MT Bold", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximumSize = new Size(1080, 720);
            MinimumSize = new Size(1080, 720);
            Name = "MainApp";
            Text = "Vibely";
            mainPanel.ResumeLayout(false);
            mainPanel.PerformLayout();
            sidePanel.ResumeLayout(false);
            sidePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pctrUser).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel playerPanel;
        private Panel mainPanel;
        private Button btnSearch;
        private TextBox textBox1;
        private Button btnUpload;
        private FlowLayoutPanel sidePanel;
        private PictureBox pctrUser;
        private Label lblUserName;
    }
}
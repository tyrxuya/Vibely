using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vibely_App.View
{
    public partial class MainApp : Form
    {
        public MainApp()
        {
            InitializeComponent();
            for (int i = 0; i < 20; i++) 
            {
                Button button = new Button();
                button.Location = new Point(10, 10 + (i * 30));
                button.Size = new Size(200, 50);
                sidePanel.Controls.Add(button);
            }
            pctrUser.Image = Image.FromFile("C:\\Users\\ivan2\\Pictures\\Screenshots\\Screenshot 2025-04-06 023950.png");
        }
    }
}

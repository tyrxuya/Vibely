﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vibely_App.Controls
{
    internal class IconHelper
    {
        public static void AddIconToControl(Panel panel, Control control, Icon icon)
        {
            PictureBox pictureBox = new PictureBox
            {
                Image = icon.ToBitmap(),
                Size = new Size(20, 20),
                Location = new Point(control.Left - 25, (control.Top - 2) + (control.Height - 20) / 2), 
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            pictureBox.BackColor = Color.Transparent;
            pictureBox.Parent = panel;
            panel.Controls.Add(pictureBox);
        }
    }
}

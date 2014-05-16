using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LinqExample
{
    public partial class StretchedImageButton : Button
    {
        public StretchedImageButton()
        {
            InitializeComponent();
        }

        public new Image Image
        {
            get { return base.Image; }
            set
            {
                Image newImage = new Bitmap(Width, Height);
                using (Graphics g = Graphics.FromImage(newImage))
                {
                    g.DrawImage(value, 0, 0, Width, Height);
                }
                base.Image = newImage;
            }
        }
    }
}

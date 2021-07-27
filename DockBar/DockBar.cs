using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DockBar
{
    public partial class DockBar : UserControl
    {
        public Form[] Forms { get; set; }

        public DockStyle Dock { get; set; }

        public DockBar()
        {
            Dock = DockStyle.Left;
            Width = 100;
            //Height = Parent.Height;
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
    }
}

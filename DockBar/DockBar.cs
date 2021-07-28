using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DockBarControl
{
    [Designer(typeof(DockBarDesigner))]
    public partial class DockBar : UserControl
    {
        public Form[] Forms { get; set; }

        public DockStyle Dock { get; set; }

        public DockBar()
        {
            //Dock = DockStyle.Left;
            //Width = 100;
            //Height = Parent.Height;
            InitializeComponent();
            
            //TypeDescriptor.AddAttributes(ContentsPanel,
            //new DesignerAttribute(typeof(DockBarDesignerPanel)));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }      

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Design;

namespace DockBarControl
{
    [Designer(typeof(DockBarDesigner))]
    public partial class DockBar : UserControl
    {
        [Editor(typeof(DockBarFormListEditor), typeof(UITypeEditor))]
        public List<Form> FormList { get; set; } = new List<Form>();
        public DockBar()
        {   
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {   
            base.OnPaint(e);
            //foreach (Form f in FormList)
            //{
            //    Label l = new Label();
            //    l.Text = f.Text;
            //    Controls.Add(l);
            //}
        }
    }
}

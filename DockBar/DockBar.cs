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
using System.Drawing.Text;
using System.Drawing.Drawing2D;

namespace DockBarControl
{
    [Designer(typeof(DockBarDesigner))]
    public partial class DockBar : UserControl
    {
        private List<Form> _Forms = new List<Form>();

        [Editor(typeof(DockBarFormListEditor), typeof(UITypeEditor))]
        public Form[] Forms { get => _Forms.ToArray(); }

        public DockBar()
        {   
            InitializeComponent();
        }

        public void AddForm(Form value)
        {
            _Forms.Add(value);
        }

        protected override void OnPaint(PaintEventArgs e)
        {   
            base.OnPaint(e);
            int startPosition = 5;            
            foreach (Form f in Forms)
            {
                StringFormat sf = new StringFormat(StringFormatFlags.DirectionVertical);                
                SizeF size = e.Graphics.MeasureString(f.Text, Font, 0, sf);

                e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                //e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
                //e.Graphics.CompositingMode = CompositingMode.SourceOver;
                e.Graphics.FillRectangle(Brushes.LightGray, 0, startPosition, 7, size.Height);
                e.Graphics.DrawString(f.Text, Font, Brushes.Black, new PointF(10, startPosition), sf);
                startPosition += (int)size.Height;
                startPosition += 10;
            }
            
        }
    }
}

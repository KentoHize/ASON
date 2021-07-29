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
        protected List<Form> _Forms;

        protected List<int> _TextWidths;

        protected Form _CurrentForm;

        protected int _StartPosition;

        protected int _CurrentPosition;

        protected int _CurrentFormIndex;

        protected int _MouseOverFormIndex;
        public Form CurrentForm { get => _CurrentForm; }
        public int CurrentFormIndex { get => _CurrentFormIndex; }

        [DefaultValue(5)]
        public int TextInterval { get; set; }

        //[DefaultValue(typeof(Color), "Blue")]
        public Color NormalColor { get; set; }
        public Color MouseOverColor { get; set; }

        [Editor(typeof(DockBarFormListEditor), typeof(UITypeEditor))]
        public List<Form> Forms { get => _Forms; }

        public DockBar()
        {
            InitializeComponent();
            _Forms = new List<Form>();
            _TextWidths = new List<int>();
            TextInterval = 5;
            _StartPosition = 5;
            _CurrentPosition = 0;
            _CurrentFormIndex = -1;
            _MouseOverFormIndex = -1;
            NormalColor = Color.LightGray;
            MouseOverColor = Color.Blue;
            _CurrentForm = null;
        }

        public void AddForm(Form value)
        {
            Size size = TextRenderer.MeasureText(value.Text, Font);
            value.TextChanged += Value_TextChanged;
            _Forms.Add(value);
            _TextWidths.Add(size.Width);
        }

        protected void UpdateFormSize(Form f)
        {
            int index = _Forms.IndexOf(f);
            if (index == -1)
                throw new ArgumentOutOfRangeException(nameof(f));
            _TextWidths[index] = TextRenderer.MeasureText(f.Text, Font).Width;
        }

        private void Value_TextChanged(object sender, EventArgs e)
        {
            UpdateFormSize(sender as Form);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int position = _StartPosition;
            for (int i = 0; i < Forms.Count; i++)
            {
                StringFormat sf = new StringFormat(StringFormatFlags.DirectionVertical | StringFormatFlags.NoClip | StringFormatFlags.NoWrap);
                if (_MouseOverFormIndex == i)
                {
                    e.Graphics.DrawString(_Forms[i].Text, Font, new SolidBrush(ForeColor), new PointF(10, position), sf);
                    e.Graphics.FillRectangle(new SolidBrush(MouseOverColor), 0, position, 7, _TextWidths[i]);
                }
                else
                {
                    e.Graphics.DrawString(_Forms[i].Text, Font, new SolidBrush(ForeColor), new PointF(10, position), sf);
                    e.Graphics.FillRectangle(new SolidBrush(NormalColor), 0, position, 7, _TextWidths[i]);
                }                
                position += _TextWidths[i] + TextInterval;
            }

            //TextRenderer.DrawText(e.Graphics, _Forms[i].Text, Font, new Point(0, 0), ForeColor, TextFormatFlags.PreserveGraphicsClipping);

            //StringFormat sf = new StringFormat(StringFormatFlags.DirectionVertical);                
            //SizeF size = e.Graphics.MeasureString(f.Text, Font, 0, sf);

            //e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            //e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            //e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            ////e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            ////e.Graphics.CompositingMode = CompositingMode.SourceOver;
            //e.Graphics.FillRectangle(Brushes.LightGray, 0, startPosition, 7, size.Height);
            //e.Graphics.DrawString(f.Text, Font, Brushes.Black, new PointF(10, startPosition), sf);
            //startPosition += (int)size.Height;
            //startPosition += 10;


        }
    }
}

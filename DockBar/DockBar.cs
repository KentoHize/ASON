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

        protected int _StartPosition;

        protected int _CurrentPosition;

        protected int _MouseOverFormIndex;
        public Form CurrentForm { get; protected set; }
        public int CurrentFormIndex { get; protected set; }
        public int DefaultWindowWidth { get; set; }

        [DefaultValue(5)]
        public int TextInterval { get; set; }

        //[DefaultValue(typeof(Color), "Blue")]
        //public Color NormalColor { get; set; }
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
            CurrentForm = null;
            CurrentFormIndex = -1;
            _MouseOverFormIndex = -1;
            //NormalColor = SystemColors.Control;
            MouseOverColor = SystemColors.MenuHighlight;
            DefaultWindowWidth = 200;
        }

        public void AddForm(Form value)
        {
            Size size = TextRenderer.MeasureText(value.Text, Font);
            value.TextChanged += Value_TextChanged;
            value.FormClosed += Value_FormClosed;
            _Forms.Add(value);
            _TextWidths.Add(size.Width);
            Refresh();
        }

        public void RemoveForm(Form value)
        {
            int index = _Forms.IndexOf(value);

            if (index != -1)
            {
                if (CurrentFormIndex == index)
                {
                    CurrentForm.Hide();
                    CurrentFormIndex = -1;
                    CurrentForm = null;
                }
                _Forms.RemoveAt(index);
                _TextWidths.RemoveAt(index);
            }
            Refresh();
        }

        private void Value_FormClosed(object sender, FormClosedEventArgs e)
        {
            RemoveForm(sender as Form);
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

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (_MouseOverFormIndex == -1)
                return;
            else if (CurrentFormIndex == _MouseOverFormIndex)
            {
                CurrentForm.Hide();
                CurrentFormIndex = -1;
                CurrentForm = null;
            }
            else
            {
                if (CurrentForm != null)
                    CurrentForm.Hide();
                CurrentFormIndex = _MouseOverFormIndex;
                CurrentForm = _Forms[CurrentFormIndex];
                CurrentForm.StartPosition = FormStartPosition.Manual;
                CurrentForm.Width = DefaultWindowWidth;
                Point p = PointToScreen(new Point(-5, 1));// NeedCheck
                CurrentForm.Height = Height;
                CurrentForm.Left = p.X + Width;
                CurrentForm.Top = p.Y;
                CurrentForm.Show(ParentForm);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _MouseOverFormIndex = -1;
            base.OnMouseLeave(e);
            Refresh();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int y = _StartPosition;
            int newMouseOverFormIndex = _MouseOverFormIndex;
            for (int i = 0; i < _TextWidths.Count; i++)
            {
                if (e.Y < y)
                {
                    newMouseOverFormIndex = -1;
                    break;
                }
                y += _TextWidths[i];
                if (e.Y < y)
                {
                    newMouseOverFormIndex = i;
                    break;
                }
                y += TextInterval;
            }
            base.OnMouseMove(e);
            if (newMouseOverFormIndex != _MouseOverFormIndex)
            {
                _MouseOverFormIndex = newMouseOverFormIndex;
                Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int position = _StartPosition;
            //e.Graphics.Clear(SystemColors.Control);
            for (int i = 0; i < Forms.Count; i++)
            {
                StringFormat sf = new StringFormat(StringFormatFlags.DirectionVertical | StringFormatFlags.NoClip | StringFormatFlags.NoWrap);
                if (_MouseOverFormIndex == i)
                {
                    e.Graphics.DrawString(_Forms[i].Text, Font, new SolidBrush(MouseOverColor), new PointF(10, position), sf);
                    e.Graphics.FillRectangle(new SolidBrush(MouseOverColor), 0, position, 7, _TextWidths[i]);
                }
                else
                {
                    e.Graphics.DrawString(_Forms[i].Text, Font, new SolidBrush(ForeColor), new PointF(10, position), sf);
                    //e.Graphics.DrawString(_Forms[i].Text, Font, new SolidBrush(Color.Blue), new PointF(10, position), sf);
                    e.Graphics.FillRectangle(new SolidBrush(BackColor), 0, position, 7, _TextWidths[i]);
                }
                position += _TextWidths[i] + TextInterval;
            }
            //            Console.WriteLine(MouseOverColor);

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

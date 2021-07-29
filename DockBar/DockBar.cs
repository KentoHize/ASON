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
        protected List<DockBarFormInfo> _FormsInfo;

        protected int _StartPosition;

        protected int _CurrentPosition;

        protected int _MouseOverFormIndex;
        public Form CurrentForm { get; protected set; }
        public int CurrentFormIndex { get; protected set; }
        public int DefaultWindowWidth { get; set; }

        public int WindowCaptionHeight { get; set; }

        public Color WindowCaptionColor { get; set; }

        [DefaultValue(5)]
        public int TextInterval { get; set; }

        //[DefaultValue(typeof(Color), "Blue")]
        //public Color NormalColor { get; set; }
        public Color MouseOverColor { get; set; }

        //[Editor(typeof(DockBarFormListEditor), typeof(UITypeEditor))]
        //public IReadOnlyList<Form> Forms { get => _FormsInfo.AsReadOnly(); }

        public DockBar()
        {
            InitializeComponent();
            _FormsInfo = new List<DockBarFormInfo>();
            TextInterval = 5;
            _StartPosition = 5;
            _CurrentPosition = 0;
            CurrentForm = null;
            CurrentFormIndex = -1;
            _MouseOverFormIndex = -1;
            MouseOverColor = SystemColors.MenuHighlight;
            DefaultWindowWidth = 200;
            WindowCaptionHeight = 20;
            WindowCaptionColor = SystemColors.MenuHighlight;
            Left = 0;
        }

        public void AddForm(Form value)
        {
            DockBarFormInfo dbfi = new DockBarFormInfo();
            Size size = TextRenderer.MeasureText(value.Text, Font);
            value.TextChanged += Value_TextChanged;
            value.FormClosed += Value_FormClosed;
            value.Paint += Value_Paint;
            dbfi.OrignialFBS = value.FormBorderStyle;
            value.FormBorderStyle = FormBorderStyle.None;
            foreach (Control c in value.Controls)
                c.Top += WindowCaptionHeight;
            dbfi.TextWidth = size.Width;
            dbfi.Form = value;
            _FormsInfo.Add(dbfi);
            Refresh();
        }

        private void Value_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(WindowCaptionColor), 0, 0, DefaultWindowWidth, WindowCaptionHeight);
        }

        protected int IndexOfForm(Form value)
        {
            for (int i = 0; i < _FormsInfo.Count; i++)
                if (value == _FormsInfo[i].Form)
                    return i;
            return -1;
        }

        public void RemoveForm(Form value)
        {
            int index = IndexOfForm(value);
            if (index != -1)
            {
                if (CurrentFormIndex == index)
                {
                    CurrentForm.Hide();
                    CurrentFormIndex = -1;
                    CurrentForm = null;
                }
                _FormsInfo[index].Form.TextChanged -= Value_TextChanged;
                _FormsInfo[index].Form.FormClosed -= Value_FormClosed;
                _FormsInfo[index].Form.Paint -= Value_Paint;
                _FormsInfo[index].Form.FormBorderStyle = _FormsInfo[index].OrignialFBS;
                foreach (Control c in _FormsInfo[index].Form.Controls)
                    c.Top -= WindowCaptionHeight;
                _FormsInfo.RemoveAt(index);
            }
            Refresh();
        }

        private void Value_FormClosed(object sender, FormClosedEventArgs e)
        {
            RemoveForm(sender as Form);
        }

        protected void UpdateFormSize(Form f)
        {
            int index = IndexOfForm(f);
            if (index == -1)
                throw new ArgumentOutOfRangeException(nameof(f));
            _FormsInfo[index].TextWidth = TextRenderer.MeasureText(f.Text, Font).Width;
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
                CurrentForm = _FormsInfo[CurrentFormIndex].Form;
                CurrentForm.StartPosition = FormStartPosition.Manual;
                CurrentForm.Width = DefaultWindowWidth;
                //Point p = PointToScreen(new Point(-5, 1));// NeedCheck
                Point p = PointToScreen(new Point(0, 0));// NeedCheck
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
            for (int i = 0; i < _FormsInfo.Count; i++)
            {
                if (e.Y < y)
                {
                    newMouseOverFormIndex = -1;
                    break;
                }
                y += _FormsInfo[i].TextWidth;
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
            for (int i = 0; i < _FormsInfo.Count; i++)
            {
                StringFormat sf = new StringFormat(StringFormatFlags.DirectionVertical | StringFormatFlags.NoClip | StringFormatFlags.NoWrap);
                if (_MouseOverFormIndex == i)
                {
                    e.Graphics.DrawString(_FormsInfo[i].Form.Text, Font, new SolidBrush(MouseOverColor), new PointF(10, position), sf);
                    e.Graphics.FillRectangle(new SolidBrush(MouseOverColor), 0, position, 7, _FormsInfo[i].TextWidth);
                }
                else
                {
                    e.Graphics.DrawString(_FormsInfo[i].Form.Text, Font, new SolidBrush(ForeColor), new PointF(10, position), sf);
                    //e.Graphics.DrawString(_Forms[i].Text, Font, new SolidBrush(Color.Blue), new PointF(10, position), sf);
                    e.Graphics.FillRectangle(new SolidBrush(BackColor), 0, position, 7, _FormsInfo[i].TextWidth);
                }
                position += _FormsInfo[i].TextWidth + TextInterval;
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

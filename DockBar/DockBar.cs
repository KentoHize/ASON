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
        protected bool _CurrentFormMoving { get; set; }

        protected Point MouseLocation { get; set; }

        public Color BarColor { get; set; }
        public int DefaultWindowWidth { get; set; }
        public int WindowCaptionHeight { get; set; }
        public Color WindowCaptionBackColor { get; set; }
        public Color WindowCaptionForeColor { get; set; }

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
            WindowCaptionBackColor = SystemColors.MenuHighlight;
            WindowCaptionForeColor = Color.White;
            BarColor = SystemColors.ControlLight;
        }

        public void AddForm(Form value)
        {
            DockBarFormInfo dbfi = new DockBarFormInfo();
            Size size = TextRenderer.MeasureText(value.Text, Font);
            value.TextChanged += DockBarForm_TextChanged;
            value.FormClosed += DockBarForm_FormClosed;
            value.Paint += DockBarForm_Paint;
            value.MouseDown += DockBarForm_MouseDown;
            value.MouseMove += DockBarForm_MouseMove;
            value.MouseUp += DockBarForm_MouseUp;
            dbfi.OrignialFBS = value.FormBorderStyle;
            value.FormBorderStyle = FormBorderStyle.None;
            foreach (Control c in value.Controls)
                c.Top += WindowCaptionHeight;
            dbfi.TextWidth = size.Width;
            dbfi.Form = value;
            dbfi.Float = false;
            dbfi.DefaultFormWidth = value.Width;
            dbfi.DefaultFormHeight = value.Height;
            _FormsInfo.Add(dbfi);
            Refresh();
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
                _FormsInfo[index].Form.TextChanged -= DockBarForm_TextChanged;
                _FormsInfo[index].Form.FormClosed -= DockBarForm_FormClosed;
                _FormsInfo[index].Form.Paint -= DockBarForm_Paint;
                _FormsInfo[index].Form.MouseDown -= DockBarForm_MouseDown;
                _FormsInfo[index].Form.MouseMove -= DockBarForm_MouseMove;
                _FormsInfo[index].Form.MouseUp -= DockBarForm_MouseUp;
                _FormsInfo[index].Form.FormBorderStyle = _FormsInfo[index].OrignialFBS;
                foreach (Control c in _FormsInfo[index].Form.Controls)
                    c.Top -= WindowCaptionHeight;
                _FormsInfo.RemoveAt(index);
            }
            Refresh();
        }

        private void DockBarForm_FormClosed(object sender, FormClosedEventArgs e)
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

        private void DockBarForm_TextChanged(object sender, EventArgs e)
        {
            UpdateFormSize(sender as Form);
        }

        private void DockBarForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (_CurrentFormMoving)
            {
                (sender as Form).Left += e.X - MouseLocation.X;
                (sender as Form).Top += e.Y - MouseLocation.Y;
            }
        }

        private void DockBarForm_MouseDown(object sender, MouseEventArgs e)
        {
            _CurrentFormMoving = false;
            if (new Rectangle(0, 0, (sender as Form).Width, WindowCaptionHeight).Contains(e.X, e.Y))
            {
                _CurrentFormMoving = true;
                MouseLocation = e.Location;
            }
        }

        private void DockBarForm_MouseUp(object sender, MouseEventArgs e)
        {
            _CurrentFormMoving = false;
            MouseLocation = Point.Empty;
        }

        private void DockBarForm_Paint(object sender, PaintEventArgs e)
        {
            int index = IndexOfForm(sender as Form);
            e.Graphics.FillRectangle(new SolidBrush(WindowCaptionBackColor), 0, 0, (sender as Form).Width, WindowCaptionHeight);
            e.Graphics.DrawString(_FormsInfo[index].Form.Text, Font, new SolidBrush(WindowCaptionForeColor), new Point(3, 0));
            e.Graphics.DrawRectangle(new Pen(BarColor), 0, 0, (sender as Form).Width - 1, (sender as Form).Height - 1);
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
                Point p = PointToScreen(new Point(0, 0));
                CurrentForm.Height = Height;
                CurrentForm.Left = p.X + Width;
                CurrentForm.Top = p.Y;
                _CurrentFormMoving = false;
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
            for (int i = 0; i < _FormsInfo.Count; i++)
            {
                StringFormat sf;
                if (Dock == DockStyle.Left || Dock == DockStyle.Right)
                {
                    sf = new StringFormat(StringFormatFlags.DirectionVertical | StringFormatFlags.NoClip | StringFormatFlags.NoWrap);
                    if (_MouseOverFormIndex == i)
                    {
                        e.Graphics.DrawString(_FormsInfo[i].Form.Text, Font, new SolidBrush(MouseOverColor), new PointF(10, position), sf);
                        e.Graphics.FillRectangle(new SolidBrush(MouseOverColor), 0, position, 7, _FormsInfo[i].TextWidth);
                    }
                    else
                    {
                        e.Graphics.DrawString(_FormsInfo[i].Form.Text, Font, new SolidBrush(ForeColor), new PointF(10, position), sf);
                        e.Graphics.FillRectangle(new SolidBrush(BarColor), 0, position, 7, _FormsInfo[i].TextWidth);
                        //e.Graphics.DrawString(_Forms[i].Text, Font, new SolidBrush(Color.Blue), new PointF(10, position), sf);
                    }
                }
                else
                {
                    sf = new StringFormat(StringFormatFlags.NoClip | StringFormatFlags.NoWrap);
                    if (_MouseOverFormIndex == i)
                    {
                        e.Graphics.DrawString(_FormsInfo[i].Form.Text, Font, new SolidBrush(MouseOverColor), new PointF(position, 5), sf);
                        e.Graphics.FillRectangle(new SolidBrush(MouseOverColor), position, Font.Height + 10, _FormsInfo[i].TextWidth, 7);
                    }
                    else
                    {
                        e.Graphics.DrawString(_FormsInfo[i].Form.Text, Font, new SolidBrush(ForeColor), new PointF(position, 5), sf);
                        e.Graphics.FillRectangle(new SolidBrush(BarColor), position, Font.Height + 10, _FormsInfo[i].TextWidth, 7);                        
                    }
                }
                position += _FormsInfo[i].TextWidth + TextInterval;
            }


        }
    }
}

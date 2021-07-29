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
        private const int FormEdgeWidth = 5;

        protected List<DockBarFormInfo> _FormsInfo;

        protected int _StartPosition;

        protected int _CurrentPosition;

        protected int _MouseOverFormIndex;
        public Form CurrentForm { get; protected set; }
        public int CurrentFormIndex { get; protected set; }
        protected bool _FormMoving { get; set; }    
        protected ResizeDirection _FormResizing { get; set; }            
        protected Point MouseLocation { get; set; }
        //protected Size OriginalFormSize { get; set; }
        public Color BarColor { get; set; }
        public int WindowCaptionHeight { get; set; }
        public Color WindowCaptionBackColor { get; set; }
        public Color WindowCaptionForeColor { get; set; }

        [DefaultValue(5)]
        public int TextInterval { get; set; }
        public Color MouseOverColor { get; set; }

        //[Editor(typeof(DockBarFormListEditor), typeof(UITypeEditor))]
        public IReadOnlyList<Form> Forms { get => _FormsInfo.Select(m => m.Form).ToList().AsReadOnly(); }

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
            WindowCaptionHeight = 20;
            WindowCaptionBackColor = SystemColors.MenuHighlight;
            WindowCaptionForeColor = Color.White;
            BarColor = SystemColors.ControlLight;
        }

        protected override void OnCreateControl()
        {
            base.CreateControl();
            ParentForm.Resize += ParentForm_Resize;            
        }

        private void ParentForm_Resize(object sender, EventArgs e)
        {
            if (CurrentForm != null)
                SetFormPostionAndSize(true);
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
            value.MaximizeBox = true;// MaximizeBox false:Dock, true:Float
            dbfi.OrignialFBS = value.FormBorderStyle;
            value.FormBorderStyle = FormBorderStyle.None;
            

            

            foreach (Control c in value.Controls)
                c.Top += WindowCaptionHeight;

            Button cb = new Button();
            Button hb = new Button();
            Button fb = new Button();
            fb.Width = 16;
            fb.Height = 16;
            fb.Left = value.Width - 30;
            fb.Top = 2;
            fb.BackColor = WindowCaptionBackColor;
            fb.FlatStyle = FlatStyle.Flat;
            fb.FlatAppearance.BorderSize = 0;
            fb.Text = "f";
            fb.Click += FloatButton_Click;
            value.Controls.Add(fb);
            dbfi.TextWidth = size.Width;
            dbfi.Form = value;            
            dbfi.DefaultFormWidth = value.Width;
            dbfi.DefaultFormHeight = value.Height;
            _FormsInfo.Add(dbfi);
            Refresh();
        }

        private void FloatButton_Click(object sender, EventArgs e)
        {
            Form f = (sender as Button).Parent as Form;
            if (f.MaximizeBox)
            {
                f.MaximizeBox = false;                
                //SetFormPostionAndSize(true)
            }
            else
            {
                f.MaximizeBox = true;
            }   
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
                    HideForm();
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

        protected void DockBarForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            RemoveForm(sender as Form);
        }

        protected override void OnDockChanged(EventArgs e)
        {
            base.OnDockChanged(e);
            if (Dock == DockStyle.Fill || Dock == DockStyle.None)
                throw new ArgumentOutOfRangeException(nameof(Dock));            
            HideForm();
            Refresh();
        }

        protected void UpdateFormSize(Form f)
        {
            int index = IndexOfForm(f);
            if (index == -1)
                throw new ArgumentOutOfRangeException(nameof(f));
            _FormsInfo[index].TextWidth = TextRenderer.MeasureText(f.Text, Font).Width;
        }

        protected ResizeDirection GetResizeDirection(Form f, Point location, ResizeDirection restrainedTo = ResizeDirection.None)
        {
            ResizeDirection result;
            if (location.X >= 0 && location.Y >= 0 && location.X <= FormEdgeWidth && location.Y <= FormEdgeWidth)
                result = ResizeDirection.TopLeft;
            else if (location.X >= f.Width - FormEdgeWidth && location.Y >= 0 && location.X <= f.Width && location.Y <= FormEdgeWidth)
                result = ResizeDirection.TopRight;
            else if (location.X >= 0 && location.Y >= f.Height - FormEdgeWidth && location.X <= FormEdgeWidth && location.Y <= f.Height)
                result = ResizeDirection.BottomLeft;
            else if (location.X >= f.Width - FormEdgeWidth && location.Y >= f.Height - FormEdgeWidth && location.X <= f.Width && location.Y <= f.Height)
                result = ResizeDirection.BottomRight;
            else if (location.X >= 0 && location.X <= f.Width && location.Y >= 0 && location.Y <= FormEdgeWidth)
                result = ResizeDirection.Top;
            else if (location.X >= 0 && location.X <= FormEdgeWidth && location.Y >= 0 && location.Y <= f.Height)
                result = ResizeDirection.Left;
            else if (location.X >= f.Width - FormEdgeWidth && location.X <= f.Width && location.Y >= 0 && location.Y <= f.Height)
                result = ResizeDirection.Right;
            else if (location.X >= 0 && location.X <= f.Width && location.Y >= f.Height - FormEdgeWidth && location.Y <= f.Height)
                result = ResizeDirection.Bottom;
            else
                result = ResizeDirection.None;

            if (result == ResizeDirection.None || restrainedTo == ResizeDirection.None)
                return result;
            else if (restrainedTo == result)
                return restrainedTo;
            else if (restrainedTo == ResizeDirection.Left &&
                (result == ResizeDirection.TopLeft || result == ResizeDirection.BottomLeft))
                return restrainedTo;
            else if (restrainedTo == ResizeDirection.Top &&
                (result == ResizeDirection.TopLeft || result == ResizeDirection.TopRight))
                return restrainedTo;
            else if (restrainedTo == ResizeDirection.Right &&
                (result == ResizeDirection.TopRight || result == ResizeDirection.BottomRight))
                return restrainedTo;
            else if (restrainedTo == ResizeDirection.Bottom &&
                (result == ResizeDirection.BottomLeft || result == ResizeDirection.BottomRight))
                return restrainedTo;            
            return ResizeDirection.None;
        }

        protected void DockBarForm_TextChanged(object sender, EventArgs e)
        {
            UpdateFormSize(sender as Form);
        }

        protected void DockBarForm_MouseMove(object sender, MouseEventArgs e)
        {
            Form f = sender as Form;
            if (_FormMoving)
            {
                f.Left += e.X - MouseLocation.X;
                f.Top += e.Y - MouseLocation.Y;
                return;
            }
            else if (_FormResizing != ResizeDirection.None)
            {   
                switch(_FormResizing)
                {
                    case ResizeDirection.Left:
                        f.Left += e.X - MouseLocation.X;
                        f.Width -= e.X - MouseLocation.X;                        
                        break;
                    case ResizeDirection.Top:
                        f.Top += e.Y - MouseLocation.Y;
                        f.Height -= e.Y - MouseLocation.Y;
                        break;
                }
                //f.Refresh();
                return;
            }
            
            ResizeDirection rd = ResizeDirection.None;
            if (f.MaximizeBox)
                rd = GetResizeDirection(f, e.Location);
            else if (Dock == DockStyle.Left)
                rd = GetResizeDirection(f, e.Location, ResizeDirection.Right);
            else if (Dock == DockStyle.Top)
                rd = GetResizeDirection(f, e.Location, ResizeDirection.Bottom);
            else if (Dock == DockStyle.Right)
                rd = GetResizeDirection(f, e.Location, ResizeDirection.Left);
            else if (Dock == DockStyle.Bottom)
                rd = GetResizeDirection(f, e.Location, ResizeDirection.Top);            
            f.Cursor = rd.GetResizeCursor();
        }

        protected void DockBarForm_MouseDown(object sender, MouseEventArgs e)
        {
            _FormMoving = false;
            Form f = sender as Form;
            ResizeDirection rd = ResizeDirection.None;
            if (f.MaximizeBox)
                rd = GetResizeDirection(f, e.Location);
            else if (Dock == DockStyle.Left)
                rd = GetResizeDirection(f, e.Location, ResizeDirection.Right);
            else if (Dock == DockStyle.Top)
                rd = GetResizeDirection(f, e.Location, ResizeDirection.Bottom);
            else if (Dock == DockStyle.Right)
                rd = GetResizeDirection(f, e.Location, ResizeDirection.Left);
            else if (Dock == DockStyle.Bottom)
                rd = GetResizeDirection(f, e.Location, ResizeDirection.Top);

            if (rd != ResizeDirection.None)
            {
                _FormResizing = rd;
                MouseLocation = e.Location;
                return;
            }

            if (e.X >= 0 && e.Y >= 0 && e.X < f.Width && e.Y <= WindowCaptionHeight)
            {
                _FormMoving = true;
                MouseLocation = e.Location;
            }
        }

        protected void DockBarForm_MouseUp(object sender, MouseEventArgs e)
        {   
            _FormMoving = false;
            _FormResizing = ResizeDirection.None;
            MouseLocation = Point.Empty;
            (sender as Form).Refresh();
        }

        protected void DockBarForm_Paint(object sender, PaintEventArgs e)
        {
            int index = IndexOfForm(sender as Form);
            //e.Graphics.DrawRectangle(new Pen(BarColor), 0, 0, (sender as Form).Width - 1, (sender as Form).Height - 1);
            e.Graphics.FillRectangle(new SolidBrush(WindowCaptionBackColor), 0, 0, (sender as Form).Width, WindowCaptionHeight);
            e.Graphics.DrawString(_FormsInfo[index].Form.Text, Font, new SolidBrush(WindowCaptionForeColor), new Point(3, 0));
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (_MouseOverFormIndex == -1)
                return;
            else if (CurrentFormIndex == _MouseOverFormIndex)
                HideForm();
            else
            {   
                HideForm();
                ShowForm(_MouseOverFormIndex);
            }
        }

        protected void ShowForm(int index)
        {
            CurrentFormIndex = index;
            CurrentForm = _FormsInfo[CurrentFormIndex].Form;
            CurrentForm.StartPosition = FormStartPosition.Manual;
            SetFormPostionAndSize();
            CurrentForm.Show(ParentForm);
        }

        protected void SetFormPostionAndSize(bool refresh = false)
        {
            _FormMoving = false;            
            Point p = PointToScreen(new Point(0, 0));
            switch (Dock)
            {
                case DockStyle.Left:
                    CurrentForm.Width = _FormsInfo[CurrentFormIndex].DefaultFormWidth;
                    CurrentForm.Height = Height;
                    CurrentForm.Left = p.X + Width;
                    CurrentForm.Top = p.Y;
                    break;
                case DockStyle.Bottom:
                    CurrentForm.Width = Width;
                    CurrentForm.Height = _FormsInfo[CurrentFormIndex].DefaultFormHeight;
                    CurrentForm.Left = p.X;
                    CurrentForm.Top = p.Y - CurrentForm.Height;
                    break;
                case DockStyle.Right:
                    CurrentForm.Width = _FormsInfo[CurrentFormIndex].DefaultFormWidth;
                    CurrentForm.Height = Height;
                    CurrentForm.Left = p.X - CurrentForm.Width;
                    CurrentForm.Top = p.Y;
                    break;
                case DockStyle.Top:
                    CurrentForm.Width = Width;
                    CurrentForm.Height = _FormsInfo[CurrentFormIndex].DefaultFormHeight;
                    CurrentForm.Left = p.X;
                    CurrentForm.Top = p.Y + Height;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Dock));
            }
            if (refresh)
                Refresh();
        }

        protected void HideForm()
        {
            if (CurrentForm == null)
                return;
            CurrentForm.Hide();
            CurrentFormIndex = -1;
            CurrentForm = null;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _MouseOverFormIndex = -1;
            base.OnMouseLeave(e);
            Refresh();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int axis = _StartPosition;
            int newMouseOverFormIndex = _MouseOverFormIndex;
            int i;
            for (i = 0; i < _FormsInfo.Count; i++)
            {
                if (((Dock == DockStyle.Left || Dock == DockStyle.Right) && e.Y < axis) ||
                    ((Dock == DockStyle.Top || Dock == DockStyle.Bottom) && e.X < axis))
                {
                    newMouseOverFormIndex = -1;
                    break;
                }
                axis += _FormsInfo[i].TextWidth;
                if (((Dock == DockStyle.Left || Dock == DockStyle.Right) && e.Y < axis) ||
                    ((Dock == DockStyle.Top || Dock == DockStyle.Bottom) && e.X < axis))
                {
                    newMouseOverFormIndex = i;
                    break;
                }
                axis += TextInterval;
            }

            if (i == _FormsInfo.Count)
                newMouseOverFormIndex = -1;

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
                Color foreColor = _MouseOverFormIndex == i ? MouseOverColor : ForeColor;
                Color barColor = _MouseOverFormIndex == i ? MouseOverColor : BarColor;
                if (Dock == DockStyle.Left || Dock == DockStyle.Right)
                {
                    sf = new StringFormat(StringFormatFlags.DirectionVertical | StringFormatFlags.NoClip | StringFormatFlags.NoWrap);
                    if (Dock == DockStyle.Left)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(barColor), 0, position, 7, _FormsInfo[i].TextWidth);
                        e.Graphics.DrawString(_FormsInfo[i].Form.Text, Font, new SolidBrush(foreColor), new PointF(10, position), sf);
                    }
                    else
                    {
                        e.Graphics.DrawString(_FormsInfo[i].Form.Text, Font, new SolidBrush(foreColor), new PointF(0, position), sf);
                        e.Graphics.FillRectangle(new SolidBrush(barColor), Font.Height + 5, position, 7, _FormsInfo[i].TextWidth);
                    }
                }
                else
                {
                    sf = new StringFormat(StringFormatFlags.NoClip | StringFormatFlags.NoWrap);
                    if (Dock == DockStyle.Top)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(barColor), position, 0, _FormsInfo[i].TextWidth, 7);
                        e.Graphics.DrawString(_FormsInfo[i].Form.Text, Font, new SolidBrush(foreColor), new PointF(position, 10), sf);
                    }
                    else
                    {
                        e.Graphics.DrawString(_FormsInfo[i].Form.Text, Font, new SolidBrush(foreColor), new PointF(position, 0), sf);
                        e.Graphics.FillRectangle(new SolidBrush(barColor), position, Font.Height + 5, _FormsInfo[i].TextWidth, 7);
                    }
                }
                position += _FormsInfo[i].TextWidth + TextInterval;
            }
        }
    }
}

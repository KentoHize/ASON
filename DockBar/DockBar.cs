﻿using System;
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
        protected bool _CurrentFormFloat { get; set; }
        protected Point MouseLocation { get; set; }
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

        protected void DockBarForm_TextChanged(object sender, EventArgs e)
        {
            UpdateFormSize(sender as Form);
        }

        protected void DockBarForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_CurrentFormFloat)
                return;
            if (_CurrentFormMoving)
            {
                (sender as Form).Left += e.X - MouseLocation.X;
                (sender as Form).Top += e.Y - MouseLocation.Y;
            }
        }

        protected void DockBarForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_CurrentFormFloat)
                return;
            _CurrentFormMoving = false;
            if (new Rectangle(0, 0, (sender as Form).Width, WindowCaptionHeight).Contains(e.X, e.Y))
            {
                _CurrentFormMoving = true;
                MouseLocation = e.Location;
            }
        }

        protected void DockBarForm_MouseUp(object sender, MouseEventArgs e)
        {   
            _CurrentFormMoving = false;
            MouseLocation = Point.Empty;
        }

        protected void DockBarForm_Paint(object sender, PaintEventArgs e)
        {
            int index = IndexOfForm(sender as Form);
            e.Graphics.DrawRectangle(new Pen(BarColor), 0, 0, (sender as Form).Width - 1, (sender as Form).Height - 1);
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
            _CurrentFormFloat = _FormsInfo[CurrentFormIndex].Float;
            SetFormPostionAndSize();
            CurrentForm.Show(ParentForm);
        }

        protected void SetFormPostionAndSize(bool refresh = false)
        {
            _CurrentFormMoving = false;            
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

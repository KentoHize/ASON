using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DockBarControl
{
    public enum ResizeDirection
    {
        None,
        TopLeft,
        Top,
        TopRight,
        Right,
        BottomRight,
        Bottom,
        BottomLeft,
        Left
    }

    public static partial class Extension
    {
        public static Cursor GetResizeCursor(this ResizeDirection rd)
        {   
            switch(rd)
            {
                case ResizeDirection.None:
                    return Cursors.Default;
                case ResizeDirection.Left:
                case ResizeDirection.Right:
                    return Cursors.SizeWE;
                case ResizeDirection.Top:
                case ResizeDirection.Bottom:
                    return Cursors.SizeNS;
                case ResizeDirection.TopLeft:
                case ResizeDirection.BottomRight:
                    return Cursors.SizeNWSE;
                case ResizeDirection.TopRight:
                case ResizeDirection.BottomLeft:
                    return Cursors.SizeNESW;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rd));
            }
        }
    }
}

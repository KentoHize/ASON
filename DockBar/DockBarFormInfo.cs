using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DockBarControl
{
    public class DockBarFormInfo
    {        
        public Form Form { get; set; }
        public int TextWidth { get; set; }
        public FormBorderStyle OrignialFBS { get; set; }
        public int DefaultFormWidth { get; set; }
        public int DefaultFormHeight { get; set; }
        public bool Float { get; set; }
    }
}

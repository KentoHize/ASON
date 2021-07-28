using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DockBarControl
{
    public class DockBarDesignerActionList : DesignerActionList
    {
        private DockBar control;
        public DockBarDesignerActionList(IComponent component)
            : base(component)
        {
            control = component as DockBar;
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();
            items.Add(new DesignerActionHeaderItem("TestHeader"));

            //items.Add(new DesignerActionPropertyItem("FormList", "FormList"));
            return items;
        }
    }
}

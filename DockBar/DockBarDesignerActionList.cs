using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockBarControl
{
    public class DockBarDesignerActionList : DesignerActionList
    {
        public DockBarDesignerActionList(IComponent component)
            : base(component)
        {
        //    this.designerActionUISvc = GetService(typeof(DesignerActionUIService))
        //as DesignerActionUIService;
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();
            items.Add(new DesignerActionHeaderItem("TestHeader"));
            return items;
        }
    }
}

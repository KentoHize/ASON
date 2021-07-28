using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace DockBarControl
{
    public class DockBarDesignerActionList : DesignerActionList
    {
        private DesignerActionUIService designerActionUISvc = null;
        public DockBarDesignerActionList(IComponent component)
            : base(component)
        {

            //this.designerActionUISvc
            if(designerActionUISvc == null)
                designerActionUISvc = GetService(typeof(DesignerActionUIService)) as DesignerActionUIService;
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();

            //Define static section header entries.
            items.Add(new DesignerActionHeaderItem("Test"));

            items.Add(new DesignerActionPropertyItem("BackColor",
                                 "Back Color", "Appearance",
                                 "Selects the background color."));
            return items;
        }
    }
}

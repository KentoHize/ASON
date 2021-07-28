using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DockBarControl
{   
    [System.Security.Permissions.PermissionSet
    (System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class DockBarDesigner : ParentControlDesigner
    {
        private DesignerActionListCollection actionLists;

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            //EnableDesignMode(((DockBar)Control).ContentsPanel, "TestDM");
        }

        public override bool CanParent(ControlDesigner controlDesigner)
        {
            return true;
        }

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (null == actionLists)
                {
                    actionLists = new DesignerActionListCollection();
                    actionLists.Add(new DockBarDesignerActionList(Component));
                }
                return actionLists;
            }
        }
        

        protected override void OnDragOver(DragEventArgs de)
        {
            base.OnDragOver(de);
            
        }
    }   
}

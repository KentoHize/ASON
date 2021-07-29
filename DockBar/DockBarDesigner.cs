using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Aritiafel.Characters.Heroes;

namespace DockBarControl
{
    public class DockBarDesigner : ControlDesigner
    {
        private DockBar control;

        private DesignerActionListCollection dalc;

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            control = component as DockBar;
        }

        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);            
            control.Dock = DockStyle.Left;            
            control.Width = 30;
        }

        //public override DesignerActionListCollection ActionLists
        //{
        //    get
        //    {
        //        if (dalc == null)
        //        {
        //            dalc = new DesignerActionListCollection();
        //            dalc.Add(new DockBarDesignerActionList(Component));
        //        }
        //        return dalc;
        //    }
        //}
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockBarControl
{
    public class DockBarFormListEditor : CollectionEditor
    {
        public DockBarFormListEditor(Type type)
            : base(type)
        { 
            
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            return base.EditValue(context, provider, value);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.None;
        }

        //public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        //{

        //    return UITypeEditorEditStyle.DropDown;
        //    //return UITypeEditorEditStyle.None; // disallow edit (hide the small browser button)            
        //    //return base.GetEditStyle(context);
        //}
    }
}

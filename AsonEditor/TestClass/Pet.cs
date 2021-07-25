using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsonEditor.TestClass
{
    public abstract class Pet : Creature
    {
        public bool IsLazy { get; set; }
        public Pet()
            : this(false)
        { }

        public Pet(bool isLazy)
            : base()
        {
            IsLazy = isLazy;
        }
    }
}

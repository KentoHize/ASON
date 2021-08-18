using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsonEditor.TestClass
{
    public class Pet : Creature
    {
        public bool IsLazy { get; set; }
        private string SpecialRace { get; set; }

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

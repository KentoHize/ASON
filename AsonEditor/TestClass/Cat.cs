using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsonEditor.TestClass
{
    public class Cat : Pet
    {
        public string Breed { get; set; }
        public bool HateDogs { get; set; }        
        public Cat()
            : base()
        { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aritiafel;

namespace AsonEditor.TestClass
{
    public class Dog : Pet
    {
        public string Breed { get; set; }
        public bool IsOmnivores { get; set; }
        public bool Sleepy { get; set; }
        public Dog()
            : base()
        { }
    }
}

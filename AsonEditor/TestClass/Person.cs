using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsonEditor.TestClass
{
    public class Person : Creature
    {
        public string Address { get; set; }
        public List<Person> Friends { get; set; }
        public List<Creature> Pets { get; set; }
        public Person()
            : base()
        { }

    }
}

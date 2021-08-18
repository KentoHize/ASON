using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aritiafel.Locations;

namespace AsonEditor.TestClass
{
    public enum Gender
    {
        Undefied = 0,        
        Male,
        Female,
        Asexual
    }

    public abstract class Creature
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public Gender Gender {get; set;}
        public DateTime Birthday { get; set; }        
        public Creature()
            : this("")
        { }

        public Creature(string name, string id = null, Gender gender = Gender.Asexual)
        {
            Name = name;
            if (id == null)
                id = IdentifyShop.GetNewID();
            ID = id;
            Gender = gender;            
        }

    }
}

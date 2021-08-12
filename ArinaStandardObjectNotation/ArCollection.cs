using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Artifacts
{
    public class ArCollection
    {
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public List<ArType> Types { get; set; }
        public List<ArObject> Objects { get; set; }
        
    }
}

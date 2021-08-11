using System;
using System.Collections.Generic;
using System.Text;

namespace ArinaStandardObjectNotation
{
    public class ArProperty
    {
        public string Name { get; set; }
        public ArType Type { get; set; }
        public bool IsKey { get; set; }
        public bool IsNullable { get; set; }
        List<object> Choices { get; set; }
        public string RegularExpression { get; set; }
        public string Description { get; set; }
        public string MinValue { get; set; }
        public string MaxValue { get; set; }
        public long MaxLength { get; set; }
        public bool AutoGenerateKey { get; set; }
        public bool IsUnique { get; set; }
        public int NumberOfRows { get; set; }
        public bool Display { get; set; }
        //Type

        //public string Name { get; set; }
        //public bool IsKey { get; set; }
        //public bool IsNullable { get; set; }
        //public JType Type { get; set; }
        //public List<string> Choices { get; set; }
        //public string FKTable { get; set; }
        //public string FKColumn { get; set; }
        //public int NumberOfRows { get; set; }
        //public bool Display { get; set; }
        //public string RegularExpression { get; set; }
        //public string Description { get; set; }
        //public string MinValue { get; set; }
        //public string MaxValue { get; set; }
        //public long MaxLength { get; set; }
        //public bool AutoGenerateKey { get; set; }
        //public bool IsUnique { get; set; }

    }
}

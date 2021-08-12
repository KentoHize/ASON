using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Artifacts
{
    
    //public enum GenericTypeList
    //{
    //    Array,
    //    List,
    //    HashTable
    //}

    public class ArType
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public int UsedBytesCount { get; set; }
        public bool IsValueType { get; set; }
        public bool IsStandardType { get; set; }
        //public GenericTypeList GenericType { get; set; }
        //public List<ArType> GenericSubTypes { get; set; }
        public List<ArProperty> Properties { get; set; }        
        public ArType()
        {
               
        }
        
    }
}

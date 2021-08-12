using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Aritiafel.Artifacts
{
    public class TomeOfEntities
    {
        public List<ArType> Index { get; set; }
        public Dictionary<string, ArCollection> Content { get; set; }

        public void Save(string file)
        {
            //using (FileStream fs = new FileStream(file, FileMode.Create))
            //{

            //}
        }


        public void Load(string folder)
        {
            //讀Index

            //讀Content
        }
        //Save
        //Load       
        

        //Read
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Aritiafel.Characters.Heroes;
using System.IO;
using AsonEditor;
using AsonEditor.TestClass;


namespace AsonTest
{
    [TestClass]
    public class MainTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void LogTest()
        {
            string testFile = $"{Tina.LogDirectory}\\Test.txt";

            FileStream fs = new FileStream(testFile, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write("ABCDEFFSDSADA");
            sw.Close();
            fs.Close();

            fs = new FileStream(testFile, FileMode.Open);

            StreamReader sr = new StreamReader(fs);
            fs.Position = 2;
            char[] buffer = new char[3];
            sr.ReadBlock(buffer, 0, 3);
            TestContext.WriteLine(new string(buffer));
            sr.Close();
            fs.Close();


            //Tina.SaveTextFile("adasd");

            //DateTime d = DateTime.Now;
            //TestContext.WriteLine(d.ToString("MM"));
            //TestContext.WriteLine(d.ToString("mm"));
        }

        [TestMethod]
        public void MemoryFileTest()
        {
            //var sample = SampleCreator.GetSample();
            //MemoryStream ms = new MemoryStream();
            
            //StreamWriter sw = new StreamWriter(ms);
            
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Aritiafel.Characters.Heroes;

namespace AsonTest
{
    [TestClass]
    public class MainTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void LogTest()
        {
            Tina.SaveTextFile("adasd");

            //DateTime d = DateTime.Now;
            //TestContext.WriteLine(d.ToString("MM"));
            //TestContext.WriteLine(d.ToString("mm"));
        }
    }
}

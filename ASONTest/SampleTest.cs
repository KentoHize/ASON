using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AsonEditor.TestClass;

namespace ASONTest
{
    [TestClass]
    public class SampleTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void GetSampleTest()
        {
            var a = SampleCreator.GetSample();
            for (int i = 0; i < a.Count; i++)
            {
                TestContext.WriteLine(a[i].Name);
            }
        }
    }
}

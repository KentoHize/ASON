using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Aritiafel.Characters.Heroes;

namespace AsonTest
{
    [TestClass]
    public class Backup
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void BackupProject()
        {
            Tina.SaveProject(ProjectChoice.ASON);
        }
    }
}

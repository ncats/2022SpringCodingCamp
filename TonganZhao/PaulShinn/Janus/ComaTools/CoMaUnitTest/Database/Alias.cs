using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FOTSLibrary.FOTSOrders;

namespace CoMaUnitTest.Database
{
    [TestClass]
    public class Alias
    {
        [TestMethod]
        public void GetAliases()
        {
            Compound cmpd = new Compound("NCGC00013495-17");
            List<string> aliases = cmpd.Aliases;
            Assert.AreEqual(cmpd.SampleID, "NCGC00373219:01");

        }
    }
}

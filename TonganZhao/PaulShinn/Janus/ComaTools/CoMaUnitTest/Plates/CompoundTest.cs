using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlateLibrary.Compounds;

namespace CoMaUnitTest
{
    [TestClass]
    public class CompoundTest
    {
        [TestMethod]
        public void Test_Compound_Properties()
        {
            string sampleID = "NCGC12345678-90";
            string ncgcRoot = "NCGC12345678";
            string batch = "90";
            Compound cmpd = new Compound(sampleID);
            Assert.AreEqual(sampleID, cmpd.SampleID);
            Assert.AreEqual(ncgcRoot, cmpd.NCGCRoot);
            Assert.AreEqual(batch, cmpd.BatchNumber);
        }

        [TestMethod]
        public void Test_Compouds_Are_Equal_SampleID()
        {
            string sampleID = "NCGC12345678-90";
            Compound cmpd1 = new Compound(sampleID);
            Compound cmpd2 = new Compound(sampleID);
            Assert.AreEqual(cmpd1, cmpd2);
        }

        [TestMethod]
        public void Test_Compouds_Are_Equal_SampleID_Conc()
        {
            string conc = "10mM";
            string sampleID = "NCGC12345678-90";
            Compound cmpd1 = new Compound(sampleID, conc);
            Compound cmpd2 = new Compound(sampleID, conc);
            Assert.AreEqual(cmpd1, cmpd2);
        }
    }
}

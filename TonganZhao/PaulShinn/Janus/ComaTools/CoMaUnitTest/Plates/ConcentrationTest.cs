using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlateLibrary.Compounds;

namespace CoMaUnitTest.Plates
{
    [TestClass]
    public class ConcentrationTest
    {
        [TestMethod]
        public void Test_Concentration_Properties()
        {
            double value = 10;
            Concentration conc = new Concentration(value);
            Assert.AreEqual(value, conc.Value);
        }

        [TestMethod]
        public void Test_Concentration_Constructor()
        {
            string value = "10";
            Concentration conc = new Concentration(value);
            Assert.AreEqual(Convert.ToDouble(value), conc.Value);
        }

        [TestMethod]
        public void Test_Concentration_Constructor_Decimal()
        {
            string value = "4.47";
            Concentration conc = new Concentration(value);
            Assert.AreEqual(Convert.ToDouble(value), conc.Value);
        }

        [TestMethod]
        public void Test_Concentrations_Are_Equal()
        {
            string conc = "10mM";
            Concentration conc1 = new Concentration(conc);
            Concentration conc2 = new Concentration(conc);
            Assert.AreEqual(conc1, conc2);
        }
    }
}

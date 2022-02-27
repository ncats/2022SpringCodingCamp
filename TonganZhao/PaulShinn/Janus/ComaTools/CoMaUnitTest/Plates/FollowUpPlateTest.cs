using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FOTSLibrary;
using PlateLibrary;

namespace CoMaUnitTest.Plates
{
    [TestClass]
    public class FollowUpPlateTest
    {
        [TestMethod]
        public void Dilute_Concentration_Should_Equal_500()
        {
            double concentration = FollowUpPlate.DiluteConcentration(1000, 2, 1);
            Assert.AreEqual(concentration, 500);
        }

        [TestMethod]
        public void Dilute_Concentration_Should_Equal_1000()
        {
            double concentration = FollowUpPlate.DiluteConcentration(1000, 2, 0);
            Assert.AreEqual(concentration, 1000);
        }

        [TestMethod]
        public void Next_Well_Horizontal_Should_Equal_A07()
        {
            PlateLibrary.Orientation orientation = PlateLibrary.Orientation.Horizontal;
            PlateLibrary.Plate.Well startingWell = new PlateLibrary.Plate.Well(1, 5); //A05
            int dilution = 1;

            Plate.Well newWell = FollowUpPlate.GetNextWell(1536, orientation, startingWell, dilution);

            Assert.AreEqual(newWell.AlphanumericWell, "A07");
        }

        [TestMethod]
        public void Next_Well_Vertical_Should_Equal_C05()
        {
            PlateLibrary.Orientation orientation = PlateLibrary.Orientation.Vertical;
            PlateLibrary.Plate.Well startingWell = new PlateLibrary.Plate.Well(1, 5); //A05
            int dilution = 1;

            Plate.Well newWell = FollowUpPlate.GetNextWell(1536, orientation, startingWell, dilution);

            Assert.AreEqual(newWell.AlphanumericWell, "C05");
        }
    }
}

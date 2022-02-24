using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RNAiLibrary;
using System.Collections.Generic;

namespace CoMaUnitTest.RNAi
{
    [TestClass]
    public class PickListTest
    {
        [TestMethod]
        public void SortTransfer()
        {
            //List<Transfer> transfers = new List<Transfer>()
            //{
            //    new Transfer("Source1", "A01", 1, "B01", 10),
            //    new Transfer("")
            //
            //}
        }

        [TestMethod]
        public void Add_Destination_Well_Destination_Plate_Should_Be_1()
        {
            Transfer transfer = new Transfer() { SourcePlate = "Source1", SourceWell = new PlateLibrary.Plate.Well("A01") };
            RNAiPlate.Well well = new RNAiPlate.Well("A01");
            List<RNAiPlate.Well> wells = new List<RNAiPlate.Well>() { well };
            PickListFile.AssignDestinationWell(transfer, 1, ref wells);
            Assert.AreEqual(well.AlphanumericWell, transfer.DestWell.AlphanumericWell);
            Assert.AreEqual(1, transfer.DestPlate);
        }
    }
}

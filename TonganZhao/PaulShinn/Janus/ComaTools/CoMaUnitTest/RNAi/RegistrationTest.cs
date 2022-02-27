using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RNAiLibrary;
using System.Collections.Generic;

namespace CoMaUnitTest.RNAi
{
    [TestClass]
    public class RegistrationTest
    {
        public List<RNAiPlate> _plates = new List<RNAiPlate>()
        {
            new RNAiPlate("")
            {
                PlateNumber = 1,
            }
        };

        public List<Transfer> _transfers = new List<Transfer>()
        {
            new Transfer("CPE0022O", "C15", 1, "A03", 10),
            new Transfer("CPE0022O", "C17", 2, "A04", 10)
        };

        public List<string> _barcodes = new List<string>() { "Barcode1", "Barcode2" };


        [TestMethod]
        public void Destination_Barcodes_Property_Should_Equal_2()
        {
            RegistrationFile file = new RegistrationFile(_transfers);
            file.RegisterTransfers(_barcodes);

            Assert.AreEqual(_barcodes.Count, file.DestinationBarcodes.Count);
        }

        /// <summary>
        /// This verifies the plate barcode property is being updated with the list of 
        /// barcodes passed in as a parameter
        /// </summary>
        [TestMethod]
        public void Plate_Barcodes_Should_Match_New_Barcodes()
        {
            RegistrationFile file = new RegistrationFile(_transfers);
            file.RegisterTransfers(_barcodes);

            for (int i = 0 ; i < _barcodes.Count; i++)
            {
                Assert.AreEqual(_barcodes[i], file.Plates[i].Barcode);
            }
        }

        [TestMethod]
        public void Get_RNAi_Should_Throw_Exception_For_No_RNAi()
        {
            Transfer transfer = new Transfer("CPE0022O", "C15", 1, "A03", 10);
            
            try
            {
                RNAiLibrary.RNAi rnai = transfer.GetRNAi();
                Assert.Fail("RNAi should be null");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        [TestMethod]
        public void Plate_Should_Have_Two_RNAi_in_Same_DestWell()
        {
            List<Transfer> transfers = new List<Transfer>()
            {
                 new Transfer("CPE0022O", "C15", 1, "A03", 10),
                 new Transfer("CPE0022O", "C17", 1, "A03", 10)
            };
            List<string> barcodes = new List<string>() { "Barcode1" };

            RegistrationFile file = new RegistrationFile(transfers);
            file.RegisterTransfers(barcodes);

            RNAiPlate.Well well = file.Plates[0].GetWell("A03");
            Assert.AreEqual(2, well.RNAi.Count);
        }

        //[TestMethod]
        //public void Get_Well_()
        //{
        //    List<RNAiPlate> plates = new List<RNAiPlate>();
        //    plates.Add(new RNAiPlate("")
        //    {
        //        PlateNumber = 1
        //    });
        //    plates.Add(new RNAiPlate("")
        //    {
        //        PlateNumber = 2
        //    });

        //    RegistrationFile file = new RegistrationFile(_transfers);
        //    file.RegisterTransfers(_barcodes);
        //    file.PopulateWells(_plates, _transfers);
        //    Assert.AreEqual()
        //}


        public void Get_RNAi_Should_Return_Sample_ID()
        {
            RNAiPlate.Well well = new RNAiPlate.Well("C15");
            RNAiLibrary.RNAi rnai = RNAiLibrary.RNAi.Get(well, "COE00220");
            Assert.AreEqual(rnai.SampleID, "ASO07QQK");
        }
        

        [TestMethod]
        public void Validate_Registration_File_Should_Fail()
        {
            List<Transfer> transfers = new List<Transfer>()
            {
                new Transfer("B1234789", "A01", 1, "A02", 10)
            };
            
            try
            {
                RegistrationFile file = new RegistrationFile(transfers);
                file.RegisterTransfers(_barcodes);
                Assert.Fail("Barcodes do not match transfer list");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}

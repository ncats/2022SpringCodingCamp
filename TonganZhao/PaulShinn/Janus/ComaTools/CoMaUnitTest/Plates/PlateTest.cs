using System;
using PlateLibrary;
using PlateLibrary.Compounds;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CoMaUnitTest
{
    [TestClass]
    public class PlateTest
    {
        [TestMethod]
        public void Initialize_Compound()
        {
            Compound compound = new Compound("NCGC01234567-89", 10.4);
            Assert.AreEqual(compound.SampleID, "NCGC01234567-89");
            Assert.AreEqual(compound.NCGCRoot, "NCGC01234567");
            Assert.AreEqual(compound.BatchNumber, "89");
            Assert.AreEqual(compound.Concentration.Value, 10.4);
        }
        
        public void Initialize_Plate_Types(int plateType, int rows, int columns)
        {
            Plate plate = new Plate(plateType);
            Assert.AreEqual(plateType, plate.PlateType);
            Assert.AreEqual(8, plate.NumberOfRows);
            Assert.AreEqual(12, plate.NumberOfColumns);
        }

        [TestMethod]
        public void Initialize_Plate_96()
        {
            Plate plate = new Plate(96);
            Assert.AreEqual(96, plate.PlateType);
            Assert.AreEqual(8, plate.NumberOfRows);
            Assert.AreEqual(12, plate.NumberOfColumns);
        }

        [TestMethod]
        public void Initialize_Plate_384()
        {
            Plate plate = new Plate(384);
            Assert.AreEqual(384, plate.PlateType);
            Assert.AreEqual(16, plate.NumberOfRows);
            Assert.AreEqual(24, plate.NumberOfColumns);
        }

        [TestMethod]
        public void Initialize_Plate_1536()
        {
            Plate plate = new Plate(1536);
            Assert.AreEqual(1536, plate.PlateType);
            Assert.AreEqual(32, plate.NumberOfRows);
            Assert.AreEqual(48, plate.NumberOfColumns);
        }

        [TestMethod]
        public void Test_Compound_Property()
        {
            Plate plate = new Plate(1536);
            plate.AddCompound(1, 1, new Compound("NCGC11111111-12"));
            plate.AddCompound(1, 2, new Compound("NCGC11111111-13"));

            Assert.AreEqual(2, plate.Compounds.Count);
        }

        [TestMethod]
        public void Convert_Barcode_Throw_Invalid_Operation_Exception()
        {
            string barcode = "B123B123";
            try
            {
                string newBarcode = Plate.ConvertToOddBarcode(barcode);
                Assert.Fail("Did not throw exception");
            }
            catch (InvalidOperationException ex)
            {
                Console.Write(ex.Message);
            }
        }

        [TestMethod]
        public void Convert_To_Odd_Barcode_From_Even_Barcode()
        {
            string barcode = "B0123456";
            string newBarcode = Plate.ConvertToOddBarcode(barcode);
            Console.Write(newBarcode);
            Assert.AreEqual(newBarcode, "B0123455");
        }
        
        #region Set Current Well
        [TestCategory("Set Current Well")]
        [TestMethod]
        public void Set_Current_Well_By_Row_Column()
        {
            List<int> plateTypes = new List<int>() { 96, 384, 1536 };
            
            foreach (int plateType in plateTypes)
            {
                Plate plate = new Plate(plateType);
                foreach (int row in plate.Rows)
                {
                    foreach (int col in plate.Columns)
                    {
                        Plate.Well well = plate.GetWell(row, col);
                        plate.SetCurrentWell(well.Row, well.Column);
                        Assert.AreEqual(well, plate.CurrentWell);                        
                    }
                }
            }           
        }

        [TestCategory("Set Current Well")]
        [TestMethod]
        public void Set_Current_Well_By_Alphanumeric_Well()
        {
            List<int> plateTypes = new List<int>() { 96, 384, 1536 };

            foreach (int plateType in plateTypes)
            {
                Plate plate = new Plate(plateType);
                foreach (int row in plate.Rows)
                {
                    foreach (int col in plate.Columns)
                    {
                        Plate.Well well = plate.GetWell(row, col);
                        plate.SetCurrentWell(well.AlphanumericWell);
                        Assert.AreEqual(well, plate.CurrentWell);
                    }
                }
            }
        }

        [TestCategory("Set Current Well")]
        [TestMethod]
        public void Set_Current_Well_By_Well()
        {
            List<int> plateTypes = new List<int>() { 96, 384, 1536 };

            foreach (int plateType in plateTypes)
            {
                Plate plate = new Plate(plateType);
                foreach (int row in plate.Rows)
                {
                    foreach (int col in plate.Columns)
                    {
                        Plate.Well well = new Plate.Well(row, col);
                        plate.SetCurrentWell(well);
                        Assert.AreEqual(well.AlphanumericWell, plate.CurrentWell.AlphanumericWell);
                    }
                }
            }
        }
        #endregion
        
        [TestMethod]
        public void Map_New_Well()
        {
            List<int> plateTypes = new List<int>() { 96, 384, 1536 };

            
        }

        [TestClass]
        public class Well
        {
            #region Add Compound
            [TestMethod]
            public void Test_Method_Add_Compound_Sample_ID()
            {
                Plate.Well well = new Plate.Well("A01");
                string sampleID = "NCGC12345678-90";
                well.AddCompound(sampleID);
                Assert.AreEqual(well.Compounds[0].SampleID, sampleID);
            }

            [TestMethod]
            public void Test_Method_Add_Compound_Sample_ID_Barcode()
            {
                Plate.Well well = new Plate.Well("A01");
                string sampleID = "NCGC12345678-90";
                string barcode = "1234546789";
                well.AddCompound(sampleID, barcode);
                Assert.AreEqual(well.Compounds[0].SampleID, sampleID);
                Assert.AreEqual(well.Compounds[0].Barcode, barcode);
            }

            [TestMethod]
            public void Test_Method_Add_Compound_Sample_ID_Barcode_Conc()
            {
                Plate.Well well = new Plate.Well("A01");
                string sampleID = "NCGC12345678-90";
                string barcode = "1234546789";
                string conc = "10 mM";
                well.AddCompound(sampleID, barcode, conc);
                Assert.AreEqual(well.Compounds[0].SampleID, sampleID);
                Assert.AreEqual(well.Compounds[0].Barcode, barcode);
                Assert.AreEqual(well.Compounds[0].Concentration.ToString(), conc);
            }

            [TestMethod]
            public void Test_Method_Add_Compound()
            {
                Plate.Well well = new Plate.Well("A01");
                Compound compound = new Compound("NCGC12345678-90");
                well.AddCompound(compound);
                Assert.AreEqual(well.Compounds[0], compound);
            }
            #endregion

            [TestMethod]
            public void To_Column_String_1_0()
            {
                Plate.Well well = new Plate.Well("A1");
                string colString = well.ToColumnString(2);
                Assert.AreEqual("A01", colString);
            }

            [TestMethod]
            public void To_Column_String()
            {
                Plate.Well well = new Plate.Well("A01");
                string colString = well.ToColumnString(0);
                Assert.AreEqual("A1", colString);
            }

            [TestMethod]
            public void Test_Property_Is_Well_Empty_Return_False()
            {
                Plate.Well well = new Plate.Well("A01");
                well.AddCompound(new Compound("NCGC11111111-12"));
                Assert.IsFalse(well.IsEmpty);
            }

            [TestMethod]
            public void Test_Property_Is_Well_Empty_Return_True()
            {
                Plate.Well well = new Plate.Well("A01");
                Assert.IsTrue(well.IsEmpty);
            }

            [TestMethod]
            public void Test_Property_Contains_DMSO_Return_True()
            {
                Plate.Well well = new Plate.Well("A01");
                well.AddCompound(new Compound("DMSO"));
                Assert.IsTrue(well.ContainsDMSO);
            }

            [TestMethod]
            public void Test_Property_Contains_DMSO_Return_False()
            {
                Plate.Well well = new Plate.Well("A01");
                Assert.IsFalse(well.ContainsDMSO);
            }

            
        }
    }
}

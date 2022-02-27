using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlateLibrary;

namespace CoMaUnitTest.Plates
{
    [TestClass]
    public class StampPlateTest
    {
        #region Mapping Factor
        [TestMethod]
        public void Get_Mapping_Factor_96_To_96_Should_Equal_1()
        {
            double mappingFactor = StampPlate.GetMappingFactor(96, 96);
            Assert.AreEqual(1, mappingFactor);
        }

        [TestMethod]
        public void Get_Mapping_Factor_96_To_384_Should_Equal_2()
        {
            double mappingFactor = StampPlate.GetMappingFactor(96, 384);
            Assert.AreEqual(2, mappingFactor);
        }

        [TestMethod]
        public void Get_Mapping_Factor_384_To_96_Should_Equal_Half()
        {
            double mappingFactor = StampPlate.GetMappingFactor(384, 96);
            Assert.AreEqual(.5, mappingFactor);
        }

        [TestMethod]
        public void Get_Mapping_Factor_1536_To_96_Should_Equal_Quarter()
        {
            double mappingFactor = StampPlate.GetMappingFactor(1536, 96);
            Assert.AreEqual(.25, mappingFactor);
        }

        [TestMethod]
        public void Get_Mapping_Factor_384_To_384_Should_Equal_1()
        {
            double mappingFactor = StampPlate.GetMappingFactor(384, 384);
            Assert.AreEqual(1, mappingFactor);
        }

        [TestMethod]
        public void Get_Mapping_Factor_384_To_1536_Should_Equal_2()
        {
            double mappingFactor = StampPlate.GetMappingFactor(384, 1536);
            Assert.AreEqual(2, mappingFactor);
        }
        #endregion

        #region MapNewColumn
        [TestMethod]
        public void Map_Column_Q1_384_To_384_Should_Be_Same_Column()
        {
            int newColumn = StampPlate.MapNewColumn(Plate.Quadrant.Q1, 1, 384, 384);
            Assert.AreEqual(1, newColumn);
        }
        [TestMethod]
        public void Map_Column_1_Q1_384_To_1536_Should_Be_Same_Column()
        {
            int newColumn = StampPlate.MapNewColumn(Plate.Quadrant.Q1, 1, 384, 1536);
            Assert.AreEqual(1, newColumn);
        }

        [TestMethod]
        public void Map_Column_1_Q2_384_To_1536_Should_Be_Column_2()
        {
            int newColumn = StampPlate.MapNewColumn(Plate.Quadrant.Q2, 1, 384, 1536);
            Assert.AreEqual(2, newColumn);
        }

        [TestMethod]
        public void Map_Column_1_Q3_384_To_1536_Should_Be_Column_1()
        {
            int newColumn = StampPlate.MapNewColumn(Plate.Quadrant.Q3, 1, 384, 1536);
            Assert.AreEqual(1, newColumn);
        }

        [TestMethod]
        public void Map_Column_1_Q4_384_To_1536_Should_Be_Column_2()
        {
            int newColumn = StampPlate.MapNewColumn(Plate.Quadrant.Q4, 1, 384, 1536);
            Assert.AreEqual(2, newColumn);
        }
        #endregion
    }
}

using System;
using FOTSLibrary.Users;
using FOTSLibrary.Projects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlateLibrary.Compounds;
using System.Collections.Generic;
using System.Data;

namespace CoMaUnitTest.Database
{
    [TestClass]
    public class MatrixTubeTestClass
    {
        [TestMethod]
        public MatrixTube CreateMatrixTubeObject()
        {
            string barcode = "0202579917";
            string sampleID = "TRND00482475-01";
            string conc = "10mM";

            Project project = Project.Get("Testing");
            User user = User.GetUserByUserID("mierzwatj");
            MatrixTube tbe = new MatrixTube(barcode, sampleID, conc, project, user, DateTime.Now, "1", 1, 16);

            Assert.AreEqual(tbe.Barcode, barcode);
            Assert.AreEqual(tbe.Compound.SampleID, sampleID);
            Assert.AreEqual(conc, tbe.Compound.Concentration.ToString());
            Assert.AreEqual(tbe.Chemist.FullName, user.FullName);
            Assert.AreEqual(tbe.Project.Name, project.Name);

            return tbe;
        }

        [TestMethod]
        public void Get_Matrix_Tube_By_Barcode()
        {
            string barcode = "0202579917";
            MatrixTube tube = MatrixTube.Get(barcode);
            Assert.AreEqual(barcode, tube.Barcode);
        }

        [TestMethod]
        public void Get_Matrix_Tube_By_Barcode_Is_Null()
        {
            string barcode = "9999";
            MatrixTube tube = MatrixTube.Get(barcode);
            Assert.AreEqual(null, tube);
        }

        [TestMethod]
        public void Get_Matrix_Tube_By_Vial_Number()
        {
            int vialNum = 373214;
            MatrixTube tube = MatrixTube.Get(vialNum);
            Assert.AreEqual(vialNum, tube.VialNumber);
        }

        [TestMethod]
        public void Get_Matrix_Tube_By_Vial_Number_Is_Null()
        {
            int vialNum = -1;
            MatrixTube tube = MatrixTube.Get(vialNum);
            Assert.AreEqual(null, tube);
        }

        [TestMethod]
        public void Add_Matrix_Tube_No_Smart_Table_No_ADME()
        {  
            try
            {
                MatrixTube tube = CreateMatrixTubeObject();
                tube.ProcessTube(false, false);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void Add_Matrix_Tube_Smart_Table_No_ADME()
        {
            try
            {
                MatrixTube tube = CreateMatrixTubeObject();
                tube.ProcessTube(true, false);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void Add_Matrix_Tube_Smart_Table_ADME()
        {
            try
            {
                MatrixTube tube = CreateMatrixTubeObject();
                tube.ProcessTube(true, true);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        //#region Query Volume admin page
        //[TestMethod]
        //public void Get_Matrix_Tube_Volumes_By_NCGCID()
        //{
        //    List<string> compounds = new List<string>() { "TRND00482475-01" };
        //    DataTable dt = FOTSLibrary.ProbeDB.GetBarcodeDetails(compounds, "NCGC ID", false);
        //    Assert.IsTrue(dt.Rows.Count > 0);
        //}

        //[TestMethod]
        //public void Get_Matrix_Tube_Volumes_By_NCGCID_Any_Batch()
        //{
        //    List<string> compounds = new List<string>() { "NCGC00179686" };
        //    DataTable dt = FOTSLibrary.ProbeDB.GetBarcodeDetails(compounds, "NCGC ID", true);
        //    Console.WriteLine("Rows: {0}", dt.Rows.Count);
        //    Assert.IsTrue(dt.Rows.Count > 1);
        //}

        //[TestMethod]
        //public void Get_Matrix_Tube_Volumes_By_Barcode()
        //{
        //    List<string> barcodes = new List<string>() { "0088246817" };
        //    DataTable dt = FOTSLibrary.ProbeDB.GetBarcodeDetails(barcodes, "Barcode", true);
        //    Console.WriteLine("Rows: {0}", dt.Rows.Count);
        //    Assert.IsTrue(dt.Rows.Count > 0);
        //}

        //[TestMethod]
        //public void Get_Matrix_Tube_Volumes_Multiple_Barcodes()
        //{
        //    List<string> barcodes = new List<string>() { "0088246817", "0128121322" };
        //    DataTable dt = FOTSLibrary.ProbeDB.GetBarcodeDetails(barcodes, "Barcode", true);
        //    Console.WriteLine("Rows: {0}", dt.Rows.Count);
        //    Assert.IsTrue(dt.Rows.Count > 1);
        //}

        //[TestMethod]
        //public void Get_Matrix_Tube_Volumes_By_Project()
        //{
        //    List<string> barcodes = new List<string>() { "12-hLO" };
        //    DataTable dt = FOTSLibrary.ProbeDB.GetBarcodeDetails(barcodes, "Project", true);
        //    Console.WriteLine("Rows: {0}", dt.Rows.Count);
        //    Assert.IsTrue(dt.Rows.Count > 1);
        //}
    }
}

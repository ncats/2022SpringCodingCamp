using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VialInventoryLibrary;
using FOTSLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using PlateLibrary.Compounds;

namespace CoMaUnitTest.Database
{
    [TestClass]
    public class WebPagesTest
    {
        [TestMethod]
        public void TestWebPagesURLsFromDB()
        {
            List<string> webPages = new List<string>()
            {
                "Barcode Query",
                "Inventory Browser",
                "SMARTBROWSER Home Page",
                "SMARTCART New Order",
                "SMARTCART My Orders",
                "SMARTCART Admin Page"
            };
            
            List<string> failedPages = GetMissingPages(webPages);

            if (failedPages.Count > 0)
                Assert.Fail("Missing URLs: " + failedPages.Aggregate((x, y) => x + ',' + y));
        }

        [TestMethod]
        public void Test_Page_URLs_FromDB_Fail()
        {
            List<string> webPages = new List<string>()
            {
                "Barcode Query",
                "Inventory Browser",
                "SMARTBROWSER Home Page",
                "SMARTCART New Order",
                "SMARTCART My Orders",
                "SMARTCART Admin Page",
                "Test Fail"
            };

            List<string> failedPages = GetMissingPages(webPages);

            Assert.AreEqual(failedPages.Count, 1);
        }

        private List<string> GetMissingPages(List<string> pages)
        {
            List<string> failedPages = new List<string>();

            foreach (string page in pages)
            {
                string ret = ProbeDB.GetURL(page);
                if (ret == "")
                    failedPages.Add(page);
            }

            return failedPages;
        }

        [TestMethod]
        public void Get_Abase_Registered_Compounds()
        {
            DataTable dt = AbaseCompound.GetRegisteredCompounds(30);
            Console.Write("Rows: {0}", dt.Rows);
            Assert.AreNotEqual(null, dt);            
        }

        [TestMethod]
        public void Get_Reg_DB_Registered_Compounds()
        {
            DataTable dt = AbaseCompound.GetRegDBRegisteredCompounds(30);
            Console.Write("Rows: {0}", dt.Rows);
            Assert.AreNotEqual(null, dt);
        }

        #region SMILES
        [TestMethod]
        public void Get_SMILES_By_Sample_ID()
        {
            List<Compound> compounds = new List<Compound>()
            {
                new Compound("NCGC00142571-01"),
                new Compound("NCGC00059191-03")
            };
            DataTable dt = ProbeDB.GetSmiles(compounds, false, "Sample_ID");
            Assert.IsTrue(dt.Rows.Count > 0);
        }

        [TestMethod]
        public void Get_SMILES_By_Sample_ID_Any_Batch()
        {
            List<Compound> compounds = new List<Compound>()
            {
                new Compound("NCGC00142571-01"),
                new Compound("NCGC00059191-03")
            };
            DataTable dt = ProbeDB.GetSmiles(compounds, true, "Sample_ID");
            Assert.IsTrue(dt.Rows.Count > 0);
        }

        [TestMethod]
        public void Get_SMILES_By_Vendor()
        {
            List<Compound> compounds = new List<Compound>()
            {
                new Compound("Timtec")
            };
            DataTable dt = ProbeDB.GetSmiles(compounds, false, "Supplier");
            Assert.IsTrue(dt.Rows.Count > 0);
        }

        [TestMethod]
        public void Get_SMILES_By_Catalog()
        {
            List<Compound> compounds = new List<Compound>()
            {
                new Compound("DKL10-050")
            };
            DataTable dt = ProbeDB.GetSmiles(compounds, false, "Supplier_ID");
            Assert.IsTrue(dt.Rows.Count > 0);
        }
        #endregion

        #region Similar Compounds
        [TestMethod]
        public void Get_Similar_Compounds()
        {
            List<Compound> compounds = new List<Compound>()
            {
                new Compound("NCGC00142571-01"),
                new Compound("NCGC00059191-03")
            };
            DataTable dt = ProbeDB.GetSimilarCompounds(compounds, "NCGC ID", .9);
            Assert.IsTrue(dt.Rows.Count > 0);
        }
        #endregion
    }
}

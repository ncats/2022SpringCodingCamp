using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompoundStore;
using CompoundStore.Exceptions;

namespace CoMaUnitTest.Database
{
    //[TestClass]
    //public class StoreOrder
    //{
    //    [TestMethod]
    //    public void Store_Order_Throw_Duplicate_Barcode_Exception()
    //    {
    //        List<string> barcodes = new List<string>() { "123456789", "123456789" };
    //        CompoundStore.StoreOrder order = new CompoundStore.StoreOrder(barcodes, "user", "orderDescription", 50, "pattern", false, false, false);
    //        try
    //        {
    //            order.SubmitOrder();
    //            Assert.Fail("Order was placed");
    //        }
    //        catch (DuplicateBarcodeException ex)
    //        {
    //            Assert.AreEqual(ex.DuplicateBarcodes[0], "123456789");
    //        }
    //    }

    //    [TestMethod]
    //    public void Store_Order_Throw_Never_In_Store_Exception()
    //    {
    //        List<string> barcodes = new List<string>() { "123456789" };
    //        CompoundStore.StoreOrder order = new CompoundStore.StoreOrder(barcodes, "user", "orderDescription", 50, "pattern", false, false, false);
    //        try
    //        {
    //            order.SubmitOrder();
    //            Assert.Fail("Order was placed");
    //        }
    //        catch (NeverInStoreException ex)
    //        {
    //            Console.Write(ex.Tubes);
    //            Assert.AreEqual(ex.Tubes[0].Barcode, "123456789");
    //        }
    //    }

    //    /// <summary>
    //    /// The order should be deferred if it has greater than 96 tubes/vials
    //    /// </summary>
    //    [TestMethod]
    //    public void Store_Order_Defer_Order()
    //    {
    //        List<string> barcodes = new List<string>();
    //        bool orderDeferred = false;
    //        for (int i = 100; i < 200; i++)
    //        {
    //            barcodes.Add("123456" + i.ToString());
    //        }
    //        CompoundStore.StoreOrder order = new CompoundStore.StoreOrder(barcodes, "user", "orderDescription", 50, "pattern", orderDeferred, false, false);
    //        Assert.AreEqual(order.OrderDeferred, true);
    //    }

    //    public void Store_Order_Without_FOTS_Order_ID()
    //    {
    //        List<string> barcodes = new List<string>() { "123456789" };
    //        string orderDescription = "Project ADME";
    //        CompoundStore.StoreOrder order = new CompoundStore.StoreOrder(barcodes, "user", orderDescription, 50, "pattern", false, false, false);
    //        Assert.IsTrue(string.IsNullOrEmpty(order.FOTSOrderID));
    //    }

    //    public void Store_Order_With_FOTS_Order_ID()
    //    {
    //        List<string> barcodes = new List<string>() { "123456789" };
    //        string orderDescription = "123456789123 ADME";
    //        CompoundStore.StoreOrder order = new CompoundStore.StoreOrder(barcodes, "user", orderDescription, 50, "pattern", false, false, false);
    //        Assert.AreEqual(order.FOTSOrderID, "123456789123");
    //    }
    

    //}
}

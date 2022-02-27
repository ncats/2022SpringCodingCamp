using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompoundStore;
using System.Collections.Generic;

namespace CoMaUnitTest.Database
{
    [TestClass]
    public class CompoundStoreTest
    {
        [TestMethod]
        public void Get_Store_Temperature_Test()
        {
            string temp = CompoundStore.Properties.Temperature;
            Console.WriteLine("Store Temperature: {0}", temp);
            Assert.AreNotEqual("", temp);
        }

        [TestMethod]
        public void Get_Store_Humidity_Test()
        {
            string humidity = CompoundStore.Properties.Humidity;
            Console.WriteLine("Humidity: {0}", humidity);
            Assert.AreNotEqual("", humidity);
        }

        [TestMethod]
        public void GetStoreOrderTest()
        {
            string orderId = "24996";
            //CompoundStore.StoreOrder storeOrder = CompoundStore.StoreOrder.Get(orderId);
            //Assert.IsTrue(storeOrder.PlateMap.Count == 3);
        }

        [TestMethod]
        public void GetTubePlateMapTest()
        {
            string orderId = "24710";
            //CompoundStore.StoreOrder storeOrder = CompoundStore.StoreOrder.Get(orderId);
            //Assert.IsTrue(storeOrder.PlateMap.Count == 8);
        }

        [TestMethod]
        public void GetVialPlateMapTest()
        {
            //string orderId = "24699";
            //StoreOrder storeOrder = StoreOrder.GetStoreOrder(orderId);
            //Assert.IsTrue(storeOrder.PlateMap.Count == 2);
        }

        /// <summary>
        /// Places a store order strictly with barcodes
        /// </summary>
      
        //public void PlaceStoreOrderBarcodesTest()
        //{
        //    List<StoreOrder.OrderItem> orderItems = new List<StoreOrder.OrderItem>();
        //    orderItems.Add(new StoreOrder.OrderItem("0107227373"));
        //    orderItems.Add(new StoreOrder.OrderItem("0107227386"));
        //    orderItems.Add(new StoreOrder.OrderItem("0107226910"));

        //    PlaceStoreOrder(orderItems);
        //}

        //private void PlaceStoreOrder(List<StoreOrder.OrderItem> orderItems)
        //{
        //    string customer = "Unit Test";
        //    string orderDescription = "Unit Test Description";
        //    int orderPriority = 50;
        //    bool deferred = true;
        //    bool pendIfUnavailable = true;
        //    string tubePattern = "";

        //    //StoreOrder order = new StoreOrder(orderItems, customer, orderDescription, orderPriority, tubePattern, deferred, pendIfUnavailable, false);
        //    //order.SubmitOrder();
        //}

        /// <summary>
        /// Places a store order with a predefined plate map
        /// </summary>
        //[TestMethod]
        //public void PlaceStoreOrderPlateTest()
        //{
        //    List<StoreOrder.OrderItem> orderItems = new List<StoreOrder.OrderItem>();
        //    orderItems.Add(new StoreOrder.OrderItem("0059906317", "1", "A04"));
        //    orderItems.Add(new StoreOrder.OrderItem("0059906319", "1", "D12"));
        //    orderItems.Add(new StoreOrder.OrderItem("0059906325", "1", "B12"));
        //    orderItems.Add(new StoreOrder.OrderItem("0059906326", "1", "D04"));
        //    orderItems.Add(new StoreOrder.OrderItem("0059906337", "1", "C02"));
        //    orderItems.Add(new StoreOrder.OrderItem("0059906341", "1", "D11"));
        //    orderItems.Add(new StoreOrder.OrderItem("0059906344", "1", "C12"));
        //    orderItems.Add(new StoreOrder.OrderItem("0059906345", "1", "H01"));
        //    orderItems.Add(new StoreOrder.OrderItem("0059906350", "1", "H08"));
        //    orderItems.Add(new StoreOrder.OrderItem("0059906351", "1", "D10"));
        //    orderItems.Add(new StoreOrder.OrderItem("0059906352", "1", "H12"));
        //    orderItems.Add(new StoreOrder.OrderItem("0059906364", "1", "G10"));
        //    orderItems.Add(new StoreOrder.OrderItem("0059906365", "1", "E04"));
        //    orderItems.Add(new StoreOrder.OrderItem("0059906367", "1", "G12"));
        //    orderItems.Add(new StoreOrder.OrderItem("0059906368", "1", "E12"));
        //    orderItems.Add(new StoreOrder.OrderItem("0059906369", "1", "B04"));
        //    orderItems.Add(new StoreOrder.OrderItem("0059906372", "1", "C04"));
        //    orderItems.Add(new StoreOrder.OrderItem("0059906373", "1", "F12"));
        //    orderItems.Add(new StoreOrder.OrderItem("0059906376", "1", "D02"));
        //    orderItems.Add(new StoreOrder.OrderItem("0059906391", "1", "E03"));
        //    orderItems.Add(new StoreOrder.OrderItem("0064451539", "1", "A10"));
        //    orderItems.Add(new StoreOrder.OrderItem("0059906397", "1", "B10"));
        //    orderItems.Add(new StoreOrder.OrderItem("0059906398", "1", "F04"));
        //    orderItems.Add(new StoreOrder.OrderItem("0064451995", "1", "G01"));
        //    orderItems.Add(new StoreOrder.OrderItem("0107231465", "1", "F10"));
        //    orderItems.Add(new StoreOrder.OrderItem("0107231483", "1", "G04"));
        //    orderItems.Add(new StoreOrder.OrderItem("0107231506", "1", "H10"));
        //    orderItems.Add(new StoreOrder.OrderItem("0107231518", "1", "A12"));
        //    orderItems.Add(new StoreOrder.OrderItem("0107231538", "1", "D07"));
        //    orderItems.Add(new StoreOrder.OrderItem("0107231539", "1", "C10"));
        //    orderItems.Add(new StoreOrder.OrderItem("0107231540", "1", "E11"));
        //    orderItems.Add(new StoreOrder.OrderItem("0107231541", "1", "E10"));
        //    orderItems.Add(new StoreOrder.OrderItem("0107231552", "1", "H04"));
        //    orderItems.Add(new StoreOrder.OrderItem("0145164464", "1", "C01"));
        //    orderItems.Add(new StoreOrder.OrderItem("0145166689", "1", "D01"));
        //    orderItems.Add(new StoreOrder.OrderItem("0145166710", "1", "F01"));
        //    orderItems.Add(new StoreOrder.OrderItem("0145166711", "1", "E01"));
        //    orderItems.Add(new StoreOrder.OrderItem("0145166712", "1", "A01"));
        //    orderItems.Add(new StoreOrder.OrderItem("0145166713", "1", "B01"));
        //    orderItems.Add(new StoreOrder.OrderItem("0145166734", "1", "H07"));
        //    orderItems.Add(new StoreOrder.OrderItem("0145166735", "1", "F03"));
        //    orderItems.Add(new StoreOrder.OrderItem("0145166736", "1", "A06"));
        //    orderItems.Add(new StoreOrder.OrderItem("0145166737", "1", "A07"));
        //    orderItems.Add(new StoreOrder.OrderItem("0145166758", "1", "H06"));
        //    orderItems.Add(new StoreOrder.OrderItem("0145166759", "1", "G07"));
        //    orderItems.Add(new StoreOrder.OrderItem("0145166760", "1", "E07"));
        //    orderItems.Add(new StoreOrder.OrderItem("0145166761", "1", "F07"));
        //    orderItems.Add(new StoreOrder.OrderItem("0145166782", "1", "B07"));
        //    orderItems.Add(new StoreOrder.OrderItem("0145166783", "1", "A08"));
        //    orderItems.Add(new StoreOrder.OrderItem("1008626151", "1", "C07"));

        //    PlaceStoreOrder(orderItems);
        //}
    }
}

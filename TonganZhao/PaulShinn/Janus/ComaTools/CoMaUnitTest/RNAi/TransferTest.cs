using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RNAiLibrary;


namespace CoMaUnitTest.RNAi
{
    [TestClass]
    public class TransferTest
    {
        [TestMethod]
        public void Flip_Transfer_B02_ShouldEqual_023()
        {
            Transfer transfer = new Transfer("1", "B02", 1, "B02", 10);
            Transfer flippedTransfer = transfer.Flip();
            Assert.AreEqual(flippedTransfer.SourceWell.AlphanumericWell, "O23");
        }
    }
}

using System;
using System.Collections.Generic;
using FOTSLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using PlateLibrary;

namespace CoMaUnitTest
{
    //[TestClass]
    //public class JanusTest
    //{
    //    private List<int> DilutionFactors = new List<int>();

    //    private List<Plate> GetSourcePlates()
    //    {
    //        #region Sample Plate map
    //        string text = "RTS-TUBE-031\tA01\t0122688502\tNCGC00345104-01\t10mM\r\n" + 
    //        "RTS-TUBE-031\tB01\t0182367022\tNCGC00378300-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tC01\t0182271715\tNCGC00372955-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tD01\t0169604204\tNCGC00351032-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tE01\t0145184388\tNCGC00347490-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tF01\t0128121566\tNCGC00319009-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tG01\t1008629958\tNCGC00182478-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tH01\t0122688262\tNCGC00345041-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tA02\t1008629957\tNCGC00182479-01\t10  \r\n" +
    //        "RTS-TUBE-031\tB02\t1008629956\tNCGC00182480-01\t10  \r\n" +
    //        "RTS-TUBE-031\tC02\t0107231226\tNCGC00262397-01\t10  \r\n" +
    //        "RTS-TUBE-031\tD02\t0128121540\tNCGC00319005-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tE02\t0122692014\tNCGC00344941-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tF02\t0122688787\tNCGC00345029-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tG02\t0122691989\tNCGC00344903-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tH02\t0122688769\tNCGC00345042-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tA03\t0107230562\tNCGC00253487-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tB03\t0122691443\tNCGC00343571-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tC03\t0128121375\tNCGC00343896-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tD03\t0128121541\tNCGC00319004-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tE03\t0122692590\tNCGC00345208-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tF03\t0107226591\tNCGC00247845-01\t10  \r\n" +
    //        "RTS-TUBE-031\tG03\t0122719371\tNCGC00262998-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tH03\t0122691430\tNCGC00343568-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tA04\t0122718912\tNCGC00263002-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tB04\t0122718924\tNCGC00263008-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tC04\t0128121565\tNCGC00318981-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tD04\t0107226590\tNCGC00247843-01\t10  \r\n" +
    //        "RTS-TUBE-031\tE04\t0107226660\tNCGC00247844-01\t10  \r\n" +
    //        "RTS-TUBE-031\tF04\t0145183599\tNCGC00343524-02\t10mM\r\n" +
    //        "RTS-TUBE-031\tG04\t0050743265\tNCGC00112501-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tH04\t0122688639\tNCGC00345044-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tA05\t0122691771\tNCGC00345118-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tB05\t0168730844\tNCGC00356283-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tC05\t0168730773\tNCGC00356275-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tD05\t0168730800\tNCGC00356278-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tE05\t0168727339\tNCGC00356285-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tF05\t0168730775\tNCGC00356277-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tG05\t0168730842\tNCGC00356282-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tH05\t0050696386\tNCGC00101019-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tA06\t0050696450\tNCGC00101041-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tB06\t0050678629\tNCGC00104972-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tC06\t0182255582\tNCGC00381667-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tD06\t0190331657\tNCGC00238624-07\t10  \r\n" +
    //        "RTS-TUBE-031\tE06\t0050678468\tNCGC00105110-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tF06\t0155894430\tNCGC00262995-01\t12.2mM\r\n" +
    //        "RTS-TUBE-031\tG06\t0050697004\tNCGC00101317-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tH06\t0050696980\tNCGC00101321-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tA07\t0050696961\tNCGC00101323-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tB07\t0168730814\tNCGC00356288-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tC07\t0168730813\tNCGC00356287-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tD07\t0169656324\tNCGC00371473-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tE07\t0169656315\tNCGC00371474-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tF07\t0169656312\tNCGC00371470-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tG07\t0169656313\tNCGC00371472-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tH07\t0169656325\tNCGC00371471-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tA08\t0128121467\tNCGC00262393-02\t10mM\r\n" +
    //        "RTS-TUBE-031\tB08\t0128121533\tNCGC00319006-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tC08\t0122692013\tNCGC00344904-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tD08\t0122692018\tNCGC00344906-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tE08\t0122692017\tNCGC00344943-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tF08\t0050692708\tNCGC00101682-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tG08\t0050692688\tNCGC00101652-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tH08\t0050692685\tNCGC00101654-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tA09\t0050692637\tNCGC00101662-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tB09\t0050692616\tNCGC00101664-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tC09\t0050692615\tNCGC00101632-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tD09\t0050692638\tNCGC00101630-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tE09\t0050738567\tNCGC00132095-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tF09\t0050738558\tNCGC00132097-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tG09\t0050681890\tNCGC00105243-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tH09\t0050681883\tNCGC00105245-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tA10\t0050681866\tNCGC00105247-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tB10\t0050681939\tNCGC00105267-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tC10\t0050681870\tNCGC00105375-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tD10\t0050738590\tNCGC00132123-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tE10\t0050743222\tNCGC00112443-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tF10\t0050743288\tNCGC00112465-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tG10\t0050743269\tNCGC00112467-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tH10\t0050743264\tNCGC00112469-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tA11\t0050743240\tNCGC00112473-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tB11\t0050743268\tNCGC00112499-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tC11\t0155897708\tNCGC00351908-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tD11\t0155897806\tNCGC00351899-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tE11\t0155893308\tNCGC00351902-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tF11\t0050696204\tNCGC00101269-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tG11\t0155897750\tNCGC00351911-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tH11\t0155897805\tNCGC00351900-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tA12\t0155893296\tNCGC00351903-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tB12\t0050696220\tNCGC00101297-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tC12\t0155897807\tNCGC00351898-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tD12\t0050696310\tNCGC00101273-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tE12\t0050743245\tNCGC00112471-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tF12\t0050707634\tNCGC00124278-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tG12\t0050707610\tNCGC00124282-01\t10mM\r\n" +
    //        "RTS-TUBE-031\tH12\t0168730774\tNCGC00356276-01\t10mM\r\n" +
    //        "RTS-TUBE-120\tA01\t0155897751\tNCGC00351910-01\t10mM\r\n" +
    //        "RTS-TUBE-120\tB01\t0155893301\tNCGC00351914-01\t10mM\r\n" +
    //        "RTS-TUBE-120\tC01\t0050681879\tNCGC00105373-01\t10mM\r\n" +
    //        "RTS-TUBE-120\tD01\t0050742897\tNCGC00112178-01\t10mM\r\n" +
    //        "RTS-TUBE-120\tE01\t0050738582\tNCGC00132093-01\t10mM\r\n" +
    //        "RTS-TUBE-120\tF01\t0050743241\tNCGC00112505-01\t10mM\r\n" +
    //        "RTS-TUBE-120\tG01\t0155897924\tNCGC00351393-01\t10mM\r\n" +
    //        "RTS-TUBE-120\tH01\t0050707619\tNCGC00124280-01\t10mM\r\n" +
    //        "RTS-TUBE-120\tA02\t0168727348\tNCGC00356284-01\t10mM\r\n" +
    //        "RTS-TUBE-120\tB02\t0155893028\tNCGC00345106-01\t10mM\r\n" +
    //        "RTS-TUBE-120\tC02\t0122719388\tNCGC00262989-01\t10mM\r\n" +
    //        "RTS-TUBE-120\tD02\t0122719387\tNCGC00262990-01\t10mM\r\n";
    //        #endregion
    //        return JanusJob.GetSourcePlates(text);
    //    }

    //    [TestMethod]
    //    public void Get_Source_Plates_Equals_2()
    //    {
    //        List<Plate> sourcePlates = GetSourcePlates();
    //        Assert.AreEqual(sourcePlates.Count, 2);
    //    }

    //    [TestMethod]
    //    public void Test_Property_Assignments()
    //    {
    //        List<Plate> sourcePlates = GetSourcePlates();
    //        int dilutionFactor = 11;
    //        int transferVol = 10;
    //        Plate.Well startingWell = sourcePlates[0].GetWell("A03");
    //        JanusJob.TransferType transferType = JanusJob.TransferType.Dilution;
    //        string instrument = "Janus";
    //        JanusJob janus = new JanusJob(sourcePlates, transferType, instrument, dilutionFactor, transferVol, startingWell, true);

    //        Assert.AreEqual(janus.DilutionPoint, dilutionFactor);
    //        Assert.AreEqual(janus.TypeOfTransfer, transferType);
    //        Assert.AreEqual(janus.TransferVolume, transferVol);
    //        Assert.AreEqual(janus.Instrument, instrument);
    //        Assert.AreEqual(janus.StartingWell, startingWell);
    //        Assert.AreEqual(janus.SourcePlates.Count, sourcePlates.Count);
    //    }

    //    private void Test_Janus_Layout(JanusJob janus)
    //    {
    //        if (janus.DestinationPlates.Count == 0)
    //            throw new Exception("No destination plates");

    //        for (int col = janus.StartingWell.Column; col < janus.DestinationPlates[0].Columns; col += janus.DilutionPoint)
    //        {
    //            for (int row = janus.StartingWell.Row; row < janus.DestinationPlates[0].Rows; row++)
    //            {
    //                Plate.Well well = janus.DestinationPlates[0].GetWell(row, col);
    //                Console.WriteLine(well.AlphanumericWell);
    //                if (well.Compounds.Count == 0)
    //                    Assert.Fail("Well {0} does not have a compound. Dilution Factor: {1}", well.AlphanumericWell, janus.DilutionPoint);
    //            }
    //        }
    //    }
        
    //    #region Well List 
    //    private void TestTotalWellCount(List<int> columns, int dilutionFactor)
    //    {
    //        Plate destPlate = new Plate(384);
    //        JanusJob.TransferType transferType = JanusJob.TransferType.Dilution;
    //        List<Plate.Well> wells = JanusJob.GetWells(transferType, dilutionFactor);

    //        int expectedWellCount = destPlate.Rows * columns.Count;
    //        if (wells.Count != expectedWellCount)
    //            Assert.Fail("Dilution Factor: {0}. Total Well Count: {1}, Expected Well Count: {2}", dilutionFactor, wells.Count, expectedWellCount);

    //        Console.WriteLine("Wells:");
    //        foreach (Plate.Well well in wells)
    //        {
    //            if (!columns.Contains(well.Column))
    //                Assert.Fail("Improper well");
    //            Console.WriteLine(well);
    //        }
    //    }

    //    [TestMethod]
    //    public void Dilution_Factor_7_Get_Wells_Total_Count_Equal_48()
    //    {   
    //        int dilutionFactor = 7;
    //        List<int> columns = new List<int>() { 3, 10, 17};
    //        TestTotalWellCount(columns, dilutionFactor);
    //    }

    //    [TestMethod]
    //    public void Dilution_Factor_11_Get_Wells_Total_Count__Equal_32()
    //    {
    //        int dilutionFactor = 11;
    //        List<int> columns = new List<int>() { 3, 14 };
    //        TestTotalWellCount(columns, dilutionFactor);            
    //    }

    //    [TestMethod]
    //    public void Dilution_Factor_12_Get_Wells_Total_Count_Equal_32()
    //    {
    //        int dilutionFactor = 12;
    //        List<int> columns = new List<int>() { 1, 13 };
    //        TestTotalWellCount(columns, dilutionFactor);
    //    }
    //    [TestMethod]
    //    public void Dilution_Factor_22_Get_Wells_Total_Count_Equal_16()
    //    {
    //        int dilutionFactor = 22;
    //        List<int> columns = new List<int>() { 3 };
    //        TestTotalWellCount(columns, dilutionFactor);
    //    }
    //    [TestMethod]
    //    public void Dilution_Factor_24_Get_Wells_Total_Count_Equal_16()
    //    {
    //        int dilutionFactor = 24;
    //        List<int> columns = new List<int>() { 1 };
    //        TestTotalWellCount(columns, dilutionFactor);
    //    }
    //    [TestMethod]
    //    public void Get_Wells_1_To_1_TransferType()
    //    {
    //        JanusJob.TransferType transferType = JanusJob.TransferType._1to1;

    //        foreach (int dilutionFactor in DilutionFactors)
    //        {
    //            List<Plate.Well> wells = JanusJob.GetWells(transferType, dilutionFactor);
    //            Console.WriteLine("Dilution Factor: " + dilutionFactor + " Well Count: " + wells.Count);
    //            foreach (Plate.Well well in wells)
    //            {
    //                Console.WriteLine(well.AlphanumericWell);
    //            }
    //        }
    //    }
    //    #endregion




    //    [TestMethod]
    //    public void Janus_Dilution()
    //    {
    //        List<Plate> sourcePlates = GetSourcePlates();
    //        int transferVol = 10;
    //        JanusJob.TransferType transferType = JanusJob.TransferType.Dilution;

    //        foreach (int dilutionFactor in DilutionFactors)
    //        {
    //            List<Plate.Well> wells = JanusJob.GetWells(transferType, dilutionFactor);
    //            foreach (Plate.Well well in wells)
    //            {
    //                Console.WriteLine("Dilution Factor: " + dilutionFactor);
    //                try
    //                {
    //                    JanusJob janus = new JanusJob(sourcePlates, transferType, "Janus", dilutionFactor, transferVol, well, true);
    //                    Test_Janus_Layout(janus);
    //                }
    //                catch (Exception ex)
    //                {
    //                    Assert.Fail("Dilution Factor: {0}, Well: {1}, Exception: {2}", dilutionFactor, well.AlphanumericWell, ex.Message);
    //                }
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Gets a list of columns based on the Janus plate settings
    //    /// </summary>
    //    /// <param name="janus"></param>
    //    /// <returns>A list of columns</returns>
    //    private List<int> GetColumns(JanusJob janus)
    //    {
    //        List<int> columns = new List<int>();
    //        int dilutionFactor = janus.DilutionPoint;
    //        Plate.Well startingWell = janus.StartingWell;
    //        Console.Write("Columns: ");

    //        for (int col = startingWell.Column; col <= janus.DestinationPlates[0].Columns; col += dilutionFactor)
    //        {
    //            if (col + dilutionFactor - 1 <= janus.DestinationPlates[0].Columns)
    //            {
    //                columns.Add(col);
    //                Console.Write(col.ToString() + " ");
    //            }        
    //        }

    //        Console.WriteLine();

    //        return columns;
    //    }

    //    [TestMethod]
    //    public void Janus_Dilution_Check_All_Wells()
    //    {
    //        List<Plate> sourcePlates = GetSourcePlates();
    //        int transferVol = 10;

    //        JanusJob.TransferType transferType = JanusJob.TransferType.Dilution;

    //        foreach (int dilutionFactor in DilutionFactors)
    //        {
    //            Console.WriteLine("Dilution Factor: " + dilutionFactor);  
    //            List<Plate.Well> wells = JanusJob.GetWells(transferType, dilutionFactor);
    //            Plate.Well startingWell = wells[0];
    //            JanusJob janus = new JanusJob(sourcePlates, transferType, "Janus", dilutionFactor, transferVol, startingWell, true);
    //            List<int> columns = GetColumns(janus);              

    //            for (int col = 1; col <= janus.DestinationPlates[0].Columns; col++)
    //            {
    //                for (int row = 1; row <= janus.DestinationPlates[0].Rows; row++)
    //                {
    //                    Plate.Well well = janus.DestinationPlates[0].GetWell(row, col);
    //                    if (columns.Contains(well.Column))
    //                    {
    //                        Console.Write(well.ToString() + ",");
    //                        if (well.Compounds.Count == 0)
    //                            Assert.Fail("No compound in well {0}. Starting Well: {1}. Dilution Factor: {2}", well.AlphanumericWell, startingWell.AlphanumericWell, dilutionFactor);
    //                    }
    //                    else
    //                    {
    //                        if (well.Compounds.Count > 0)
    //                            Assert.Fail("Compound {0} found in well {1}. Dilution Factor: {2}", well.Compounds[0].SampleID, well.AlphanumericWell, dilutionFactor);
    //                    }
    //                }
    //                if (col == janus.DestinationPlates[0].Columns)
    //                    Console.WriteLine("");
    //            }
    //        }
    //    }

    //    [TestMethod]
    //    public void Janus_Dilution_Check_All_Transfers()
    //    {
    //        List<Plate> sourcePlates = GetSourcePlates();
    //        int transferVol = 10;

    //        JanusJob.TransferType transferType = JanusJob.TransferType.Dilution;

    //        foreach (int dilutionFactor in DilutionFactors)
    //        {
    //            Console.WriteLine("Dilution Factor: " + dilutionFactor);
    //            List<Plate.Well> wells = JanusJob.GetWells(transferType, dilutionFactor);
    //            Plate.Well startingWell = wells[0];
    //            JanusJob janus = new JanusJob(sourcePlates, transferType, "Janus", dilutionFactor, transferVol, startingWell, true);
    //            List<int> columns = GetColumns(janus);
                
    //            foreach (Plate destPlate in janus.DestinationPlates)
    //            {
    //                Console.WriteLine("Barcode: " + destPlate.Barcode);
    //                for (int col = 1; col <= destPlate.Columns; col++)
    //                {
    //                    for (int row = 1; row <= destPlate.Rows; row++)
    //                    {
    //                        Plate.Well well = destPlate.GetWell(row, col);
    //                        if (columns.Contains(well.Column))
    //                        {
    //                            Console.Write(well.ToString() + ",");
    //                            if (well.Compounds.Count == 0)
    //                                Assert.Fail("No compound in well {0}. Starting Well: {1}. Dilution Factor: {2}", well.AlphanumericWell, startingWell.AlphanumericWell, dilutionFactor);
    //                        }
    //                        else
    //                        {
    //                            if (well.Compounds.Count > 0)
    //                                Assert.Fail("Compound {0} found in well {1}. Dilution Factor: {2}", well.Compounds[0].SampleID, well.AlphanumericWell, dilutionFactor);
    //                        }
    //                    }
    //                    if (columns.Contains(col))
    //                        Console.WriteLine("");
    //                }
    //            }               
    //            Console.WriteLine("");
    //        }
    //    }

    //}
}

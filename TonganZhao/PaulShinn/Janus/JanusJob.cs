using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlateLibrary;
using PlateLibrary.Compounds;

namespace FOTSLibrary
{
    public class JanusJob
    {
        public enum TransferType
        {
            Dilution, _1to1
        }

        public class Transfer : Matrix.Transfer
        {
            public Compound Compound { get; set; }

            public Transfer(Plate sourcePlate, Plate.Well sourceWell, Compound compound, double volume)
            {
                SourcePlate = sourcePlate;
                SourceWell = sourceWell;
                Compound = compound;
                Volume = volume;
            }

        }
        
        public int DilutionPoint { get; set; }
        public string Instrument { get; private set; }
        public int TransferVolume { get; private set; }
        public Plate.Well StartingWell { get; private set; }
        public List<Plate> DestinationPlates { get; private set; } = new List<Plate>();
        public List<Plate> SourcePlates { get; private set; } = new List<Plate>();
        public List<Transfer> Transfers { get; private set; } = new List<Transfer>();
        public TransferType TypeOfTransfer
        {
            get; private set;
        }
        public bool SortedPlateMap
        {
            get; private set;
        }

        #region Static Properties
        private static List<int> _dilutionPoints = new List<int>(){ 7, 11, 12, 22, 24 };
        public static List<int> DilutionPoints
        {
            get
            {
                return _dilutionPoints;
            }
        }
        private static List<string> _instruments = new List<string>() { "Janus", "FX" };
        public static List<string> Instruments
        {
            get { return _instruments; }
        }
        #endregion

        public DataTable InstrumentTable
        {
            get { return GetInstrumentTable(); }
        }
        public DataTable PlatemapTable
        {
            get { return GetPlateMapTable(); }
        }

        private DataTable GetPlateMapTable()
        {
            DataTable dt = new DataTable("Platemap");
            for (int i = 0; i <= 6; i++)
                dt.Columns.Add(i.ToString());

            if (SortedPlateMap)
                Transfers = Transfers.OrderBy(x => x.DestPlate.Barcode).ThenBy(x => x.DestWell.Column).ThenBy(x => x.DestWell.Row).ToList();
            
            foreach (Transfer transfer in Transfers)
            {
                string[] s = transfer.DestPlate.Barcode.Split('-');
                DataRow dr = dt.NewRow();
                dr[0] = "PFR" + s[1];
                dr[1] = transfer.Compound.Barcode;
                dr[2] = transfer.Compound.SampleID;
                dr[3] = transfer.Compound.Concentration.ToString();
                dr[4] = transfer.DestPlate.Barcode;
                dr[5] = transfer.DestWell.GetWellNumber(384, true);
                dr[6] = transfer.DestWell.ToString();
                dt.Rows.Add(dr);
            }

            return dt;
        }
        private DataTable GetInstrumentTable()
        {
            DataTable dt = new DataTable(Instrument);
            dt.Columns.Add("Source");
            dt.Columns.Add("Well");
            dt.Columns.Add("Dest");
            DataColumn col = new DataColumn()
            {
                ColumnName = "DestWell",
                Caption = "Well"
            };
            dt.Columns.Add(col);
            dt.Columns.Add("Volume");

            foreach (Transfer transfer in Transfers)
            {
                DataRow dr = dt.NewRow();
                dr[0] = transfer.SourcePlate;
                dr[1] = transfer.SourceWell.GetWellNumber(96, Instrument == "Janus");
                dr[2] = transfer.DestPlate;
                dr[3] = transfer.DestWell.GetWellNumber(384, Instrument == "Janus");
                dr[4] = transfer.Volume;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        #region Constructors
        public JanusJob(List<Plate> sourcePlates, TransferType transferType, string instrument, int dilutionFactor, int transferVolume, string startingWell, bool sortedPlateMap)
            : this(sourcePlates, transferType, instrument, dilutionFactor, transferVolume, new Plate.Well(startingWell), sortedPlateMap)
        {

        }
        public JanusJob(List<Plate> sourcePlates, TransferType transferType, string instrument, int dilutionPoint, int transferVolume, Plate.Well startingWell, bool sortedPlateMap)
        {
            Instrument = instrument;
            TransferVolume = transferVolume;
            DilutionPoint = dilutionPoint;
            SourcePlates = sourcePlates;
            StartingWell = startingWell;
            SortedPlateMap = sortedPlateMap;
            TypeOfTransfer = transferType;

            CreateDestinationPlates();            
        }
        #endregion

        private void CreateDestinationPlates()
        {
            Plate destPlate = new Plate(384, "384-1");
            DestinationPlates.Add(destPlate);
            List<Transfer> janusTransfers = GetJanusTransfers(SourcePlates, TransferVolume);
            destPlate.SetCurrentWell(StartingWell);

            for (int i = 0; i < janusTransfers.Count; i++)
            {
                List<Transfer> newTransfers = janusTransfers.Skip(i).Take(destPlate.NumberOfRows - destPlate.CurrentWell.Row + 1).ToList();

                for (int j = 0; j < newTransfers.Count; j++)
                {
                    bool alternateWells = newTransfers.Count == destPlate.NumberOfRows;

                    janusTransfers[i + j].DestPlate = destPlate;
                    janusTransfers[i + j].DestWell = destPlate.CurrentWell;
                    destPlate.AddCompound(destPlate.CurrentWell, janusTransfers[i + j].Compound);

                    IncrementWell(alternateWells, destPlate);

                    if ((j + 1) == destPlate.NumberOfRows || (j + 1) == newTransfers.Count)
                    {
                        if (destPlate.CurrentWell.Column + DilutionPoint - 1 >= destPlate.NumberOfColumns)
                        {
                            destPlate = new Plate(384, "384-" + (DestinationPlates.Count + 1));
                            destPlate.SetCurrentWell(StartingWell);
                            DestinationPlates.Add(destPlate);
                        }
                        else
                        {
                            if (TypeOfTransfer == TransferType.Dilution)
                                destPlate.IncrementCurrentWell(false, DilutionPoint - 1);
                        }
                    }
                }

                Transfers.AddRange(newTransfers);
                i += newTransfers.Count - 1;
            }
        }

        private void IncrementWell(bool alternateWells, Plate destPlate)
        {
            if (alternateWells)
            {
                if (destPlate.CurrentWell.Row == destPlate.NumberOfRows - 1)
                    destPlate.SetCurrentWell(2, destPlate.CurrentWell.Column);
                else
                    destPlate.IncrementCurrentWell(true, 2);
            }
            else
                destPlate.IncrementCurrentWell();
        }

        public static List<Plate.Well> GetWells (TransferType transferType, int dilutionFactor)
        {
            Plate destPlate = new Plate(384);
            List<Plate.Well> wells = new List<Plate.Well>();

            if (transferType == TransferType.Dilution)
            {
                List<int> columns = new List<int>();
                if (dilutionFactor == 7 || dilutionFactor == 11 || dilutionFactor == 22)
                    columns.Add(3);
                else
                    columns.Add(1);

                for (int col = columns.First() + dilutionFactor; col <= destPlate.NumberOfColumns; col += dilutionFactor)
                {
                    if (col + dilutionFactor - 1 <= destPlate.NumberOfColumns)
                        columns.Add(col);
                }
                
                foreach (int column in columns)
                {
                    for (int row = 1; row <= destPlate.NumberOfRows; row++)
                    {
                        Plate.Well well = destPlate.GetWell(row, column);
                        wells.Add(well);
                    }
                }
            }
            else
            {
                foreach (int column in destPlate.Columns)
                {
                    foreach (int row in destPlate.Rows)
                    {
                        wells.Add(destPlate.GetWell(row, column));
                    }
                }
            }

            return wells;            
        }

        //public static List<Plate> GetSourcePlates(string text)
        //{
        //    List<Plate> sourcePlates = new List<Plate>();
        //    List<CompoundStore.PlateMap> plateMapList = CompoundStore.PlateMap.ParsePlateMapList(text);

        //    //foreach (CompoundStore.PlateMap plateMap in plateMapList)
        //    //{
        //    //    //Plate sourcePlate = sourcePlates.FirstOrDefault(x => x.Barcode == plateMap.PlatesTubeRack);
        //    //    //if (sourcePlate == null)
        //    //    //{
        //    //    //    sourcePlate = new Plate(96, plateMap.TubeRack);
        //    //    //    sourcePlates.Add(sourcePlate);
        //    //    //}
        //    //    //Plate.Well sourceWell = sourcePlate.GetWell(plateMap.RackWell);
        //    //    //Compound compound = new Compound(plateMap.Compound.SampleID, plateMap.Compound.Barcode);
        //    //    //compound.Concentration = new Concentration(plateMap.Compound.Concentration.ToString());
        //    //    //sourceWell.AddCompound(compound);
        //    //}

        //    return sourcePlates;
        //}

        public static List<Transfer> GetJanusTransfers(List<Plate> sourcePlates, double transferVolume)
        {
            List<Transfer> transfers = new List<Transfer>();

            foreach (Plate sourcePlate in sourcePlates)
            {
                foreach (int column in sourcePlate.Columns)
                {
                    foreach (int row in sourcePlate.Rows)
                    {
                        Plate.Well sourceWell = sourcePlate.GetWell(row, column);
                        if (sourceWell.Compounds.Count > 0)
                        {
                            Compound compound = sourceWell.Compounds[0];
                            Transfer transfer = new Transfer(sourcePlate, sourceWell, compound, transferVolume);
                            transfers.Add(transfer);
                        }
                    }
                }
            }

            return transfers;
        }
    }
}
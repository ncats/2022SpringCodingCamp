using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlateLibrary.Compounds;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;

namespace PlateLibrary
{
    public enum Orientation
    {
        Vertical, Horizontal
    }

    public class WellNotFoundException : Exception
    {
        public string AlphaNumericWell { get; private set; }
        public int Row { get; private set; }
        public int Column { get; private set; }
        
        public WellNotFoundException(int row, int column) : base("Well not found: Row " + row + " Column " + column)
        {
            Row = row;
            Column = column;
        }
        public WellNotFoundException(string alphanumericWell) : base("Well not found: " + alphanumericWell)
        {
            AlphaNumericWell = alphanumericWell;
        }
    }

    [DataContract]
    public class Plate
    {
        public enum Quadrant
        {
            Q1, Q2, Q3, Q4, None
        }        

        #region Private Member Variables
        private int _plateType;
        public static Random rand = new Random();
        #endregion

        #region Properties
        public string Barcode { get; set; }
        public int PlateType
        {
            get { return _plateType; }
            set
            {
                if (_plateType != value)
                {
                    _plateType = value;
                    AssignPlateProperties();
                }
            }
        }
        public List<int> Rows { get; private set; } = new List<int>();
        public int NumberOfRows
        {
            get { return Rows.Count; }
        }
        public List<int> Columns {  get; private set;  } = new List<int>();
        public int NumberOfColumns
        {
            get { return Columns.Count; }
        }
        public Well CurrentWell { get; private set; }
        public List<Well> Wells
        {
            get
            {
                List<Well> _wells = new List<Well>();
                foreach (int col in Columns)
                { 
                    foreach (int row in Rows)
                    {
                        _wells.Add(GetWell(row, col));
                    }
                }  
                return _wells; 
            }
        }
        private Well[,] WellArray { get; set; } = new Well[0, 0];
        private List<int> AcceptedPlateTypes
        {
            get
            {
                List<int> plateTypes = new List<int>();
                plateTypes.AddRange(new int[] { 6, 12, 24, 48, 96, 384, 1536, 3456 });
                return plateTypes;
            }
        }
        public Orientation PlateOrientation
        {
            get;
            private set;
        } = Orientation.Vertical;
        public List<Compound> Compounds
        {
            get
            {
                List<Compound> compounds = new List<Compound>();
                foreach (int row in Rows)
                {
                    foreach (int col in Columns)
                    {
                        Well well = GetWell(row, col);
                        if (well.Compounds.Count > 0)
                            compounds.AddRange(well.Compounds);
                    }
                }
                return compounds;
            }
        }
        
        #endregion

        #region Constructors
        public Plate (int plateType, string barcode, Orientation orientation)
            : this(plateType, barcode)
        {
            PlateOrientation = orientation;
        }
        public Plate(int plateType)
        {
            PlateType = plateType;
        }
        public Plate(int plateType, string barcode)
            : this(plateType)
        {
            Barcode = barcode;
        }
        #endregion

        #region Methods
      
        /// <summary>
        /// Increments current well by one well vertically or horizontally
        /// </summary>
        /// <param name="vertical"></param>
        public bool IncrementCurrentWell(bool vertical = true, int increment = 1)
        {
            if (vertical)
            {
                if (CurrentWell.Row + increment > NumberOfRows)
                    CurrentWell = WellArray[0, CurrentWell.Column];
                else
                    CurrentWell = GetWell(CurrentWell.Row + increment, CurrentWell.Column);
            }
            else
            {
                if (CurrentWell.Column + increment > NumberOfColumns)
                    CurrentWell = WellArray[CurrentWell.Row, 0];
                else
                    CurrentWell = GetWell(CurrentWell.Row, CurrentWell.Column + increment);
            }

            return true;
        }

        #region Set Current Well
        public void SetCurrentWell(int row, int column)
        {
            CurrentWell = GetWell(row, column);
        }
        public void SetCurrentWell(string alphanumericWell)
        {
            CurrentWell = GetWell(alphanumericWell);
        }
        public void SetCurrentWell(Plate.Well well)
        {
            CurrentWell = GetWell(well);
        }
        #endregion
        private void AssignPlateProperties()
        {
            if (!AcceptedPlateTypes.Contains(PlateType))
                throw new Exception("Invalid plate type", new Exception("Valid plate types include 6, 12, 24, 48, 96, 384, 1536, 3456"));

            int rows = 0, columns = 0;

            switch (PlateType)
            {
                case 6:
                    rows = 2;
                    columns = 3;
                    break;
                case 12:
                    rows = 3;
                    columns = 4;
                    break;
                case 24:
                    rows = 4;
                    columns = 6;
                    break;
                case 48:
                    rows = 6;
                    columns = 8;
                    break;
                case 96:
                    rows = 8;
                    columns = 12;
                    break;
                case 384:
                    rows = 16;
                    columns = 24;
                    break;
                case 1536:
                    rows = 32;
                    columns = 48;
                    break;
                case 3456:
                    rows = 48;
                    columns = 72;
                    break;
            }

            ResetWellArray(rows, columns);
        }
        public void ResetWellArray(int rows, int columns)
        {
            WellArray = new Well[rows, columns];
            
            for (int row = 1; row <= rows; row++)
            {
                if (!Rows.Contains(row))
                    Rows.Add(row);
                for (int column = 1; column <= columns; column++)
                {
                    Well well = new Well(row, column);
                    WellArray[row - 1, column - 1] = well;

                    if (PlateOrientation == Orientation.Vertical)
                        well.WellNumber = (well.Column - 1) * rows + well.Row;
                    else
                        well.WellNumber = (well.Row - 1) * columns + well.Column;             

                    if (!Columns.Contains(column))
                        Columns.Add(column);
                }
            }
            CurrentWell = WellArray[0, 0];
        }

        #region Random Wells
       

        public static List<Plate.Well> GenerateWellList()
        {
            List<Plate.Well> wells = new List<Plate.Well>();
            //Excludes columns 1-2 & 23-24 (Controls)

            for (int row = 1; row <= 24; row++)
            {
                for (int col = 3; col <= 22; col++)
                {
                    Plate.Well well = new Plate.Well(row, col);
                    wells.Add(well);
                }
            }

            return wells;
        }
        #endregion

        public static string ConvertToOddBarcode(string barcode)
        {
            if (!Regex.IsMatch(barcode, "[a-zA-Z][0-9]+"))
                throw new InvalidOperationException("Barcode " + barcode + " is not inproper format");

            string ret = "";
            //Letters section of barcode
            string letters = "";
            //Numbers section of barcode
            string numbers = "";
            
            int result;            
            foreach (char c in barcode)
            {
                if (int.TryParse(c.ToString(), out result))
                    numbers += result.ToString();
                else
                    letters += c.ToString();                
            }

            if (numbers.Length > 0)
            {
                int nums = Convert.ToInt32(numbers);
                if (nums % 2 == 0)
                    nums--;
                int zeros = numbers.Length - nums.ToString().Length;
                numbers = new string('0', zeros) + nums.ToString();
            }

            ret = letters + numbers;
            return ret;
        }

        #region Get Well
        public Well GetWell(string alphanumericWell)
        {
            try
            {
                Well well = new Well(alphanumericWell);
                return WellArray[well.Row - 1, well.Column - 1];
            }
            catch
            {
                throw new WellNotFoundException(alphanumericWell);
            }
        }
        public Well GetWell(int row, int column)
        {
            try
            {
                return WellArray[row - 1, column - 1];
            }
            catch
            {
                throw new WellNotFoundException(row, column);
            }
        }
        public Well GetWell(Well well)
        {
            return WellArray[well.Row - 1, well.Column - 1];
        }
        public Well GetWell(int wellNumber)
        {
            Well well = Well.GetWell(PlateType, wellNumber);
            return GetWell(well);
        }
        #endregion
        
        #region Add Compound
        public void AddCompound(Plate.Well well, Compound compound)
        {
            Well currWell = GetWell(well);
            currWell.AddCompound(compound);
        }
        public void AddCompound(int row, int column, Compound compound)
        {
            AddCompound(WellArray[row - 1, column - 1], compound);
        }
        public void AddCompound(Plate.Well well, string sampleID)
        {
            well.AddCompound(new Compound(sampleID));
            AddCompound(well, new Compound(sampleID));
        }
        #endregion
        #endregion

        #region overrides
        public override string ToString()
        {
            return Barcode;
        }
        #endregion

        public class Well
        {
            public class InvalidWellException : Exception
            {
                public string AlphanumericWell { get; set; }

                #region Constructors
                public InvalidWellException()
                {
                   
                }
                public InvalidWellException(string message) : base(message)
                {

                }
                #endregion
            }

            #region Properties
            public string AlphanumericWell { get; private set; } = "";
            public int Row { get; set; }
            public string RowLetter { get; private set; } = "";
            public int Column { get; set; }
            public List<Compound> Compounds { get; set; } = new List<Compound>();
            public bool ContainsDMSO
            {
                get
                {
                    foreach (Compound cmpd in Compounds)
                    {
                        if (cmpd.SampleID.ToUpper() == "DMSO")
                            return true;
                    }
                    return false;
                }
            }
            public bool IsEmpty
            {
                get { return Compounds.Count == 0; }
            }
            public int WellNumber { get; set; }
            #endregion

            #region Constructors
            public Well(int row, int column)
            {
                Row = row;
                Column = column;
                
                if (row <= 26) //A-Z
                    RowLetter = Convert.ToChar(row + 64).ToString();
                else
                    RowLetter = "A" + Convert.ToChar(row - 26 + 64).ToString();

                if (column.ToString().Length == 1)
                    AlphanumericWell = RowLetter + "0" + column.ToString();
                else
                    AlphanumericWell = RowLetter + column.ToString();
            }
            public Well(string alphanumericWell)
            {
                if (alphanumericWell == "" || alphanumericWell == null)
                    return;
                AlphanumericWell = alphanumericWell.Trim();
                if (!Regex.IsMatch(alphanumericWell, "[a-zA-Z]{1,2}\\d{1,3}"))
                    throw new Exception("Incorrect well format: " + alphanumericWell);

                int n = 0;
                int letters = 0;

                foreach (char c in alphanumericWell)
                {
                    if (!int.TryParse(c.ToString(), out n))
                        letters++;
                }

                if (letters == 1)
                {
                    RowLetter = alphanumericWell.Substring(0, 1);
                    Row = Convert.ToChar(RowLetter) - 64;
                    Column = Convert.ToInt16(alphanumericWell.Substring(1));
                }
                else if (letters == 2)
                {
                    RowLetter = alphanumericWell.Substring(0, 2);
                    Row = 26 + Convert.ToChar(RowLetter.Substring(1, 1)) - 64;
                    Column = Convert.ToInt16(alphanumericWell.Substring(2));
                }
            }
            public Well(int wellNumber, int plateType, bool vertical = true)
            {
                Well well = Well.GetWell(plateType, wellNumber, vertical);
                AlphanumericWell = well.AlphanumericWell;
                Row = well.Row;
                Column = well.Column;
                RowLetter = well.RowLetter;                
            }
            #endregion

            #region Methods
            /// <summary>
            /// 
            /// </summary>
            /// <param name="zeros">Number of characters</param>
            /// <returns></returns>
            public string ToColumnString(int length)
            {
                string ret = Column.ToString();

                while (ret.Length < length)
                {
                    ret = "0" + Column.ToString();
                }

                return ret;
            }
            public Quadrant GetQuadrant()
            {
                return GetQuadrant(this);
            }
            #endregion

            #region Static Methods
            private static Random _rand = new Random();
            public static Well GetRandomWell(List<Well> WellArray)
            {
                int index = _rand.Next(0, WellArray.Count);
                Well randomWell = WellArray[index];
                return randomWell;
            }
            public static Well GetWell(int plateType, int wellNumber, bool vertical = true)
            {
                int row = GetRow(plateType, wellNumber);
                int column = GetColumn(plateType, wellNumber);
                Well well = new Well(row, column);
                return well;
            }
            public static int GetColumn(int plateType, int wellNumber)
            {
                Plate plate = new Plate(plateType);

                if (wellNumber % plate.NumberOfRows == 0)
                    return wellNumber / plate.NumberOfRows;
                else
                    return Convert.ToInt16(wellNumber / plate.NumberOfRows) + 1;
            }
            public static int GetRow(int plateType, int wellNumber)
            {
                Plate plate = new Plate(plateType);

                if (wellNumber % plate.NumberOfRows == 0)
                    return plate.NumberOfRows;
                else
                    return wellNumber % plate.NumberOfRows;
            }
            public static int GetWellNumber(Plate.Well well, int plateType, bool vertical = true)
            {
                return GetWellNumber(well.AlphanumericWell, plateType, vertical);
            }
            public static int GetWellNumber(string alphanumericWell, int plateType, bool vertical = true)
            {
                Plate plate = new Plate(plateType);
                Well well = new Well(alphanumericWell);
                int ret = 0;

                if (vertical)
                    ret = (well.Column - 1) * plate.NumberOfRows + well.Row;
                else
                    ret = (well.Row - 1) * plate.NumberOfColumns + well.Column;

                return ret;
            }
            public static Well TransposeWell(Well originalWell, int plateType)
            {
                int originalWellNumber = GetWellNumber(originalWell, plateType);
                int flippedWellNumber = Math.Abs(originalWellNumber - plateType - 1);
                Well flippedWell = GetWell(384, flippedWellNumber);
                return flippedWell;
            }
            public static Well TransposeWell(int row, int column, int plateType)
            {
                Well originalWell = new Well(row, column);
                return TransposeWell(originalWell, plateType);
            }
            public static Quadrant GetQuadrant(Well well)
            {
                Quadrant quadrant = Quadrant.None;

                if (well.Row.IsNumberOdd() && well.Column.IsNumberOdd())
                    quadrant = Quadrant.Q1;
                else if (well.Row.IsNumberOdd() && well.Column.IsNumberEven())
                    quadrant = Quadrant.Q2;
                else if (well.Row.IsNumberEven() && well.Column.IsNumberOdd())
                    quadrant = Quadrant.Q3;
                else if (well.Row.IsNumberEven() && well.Column.IsNumberEven())
                    quadrant = Quadrant.Q4;

                return quadrant;
            }
            #endregion

            #region Methods
            #region Add Compound
            public void AddCompound(string sampleID)
            {
                Compound compound = new Compound(sampleID);
                Compounds.Add(compound);
            }
            public void AddCompound(string sampleID, string barcode)
            {
                Compound compound = new Compound(sampleID, barcode);
                Compounds.Add(compound);
            }
            public void AddCompound(string sampleID, string barcode, string concentration)
            {
                Compound compound = new Compound(sampleID, barcode);
                compound.Concentration = new Concentration(concentration);
                Compounds.Add(compound);
            }
            public void AddCompound(Compound compound)
            {
                Compounds.Add(compound);
            }
            #endregion
            public int GetWellNumber(int plateType, bool vertical = true)
            {
                int ret = 0;
                ret = Plate.Well.GetWellNumber(this.AlphanumericWell, plateType, vertical);

                return ret;
            }
            #endregion

            #region overrides
            public override int GetHashCode()
            {
                return AlphanumericWell.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                Well otherWell = (Well)obj;
                if (otherWell == null)
                    return false;
                else
                    return otherWell.AlphanumericWell == this.AlphanumericWell;
            }
            public override string ToString()
            {
                return AlphanumericWell;
            }
            #endregion
        }
    }
}

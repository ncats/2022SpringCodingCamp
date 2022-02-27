using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlateLibrary;
using System.Data;

namespace PlateLibrary
{
    public class StampPlate : Plate
    {
        public Dictionary<Quadrant, Plate> SourcePlateQuadrants { get; set; } = new Dictionary<Quadrant, Plate>();
        public List<Plate> SourcePlates
        {
            get { return SourcePlateQuadrants.Values.ToList(); }
        }
        
        public StampPlate (int destPlateType, string barcode, Orientation plateOrientation = Orientation.Horizontal): 
            base(destPlateType, barcode, plateOrientation)
        {

        }

        /// <summary>
        /// Adds compounds from a source plate to this plate based on quadrant
        /// </summary>
        /// <param name="sourcePlate"></param>
        /// <param name="quadrant"></param>
        public void AddPlate(Plate sourcePlate, Quadrant quadrant)
        {
            SourcePlateQuadrants.Add(quadrant, sourcePlate);

            foreach (Plate.Well well in sourcePlate.Wells)
            {
                if (well.Compounds.Count > 0)
                {
                    Plate.Well newWell = MapNewWell(well, quadrant, sourcePlate.PlateType, PlateType);
                    AddCompound(newWell, well.Compounds[0]);
                }
            }                     
        }
        public void AddPlate(Plate sourcePlate, string quadrant)
        {
            Plate.Quadrant oQuadrant = (Plate.Quadrant)Enum.Parse(typeof(Plate.Quadrant), quadrant.ToUpper());
            AddPlate(sourcePlate, oQuadrant);          
        }

        #region Map New Well
        public static Well MapNewWell(Plate.Well well, int currentPlateType, int targetPlateType)
        {
            Quadrant quadrant = well.GetQuadrant();
            int newRow = MapNewRow(quadrant, well.Row, currentPlateType, targetPlateType);
            int newColumn = MapNewColumn(quadrant, well.Column, currentPlateType, targetPlateType);
            return new Well(newRow, newColumn);
        }
        public static Well MapNewWell(Plate.Well well, Quadrant quadrant, int currentPlateType, int targetPlateType)
        {
            int newRow = MapNewRow(quadrant, well.Row, currentPlateType, targetPlateType);
            int newColumn = MapNewColumn(quadrant, well.Column, currentPlateType, targetPlateType);
            return new Well(newRow, newColumn);
        }
        #endregion

        public static double GetMappingFactor(int currentPlateType, int targetPlateType)
        {
            return Math.Sqrt((double)targetPlateType / (double)currentPlateType);
        }

        public static int MapNewRow(Quadrant quadrant, int currentRow, int currentPlateType, int targetPlateType)
        {
            double factor = GetMappingFactor(currentPlateType, targetPlateType);
            //1 to 1 mapping
            if (factor == 1)
                return currentRow;

            int newRow = -1;
            if (factor < 1)
            {
                if (quadrant == Quadrant.Q1 || quadrant == Quadrant.Q2)
                    newRow = Convert.ToInt16((currentRow - 1) * factor) + 1;
                else if (quadrant == Quadrant.Q3 || quadrant == Quadrant.Q4)
                    newRow = Convert.ToInt16(currentRow * factor);
            }
            else if (factor > 1)
            {
                if (quadrant == Quadrant.Q1 || quadrant == Quadrant.Q2)
                    newRow = Convert.ToInt16(currentRow * factor) - 1;
                else if (quadrant == Quadrant.Q3 || quadrant == Quadrant.Q4)
                    newRow = Convert.ToInt16(currentRow * factor);
            }

            return newRow;
        }
        public static int MapNewColumn(Quadrant quadrant, int currentColumn, int currentPlateType, int targetPlateType)
        {
            double factor = GetMappingFactor(currentPlateType, targetPlateType);
            //1 to 1 mapping
            if (factor == 1)
                return currentColumn;
            
            int newColumn = -1;
            if (factor < 1)
            {
                if (quadrant == Quadrant.Q1 || quadrant == Quadrant.Q3)
                    newColumn = Convert.ToInt16((currentColumn - 1) * factor) + 1;
                else if (quadrant == Quadrant.Q2 || quadrant == Quadrant.Q4)
                    newColumn = Convert.ToInt16(currentColumn * factor);
            }
            else if (factor > 1)
            {
                if (quadrant == Quadrant.Q1 || quadrant == Quadrant.Q3)
                    newColumn = Convert.ToInt16(currentColumn * factor) - 1;
                else if (quadrant == Quadrant.Q2 || quadrant == Quadrant.Q4)
                    newColumn = Convert.ToInt16(currentColumn * factor);
            }

            return newColumn;
        }
    }
}

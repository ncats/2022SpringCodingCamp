using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlateLibrary;

namespace Instruments.Tecan
{
    public class TecanPlate : Plate
    {
        #region Properties
        public StringBuilder TecanOutput { get; set; } = new StringBuilder();
        public string PlateFormat { get; set; }
        public string PlateDescription
        {
            get { return GetPlateDescription(); }
        }
        private string GetPlateDescription()
        {
            string ret = "";

            switch (PlateType)
            {
                case 96:
                    ret = "96 Matrix with 2D";
                    break;
                case 48:
                    ret = "48-well QC tray fixedtips";
                    break;
                case 24:
                    ret = "Genevac24 DITI";
                    break;
            }

            if (PlateType == 96 && PlateFormat == "DMSO")
                ret = "96 Matrix with 2D piercing";
            else if (PlateType == 24 && PlateFormat == "DMSO")
                ret = "Genevac24-fixed tips";

            return ret;
        }
        public string PlateNumber
        {
            get { return GetPlateNumber(); }
        }
        private string GetPlateNumber()
        {
            string ret = "";

            string[] s = Barcode.Trim().Split(new string[] { "_", "-" }, StringSplitOptions.RemoveEmptyEntries);

            if (s.Length == 1)
            {
                if (Barcode.Length < 4)
                    ret = Barcode;
                //else
                //    ret = Barcode.Right(4);
            }
            else
            {
                ret = s[1].Trim();
            }

            return ret;
        }

        public int DMSOTip
        {
            get;set;
        }
        #endregion


        public TecanPlate(int plateType, string barcode, string plateFormat, int dmsoTip) : base(plateType)
        {
            PlateFormat = plateFormat;
            Barcode = barcode;
            DMSOTip = dmsoTip;
        }

        public void AddCompound(Plate.Well well, int volume)
        {
            int dispenses = 0;
            int maxVolume = 950;

            if (well.Row > NumberOfRows || well.Column > NumberOfColumns)
                throw new Exception(PlateType + " plate does not have well location " + well.ToString());

            if (volume > maxVolume)
                dispenses = Convert.ToInt16(volume / maxVolume) + 1;
            else
                dispenses = 1;

            volume = Convert.ToInt16(Convert.ToDouble(volume / dispenses));

            for (int i = 1; i <= dispenses; i++)
            {
                DMSOTip++;
                if (DMSOTip > 8)
                    DMSOTip = 1;

                int destWell = Plate.Well.GetWellNumber(well, PlateType);
                AddTecanTransfer(volume, DMSOTip, destWell);
            }

        }

        public void AddTecanTransfer(int volume, int sourceWell, int destWell)
        {
            string text = "";
            string format = PlateFormat;

            if (PlateFormat == "Reservoir")
                text = "Res1";
            else if (PlateFormat == "DMSO")
            {
                text = "System";
                format = "SystemLiquid";
            }
            else
                throw new InvalidProgramException("Incorrect format: " + PlateFormat + "\r\n.  Format must be Reservoir or DMSO");

            TecanOutput.AppendLine("A;" + text + ";;" + format + ";" + sourceWell + ";;" + volume + ";;;;");
            TecanOutput.AppendLine("D;" + Barcode + ";;" + PlateDescription + ";" + destWell + ";;" + volume + ";;;;");
            TecanOutput.AppendLine("W;");
        }
    }
}

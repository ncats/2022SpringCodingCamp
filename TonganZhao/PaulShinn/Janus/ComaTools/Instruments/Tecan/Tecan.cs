using PlateLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlateLibrary.Compounds;
using HelperUtility.Files;


namespace Instruments.Tecan
{
    public enum PlateFormat
    {
        Reservoir, DMSO
    }

    public class TecanJob
    {

        public List<IFile> Files
        {
            get { return GetFiles(); }
        }

        public List<Plate> SourcePlates { get; set; } = new List<Plate>();
        public List<TecanPlate> DestPlates { get; set; } = new List<TecanPlate>();

        public int DestinationPlateType { get; private set; }

        public string PlateFormat { get; set; }

        public TecanJob(List<Plate> sourcePlates, int plateType, string plateFormat)
        {
            SourcePlates = sourcePlates;
            DestinationPlateType = plateType;
            PlateFormat = plateFormat;
            DestPlates = CreateDestPlates();
        }

        private List<IFile> GetFiles()
        {
            List<IFile> files = new List<IFile>();

            for (int i = 0; i < DestPlates.Count; i+=6)
            {
                StringBuilder sb = new StringBuilder();
                List<TecanPlate> plates = DestPlates.Skip(i).Take(6).ToList();
                List<string> tecanText = plates.Select(x => x.TecanOutput.ToString().Trim()).ToList();
                tecanText.ForEach((s) =>
                {
                    sb.AppendLine(s);
                });
                
                string firstPlateNumber = plates.First().PlateNumber;
                string lastPlateNumber = plates[plates.Count() - 1].PlateNumber;

                File file = new File("TecanPlates_" + firstPlateNumber + "-" + lastPlateNumber + ".gwl");
                file.FileBytes = Encoding.ASCII.GetBytes(sb.ToString().Trim());
                files.Add(file);                
            }
            
            return files;
        }

        private List<TecanPlate> CreateDestPlates()
        {
            List<TecanPlate> tecanPlates = new List<TecanPlate>();
            Plate destPlate = new Plate(DestinationPlateType);
            int dmsoTip = 0;

            foreach (Plate sourcePlate in SourcePlates)
            {
                foreach (int col in destPlate.Columns)
                {
                    foreach (int row in destPlate.Rows)
                    {
                        TecanPlate tecanPlate = tecanPlates.FirstOrDefault(x => x.Barcode == sourcePlate.Barcode);
                        if (tecanPlate == null)
                        {
                            tecanPlate = new TecanPlate(DestinationPlateType, sourcePlate.Barcode, PlateFormat, dmsoTip);
                            tecanPlates.Add(tecanPlate);
                        }
                        Plate.Well sourceWell = sourcePlate.GetWell(row, col);
                        if (sourceWell.Row > tecanPlate.NumberOfRows || sourceWell.Column > tecanPlate.NumberOfColumns)
                            throw new Exception(DestinationPlateType + " plate does not have well location " + sourceWell.ToString());

                        if (sourceWell.Compounds.Count > 0)
                            tecanPlate.AddCompound(sourceWell, Convert.ToInt16(sourceWell.Compounds[0].Amount));
                        dmsoTip = tecanPlate.DMSOTip;
                    }
                }
            }          

            return tecanPlates;
        }


      
    }

  
}

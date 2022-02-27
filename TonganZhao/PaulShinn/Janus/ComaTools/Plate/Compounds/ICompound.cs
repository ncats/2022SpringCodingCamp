using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateLibrary.Compounds
{
    public interface ICompound
    {
        double Amount { get; set; }
        //string Barcode { get; set; }
        string BatchNumber { get; set; }
        Concentration Concentration { get; set; }
        string NCGCRoot { get; set; }
        string RegistrationNumber { get; set; }
        string SampleID { get; set; }        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace PlateLibrary.Compounds
{
    [DataContract]
    ///<summary>
    /// This class contains all the methods, properties and functions of the compound class.
    /// Compounds exist in wells, plates, tubes and vials
    /// </summary>
    public class Compound : ICompound
    {
        private string _sampleID = "";

        #region Properties
        /// <summary>
        /// The amount (typically uL, nL or mg) of the compound
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// The barcode of the tube or vial the compound is located in.  Typically a 1 to many relationship.  One compound can have multiple vial or tube barcodes
        /// </summary>
        [DataMember (Name ="barcode")]
        public string Barcode { get; set; }
        /// <summary>
        /// The batch number of the compound. The batch number is the numbers in the sample id after the dash
        /// </summary>
        public string BatchNumber { get;  set; }
        /// <summary>
        /// The concentration of the compound typically expressed in mM or uM
        /// </summary>
        public Concentration Concentration { get; set; }
        /// <summary>
        /// The root id of a compound.  Typically all letters and numbers before the dash in the sample id
        /// </summary>
        public string NCGCRoot { get; set; }
        /// <summary>
        /// The registration number given to a compound after it has been registered by compound management.  After it has been registered by compound management, it is equal to the sample id
        /// </summary>
        public string RegistrationNumber { get; set; }
        ///<summary>The main identifier of a compound. 
        ///Before it has been registered by compound management, it is typically in the following format: CTL-000.
        ///After it has been registered, the typical format is 15 characters long like so: NCGC12345678-10.</summary>
        [DataMember (Name = "sampleId")]
        public string SampleID
        {
            get { return _sampleID; }
            set
            {
                if (_sampleID != value)
                {
                    _sampleID = value;
                    GetNCGCRoot();
                }
            }
        }
        /// <summary>
        /// Any additional properties of a compound not defined by existing properties
        /// </summary>
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();        
        #endregion

        #region Constructor
        public Compound(string sampleID, string barcode, double amount)
            : this (sampleID, barcode)
        {
            Amount = amount;
        }
        public Compound(string sampleID, string barcode)
            : this (sampleID)
        {
            Barcode = barcode;
        }
        public Compound(string sampleID, double concentration)
            : this(sampleID)
        {
            Concentration = new Concentration(concentration);
        }
        public Compound(string sampleID)
        {
            SampleID = sampleID.Trim();
        }
        public Compound()
        {
            SampleID = "";
            Barcode = "";
        }
        #endregion

        /// <summary>
        /// Returns the NCGC root of a compound
        /// </summary>
        private void GetNCGCRoot()
        {
            int index = SampleID.IndexOf("-");
            if (index < 0)
            {
                NCGCRoot = _sampleID;
                BatchNumber = "";
            }
            else
            {
                string[] s = _sampleID.Split('-');
                NCGCRoot = s[0];
                BatchNumber = s[1];
            }
        }

        #region overrides
        public override int GetHashCode()
        {
            return SampleID.GetHashCode() + Concentration.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType())
                return false;
            Compound otherCompound = (Compound)obj;
            return SampleID == otherCompound.SampleID && Concentration == otherCompound.Concentration;
        }
        public override string ToString()
        {
            return SampleID;
        }
        #endregion
    }
}

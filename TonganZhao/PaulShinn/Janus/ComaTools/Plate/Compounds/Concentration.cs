using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateLibrary.Compounds
{
    public class Concentration
    {
        public class InvalidConcentrationException : Exception
        {

            #region Constructors
            public InvalidConcentrationException(string text) : base ("Could not parse concentration: " + text)
            {

            }
            public InvalidConcentrationException(string message, Exception innerException)
            {

            }
            #endregion
        }

        public double Value { get; set; } = 0;
        public string Units { get; set; } = "";

        public Concentration(double value)
        {
            Value = value;
        }
        public Concentration(string concentration)
        {
            if (string.IsNullOrEmpty(concentration)) return;

            double ret;
            concentration = concentration.Trim();

            if (double.TryParse(concentration, out ret))
                Value = ret;
            else
            {
                try
                {
                    int index = concentration.IndexOf(' ');
                    if (index > 0)
                    {
                        string[] s = concentration.Split(' ');
                        Value = Convert.ToDouble(s[0].Trim());
                        Units = s[1].Trim();
                    }
                    else
                    {
                        for (int i = 0; i < concentration.Trim().Length; i++)
                        {
                            if (double.TryParse(concentration.Substring(0, i + 1), out ret))
                                Value = ret;
                        }

                        foreach (char c in concentration.Trim())
                        {
                            if (double.TryParse(Value.ToString() + c.ToString(), out ret) == false)
                            {
                                if (c != '.')
                                    Units += c.ToString().Trim();
                            }
                        }
                    }
                }
                catch
                {
                    throw new Exception("Could not parse concentration: " + concentration);
                }
            }
        }

        #region Overrides
        public override int GetHashCode()
        {
            return Value.GetHashCode() + Units.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType())
                return false;
            Concentration otherConc = (Concentration)obj;
            return Value == otherConc.Value && Units == otherConc.Units;
        }
        public override string ToString()
        {
            return Value.ToString() + Units.Trim();
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateLibrary
{
    public static class ExtensionMethods
    {
        public static bool IsNumberEven(this int number)
        {
            return number % 2 == 0;
        }
        public static bool IsNumberOdd(this int number)
        {
            return number % 2 == 1;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormalSpecification
{
    internal class ReplaceAndRemove
    {
        public static void ReplaceTab(string line)
        {
            line.Replace("\t", "");
        }
        public static void ReplaceEndline(string line)
        {
            line.Replace("\r\n", "");
        }
        public static void RemoveString(string line)
        {
            line = string.Empty;
        }
        public static void ReplaceSpace(string line)
        {
            line.Replace(" ", "");
        }
    }
}

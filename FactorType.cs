using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormalSpecification
{
    class FactorType
    {
        public static string ChangeFactorType(string factorType)
        {
            switch(factorType)
            {
                case "N":
                    return "int";
                case "N*":
                    return "int[]";
                case "Z":
                    return "int";
                case "Z*":
                    return "int[]";
                case "Q":
                    return "double";
                case "R":
                    return "double";
                case "R*":
                    return "double[]";
                case "B":
                    return "bool";
                case "char*":
                    return "string";
                default:
                    Console.WriteLine("Invalid datatype!");
                    return "0";
            }
        }
    }
}

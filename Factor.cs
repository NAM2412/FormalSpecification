using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormalSpecification
{
    class Factor
    {
        private string factorName;
        private string factorType;
        public Factor()
        {
            factorName = factorType = null;
        }
        public Factor(string name, string type)
        {
            factorType = type;
            factorName = name;
        }
        public string FactorName { get => factorName; set => factorName = value; }
        public string FactorType { get => factorType; set => factorType = value; }
    }
}

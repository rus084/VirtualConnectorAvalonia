using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualConnectorAvalonia.Connections.Params
{
    public class StaticEnumParam : EnumParam
    {
        public override IEnumerable<string> getValidValues()
        {
            return ValidValues;
        }

        private string[] ValidValues;

        public StaticEnumParam(int paramNumber, string paramName, string[] validValues, int defaultIndex) : base(paramNumber, paramName)
        {
            ValidValues = validValues;
            DefaultIndex = defaultIndex;
        }
    }
}

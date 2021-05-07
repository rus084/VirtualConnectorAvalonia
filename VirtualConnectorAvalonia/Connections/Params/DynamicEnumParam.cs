using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualConnectorAvalonia.Connections.Params
{
    public class DynamicEnumParam : EnumParam
    {
        public override IEnumerable<string> getValidValues()
        {
            return ValidValuesGetter();
        }

        Func<string[]> ValidValuesGetter;

        public DynamicEnumParam(int paramNumber, string paramName, Func<string[]> validValuesGetter, int defaultIndex) : base(paramNumber, paramName)
        {
            ValidValuesGetter = validValuesGetter;
            DefaultIndex = defaultIndex;
        }
    }
}

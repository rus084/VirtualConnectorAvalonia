using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualConnectorAvalonia.Connections.Params
{
    public abstract class EnumParam : ConnectionParam
    {
        public abstract IEnumerable<string> getValidValues();
        public int DefaultIndex;

        public EnumParam(int paramNumber, string paramName)
        {
            ParamNumber = paramNumber;
            ParamName = paramName;
        }
    }
}

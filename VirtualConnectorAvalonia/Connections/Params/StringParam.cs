using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualConnectorAvalonia.Connections.Params
{
    public class StringParam : ConnectionParam
    {
        public string DefaultValue;
        public StringParam(int paramNumber, string paramName, string defaultValue)
        {
            ParamNumber = paramNumber;
            ParamName = paramName;
            DefaultValue = defaultValue;
        }
    }
}

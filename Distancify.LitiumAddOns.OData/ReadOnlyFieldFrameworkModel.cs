using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distancify.LitiumAddOns.OData
{
    public abstract class ReadOnlyFieldFrameworkModel
    {
        public abstract T GetValue<T>(string fieldName);
    }
}

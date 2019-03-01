using System.Globalization;

namespace Distancify.LitiumAddOns.OData
{
    public abstract class ReadOnlyFieldFrameworkModel
    {
        public abstract T GetValue<T>(string fieldName);
        public abstract T GetValue<T>(string fieldName, CultureInfo culture);
    }
}

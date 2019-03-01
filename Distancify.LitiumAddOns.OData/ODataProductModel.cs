using JetBrains.Annotations;
using Litium.Products;
using System.Globalization;

namespace Distancify.LitiumAddOns.OData
{
    public class ODataProductModel : ReadOnlyFieldFrameworkModel
    {
        public ODataProductModel(BaseProduct baseProduct, Variant variant)
        {
            BaseProduct = baseProduct;
            Variant = variant;
        }

        public BaseProduct BaseProduct { get; }
        public Variant Variant { get; }

        public override T GetValue<T>(string fieldName)
        {
            if (!TryGetValue(fieldName, out object value, CultureInfo.CurrentUICulture))
                return default(T);

            return (T)value;
        }

        public override T GetValue<T>(string fieldName, CultureInfo culture)
        {
            if (!TryGetValue(fieldName, out object value, culture))
                return default(T);

            return (T)value;
        }

        private bool TryGetValue([NotNull] string id, out object value, CultureInfo culture)
        {
            value = Variant.Fields[id, culture] ?? Variant.Fields[id];
            if (value == null)
            {
                value = BaseProduct.Fields[id, culture] ?? BaseProduct.Fields[id];
            }
            return value != null;
        }
        
    }
}

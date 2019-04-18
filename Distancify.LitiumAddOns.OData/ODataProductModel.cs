using JetBrains.Annotations;
using Litium.Owin.Logging;
using Litium.Products;
using System;
using System.Collections.Generic;
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

            return ToType<T>(value);
        }

        public override T GetValue<T>(string fieldName, CultureInfo culture)
        {
            if (!TryGetValue(fieldName, out object value, culture))
                return default(T);

            return ToType<T>(value);
        }

        private T ToType<T>(object value)
        {
            if (value == null) return default(T);

            try
            {
                if (typeof(T) == typeof(string) && value is IList<string> values)
                {
                    return (T)(object)string.Join(",", values);
                }
                else if (typeof(T) != value.GetType())
                {
                    return Convert.ChangeType(value, typeof(T));
                }

                return (T)value;
            }
            catch (Exception ex)
            {
                Log.GetLoggerFor(nameof(ODataProductModel)).Error("Could not convert field value from type " + value.GetType() + " to type " + typeof(T), ex);
                return default(T);
            }
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

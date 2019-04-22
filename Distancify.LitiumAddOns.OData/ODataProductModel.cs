using JetBrains.Annotations;
using Litium.Owin.Logging;
using Litium.Products;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Distancify.LitiumAddOns.OData
{
    public class ODataProductModel : ReadOnlyFieldFrameworkModel
    {
        private readonly IDictionary<string, decimal> prices;

        public ODataProductModel(BaseProduct baseProduct, Variant variant, IDictionary<string, decimal> prices)
        {
            BaseProduct = baseProduct;
            Variant = variant;
            this.prices = prices;
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

        public decimal? GetPrice(string listId)
        {
            if (prices.TryGetValue(listId, out decimal price))
                return price;

            return null;
        }

        private T ToType<T>(object value)
        {
            if (value == null) return default(T);

            if (value is T v)
            {
                return v;
            }

            try
            {
                if (typeof(T) == typeof(string) && value is IList<string> values)
                {
                    return (T)(object)string.Join(",", values);
                }

                return (T)Convert.ChangeType(value, typeof(T));
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

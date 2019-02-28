using JetBrains.Annotations;
using Litium.FieldFramework;
using Litium.FieldFramework.FieldTypes;
using Litium.Products;
using Litium.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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
            if (!TryGetValue(fieldName, out object value))
                return default(T);

            return (T)value;
        }

        private bool TryGetValue([NotNull] string id, out object value)
        {
            value = Variant.Fields[id, CultureInfo.CurrentUICulture] ?? Variant.Fields[id];
            if (value == null)
            {
                value = BaseProduct.Fields[id, CultureInfo.CurrentUICulture] ?? BaseProduct.Fields[id];
            }
            return value != null;
        }
        
    }
}

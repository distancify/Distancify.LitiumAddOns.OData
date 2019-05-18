using AutoMapper;
using Litium;
using Litium.FieldFramework;
using Litium.FieldFramework.FieldTypes;
using Litium.Products;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Distancify.LitiumAddOns.OData.AutoMapperExtensions
{
    public static class TextOptionMappings
    {
        public static void MapFromTextOptionLabel<TSource, TDestination, TMember>(
            this IMemberConfigurationExpression<TSource, TDestination, TMember> config,
            string fieldId,
            CultureInfo culture)
            where TSource : ODataProductModel
        {
            config.MapFromTextOptionLabel(fieldId, culture.ToString());
        }

        public static void MapFromTextOptionLabel<TSource, TDestination, TMember>(
        this IMemberConfigurationExpression<TSource, TDestination, TMember> config,
        string fieldId,
        string culture)
        where TSource : ODataProductModel
        {
            Func<ODataProductModel, string> mapping = model =>
            {
                var field = IoC.Resolve<FieldDefinitionService>().Get<ProductArea>(fieldId);
                var options = field.Option as TextOption;
                if (options == null)
                    return null;

                var value = model.GetValue<object>(fieldId);

                var result = new List<string>();
                if (value is IList<string> values)
                {
                    foreach (var i in values)
                    {
                        AddLabel(options, i, culture, result);
                    }
                }
                else
                {
                    AddLabel(options, (string)value, culture, result);
                }

                return string.Join(",", result);
            };
    
            config.MapFrom(model => mapping(model));
        }

        private static void AddLabel(TextOption options, string key, string culture, IList<string> result)
        {
            var label = options.Items.Where(r => r.Value == key)
                .Select(r => r.Name)
                .Where(r => r.ContainsKey(culture.ToString()))
                .Select(r => r[culture.ToString()])
                .FirstOrDefault();
            if (label != null)
                result.Add(label);
        }
    }
}

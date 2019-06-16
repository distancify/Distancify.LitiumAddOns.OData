﻿using AutoMapper;
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
    public static class IntOptionMappings
    {
        public static void MapFromIntOptionLabel<TSource, TDestination, TMember>(
            this IMemberConfigurationExpression<TSource, TDestination, TMember> config,
            string fieldId,
            CultureInfo culture)
            where TSource : ODataProductModel
        {
            config.MapFromIntOptionLabel(fieldId, culture.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="config"></param>
        /// <param name="fieldId"></param>
        /// <param name="culture">If null, will take culture from the mapping context (i.e use together with .MapWithCultureTo(...)</param>
        public static void MapFromIntOptionLabel<TSource, TDestination, TMember>(
        this IMemberConfigurationExpression<TSource, TDestination, TMember> config,
        string fieldId,
        string culture = null)
        where TSource : ODataProductModel
        {
            Func<ODataProductModel, TDestination, TMember, ResolutionContext, string> mapping = (model, destination, member, context) =>
            {
                var field = IoC.Resolve<FieldDefinitionService>().Get<ProductArea>(fieldId);
                var options = field.Option as IntOption;
                if (options == null)
                    return null;

                var value = model.GetValue<object>(fieldId);

                var result = new List<string>();
                if (value is IList<int> values)
                {
                    foreach (var i in values)
                    {
                        AddLabel(options, i, culture ?? context.GetMappingCulture(), result);
                    }
                }
                else
                {
                    AddLabel(options, (int)value, culture ?? context.GetMappingCulture(), result);
                }

                return string.Join(",", result);
            };
    
            config.MapFrom(mapping);
        }

        private static void AddLabel(IntOption options, int key, string culture, IList<string> result)
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

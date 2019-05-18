using AutoMapper;
using System.Globalization;

namespace Distancify.LitiumAddOns.OData.AutoMapperExtensions
{
    public static class GenericFieldMappings
    {
        public static void MapFromField<TSource, TDestination, TMember>(
            this IMemberConfigurationExpression<TSource, TDestination, TMember> config,
            string fieldId)
            where TSource : ODataProductModel
        {
            config.MapFrom((model, destination, member, context) => model.GetValue<TMember>(fieldId, context.GetMappingCulture()));
        }

        public static void MapFromField<TSource, TDestination, TMember>(
            this IMemberConfigurationExpression<TSource, TDestination, TMember> config,
            string fieldId,
            CultureInfo culture)
            where TSource : ODataProductModel
        {
            config.MapFrom(model => model.GetValue<TMember>(fieldId, culture));
        }

        public static void MapFromField<TSource, TDestination, TMember>(
            this IMemberConfigurationExpression<TSource, TDestination, TMember> config,
            string fieldId,
            string culture)
            where TSource : ODataProductModel
        {
            config.MapFrom(model => model.GetValue<TMember>(fieldId, CultureInfo.GetCultureInfo(culture)));
        }
    }
}

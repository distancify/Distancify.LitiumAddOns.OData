using AutoMapper;
using System.Globalization;

namespace Distancify.LitiumAddOns.OData
{
    public static class MemberConfigurationExpression
    {
        public static void MapFromField<TSource, TDestination, TMember>(this IMemberConfigurationExpression<TSource, TDestination, TMember> config, string fieldId)
            where TSource : ReadOnlyFieldFrameworkModel
        {
            config.MapFrom(model => model.GetValue<TMember>(fieldId));
        }

        public static void MapFromField<TSource, TDestination, TMember>(this IMemberConfigurationExpression<TSource, TDestination, TMember> config, string fieldId, CultureInfo culture)
            where TSource : ReadOnlyFieldFrameworkModel
        {
            config.MapFrom(model => model.GetValue<TMember>(fieldId, culture));
        }

        public static void MapFromField<TSource, TDestination, TMember>(this IMemberConfigurationExpression<TSource, TDestination, TMember> config, string fieldId, string culture)
            where TSource : ReadOnlyFieldFrameworkModel
        {
            config.MapFrom(model => model.GetValue<TMember>(fieldId, CultureInfo.GetCultureInfo(culture)));
        }
    }
}

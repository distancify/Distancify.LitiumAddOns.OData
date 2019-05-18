using AutoMapper;
using System.Globalization;

namespace Distancify.LitiumAddOns.OData.AutoMapperExtensions
{
    public static class Extensions
    {
        public static string GetMappingCulture(this ResolutionContext context)
        {
            return context.Items["culture"] as string ?? CultureInfo.CurrentUICulture.ToString();
        }

        /// <summary>
        /// Maps Litium fields using the specified culture where the mapping configuration does not set a specific culture
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static TResult MapWithCultureTo<TResult>(this object source, CultureInfo culture)
        {
            return (TResult)AutoMapper.Mapper.Map(source, source.GetType(), typeof(TResult), opt => opt.Items["culture"] = culture.ToString());
        }

        public static TResult MapWithCultureTo<TResult>(this object source, string culture)
        {
            return (TResult)AutoMapper.Mapper.Map(source, source.GetType(), typeof(TResult), opt => opt.Items["culture"] = culture);
        }
    }
}

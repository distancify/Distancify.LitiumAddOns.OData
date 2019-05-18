using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distancify.LitiumAddOns.OData.AutoMapperExtensions
{
    public static class PriceMappings
    {

        public static void MapFromPrice<TSource, TDestination, TMember>(
            this IMemberConfigurationExpression<TSource, TDestination, TMember> config,
            string listId)
            where TSource : ODataProductModel
        {
            config.MapFrom(model => model.GetPrice(listId));
        }
    }
}

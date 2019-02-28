using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distancify.LitiumAddOns.OData
{
    public static class MemberConfigurationExpression
    {
        public static void MapFromField<TSource, TDestination, TMember>(this IMemberConfigurationExpression<TSource, TDestination, TMember> config, string fieldId)
            where TSource : ReadOnlyFieldFrameworkModel
        {
            config.MapFrom(model => model.GetValue<TMember>(fieldId));
        }
    }
}

using Microsoft.AspNet.OData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Distancify.LitiumAddOns.OData.Controllers
{
    public abstract class LitiumODataController : ODataController
    {
        protected IHttpActionResult Ok(Type contentType, object content)
        {
            var method = GetOkMethodOf(contentType);
            return (IHttpActionResult)method.Invoke(this, new[] { content });
        }

        protected static object ToListOfType(IEnumerable<object> items, Type type)
        {
            var enumerableType = typeof(System.Linq.Enumerable);
            var castMethod = enumerableType.GetMethod(nameof(System.Linq.Enumerable.Cast)).MakeGenericMethod(type);
            var toListMethod = enumerableType.GetMethod(nameof(System.Linq.Enumerable.ToList)).MakeGenericMethod(type);

            var castedItems = castMethod.Invoke(null, new[] { items });

            return toListMethod.Invoke(null, new[] { castedItems });
        }

        private MethodInfo GetOkMethodOf(Type type)
        {
            return typeof(ODataController)
                .GetMethods(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.NonPublic)
                         .First(m => m.Name == nameof(Ok) && m.ContainsGenericParameters)
                         .MakeGenericMethod(type);
        }
    }
}

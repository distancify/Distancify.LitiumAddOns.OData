using Litium.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distancify.LitiumAddOns.OData
{
    public interface IProductModelBuilder
    {
        Type ModelType { get; }
        object Build(BaseProduct baseProduct, Variant variant);

        object ToTypedList(IEnumerable<object> list);
    }

    public abstract class ProductModelBuilder<T> : IProductModelBuilder
        where T : class
    {
        public Type ModelType { get { return typeof(T); } }

        public abstract object Build(BaseProduct baseProduct, Variant variant);

        public object AsQueryable(IEnumerable<object> list)
        {
            return list.OfType<T>().AsQueryable<T>();
        }

        public object ToTypedList(IEnumerable<object> list)
        {
            return list.OfType<T>().ToList();
        }
    }
}

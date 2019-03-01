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

        /// <summary>
        /// This method gets called for every matching variant in the system.
        /// </summary>
        /// <param name="productModel">A variant with it's connected base product</param>
        /// <returns>An object of or inherited from the type returned from ModelType</returns>
        object Build(ODataProductModel productModel);
    }

    public abstract class ProductModelBuilder<T> : IProductModelBuilder
        where T : class
    {
        public Type ModelType { get { return typeof(T); } }

        public abstract object Build(ODataProductModel productModel);

        public object AsQueryable(IEnumerable<object> list)
        {
            return list.OfType<T>().AsQueryable<T>();
        }
    }
}

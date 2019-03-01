using Distancify.LitiumAddOns.OData.Controllers;
using Litium.Data;
using Litium.Products;
using Litium.Products.Queryable;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using System.Collections.Generic;
using System.Web.Http;

namespace Distancify.LitiumAddOns.OData
{
    [EnableQuery]
    public class ODataProductsController : LitiumODataController
    {
        internal static IDictionary<string, IProductModelBuilder> Builders = new Dictionary<string, IProductModelBuilder>();

        private readonly DataService _dataService;
        private readonly VariantService _variantService;

        public ODataProductsController(DataService dataService, VariantService variantService)
        {
            _dataService = dataService;
            _variantService = variantService;
        }
        
        public IHttpActionResult Get()
        {
            var model = RequestContext.RouteData.Values["ODataModel"] as string;

            if (model == null || !Builders.TryGetValue(model, out var builder))
                return NotFound();

            using (var query = _dataService.CreateQuery<BaseProduct>(opt => opt.IncludeVariants()))
            {
                var hits = query.ToList();
                var result = ToListOfType(Map(hits, builder), builder.ModelType);
                return Ok(typeof(List<>).MakeGenericType(builder.ModelType), result);
            }
        }
        
        private IEnumerable<object> Map(IEnumerable<BaseProduct> baseProducts, IProductModelBuilder builder)
        {
            foreach (var b in baseProducts)
            {
                foreach (var v in _variantService.GetByBaseProduct(b.SystemId))
                {
                    yield return builder.Build(new ODataProductModel(b, v));
                }
            }
        }
    }
}
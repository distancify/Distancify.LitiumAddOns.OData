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
    public class ODataPrroductsController : LitiumODataController
    {
        internal static IProductModelBuilder Builder { get; set; }

        private readonly DataService _dataService;
        private readonly VariantService _variantService;

        public ODataPrroductsController(DataService dataService, VariantService variantService)
        {
            _dataService = dataService;
            _variantService = variantService;
        }

        [ODataRoute("Products")]
        public IHttpActionResult Get()
        {
            using (var query = _dataService.CreateQuery<BaseProduct>(opt => opt.IncludeVariants()))
            {
                var hits = query.ToList();
                var result = ToListOfType(Map(hits), Builder.ModelType);
                return Ok(typeof(List<>).MakeGenericType(Builder.ModelType), result);
            }
        }
        
        private IEnumerable<object> Map(IEnumerable<BaseProduct> baseProducts)
        {
            foreach (var b in baseProducts)
            {
                foreach (var v in _variantService.GetByBaseProduct(b.SystemId))
                {
                    yield return Builder.Build(b, v);
                }
            }
        }
    }
}
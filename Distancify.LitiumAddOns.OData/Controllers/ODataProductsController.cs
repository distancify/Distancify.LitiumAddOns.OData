using Distancify.LitiumAddOns.OData.Controllers;
using Distancify.LitiumAddOns.OData.Security;
using Litium.Data;
using Litium.Products;
using Litium.Products.Queryable;
using Microsoft.AspNet.OData;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Distancify.LitiumAddOns.OData
{
    [BasicAuthentication]
    [NeedsProductsContentAccess]
    [EnableQuery]
    public class ODataProductsController : LitiumODataController
    {
        internal static IDictionary<string, IProductModelBuilder> Builders = new Dictionary<string, IProductModelBuilder>();

        private readonly DataService dataService;
        private readonly VariantService variantService;
        private readonly PriceListItemService priceListItemService;
        private readonly PriceListService priceListService;

        public ODataProductsController(DataService dataService, VariantService variantService, 
            PriceListItemService priceListItemService,
            PriceListService priceListService)
        {
            this.dataService = dataService;
            this.variantService = variantService;
            this.priceListItemService = priceListItemService;
            this.priceListService = priceListService;
        }
        
        public IHttpActionResult Get()
        {
            var model = RequestContext.RouteData.Values["ODataModel"] as string;

            if (model == null || !Builders.TryGetValue(model, out var builder))
                return NotFound();

            using (var query = dataService.CreateQuery<BaseProduct>(opt => opt.IncludeVariants()))
            {
                var hits = query.ToList();
                var result = ToListOfType(Map(hits, builder), builder.ModelType);
                return Ok(typeof(List<>).MakeGenericType(builder.ModelType), result);
            }
        }
        
        private IEnumerable<object> Map(IEnumerable<BaseProduct> baseProducts, IProductModelBuilder builder)
        {
            return baseProducts
                .SelectMany(baseProduct => variantService.GetByBaseProduct(baseProduct.SystemId)
                    .Select(variant => MapVariant(baseProduct, variant))
                    .Select(r => builder.Build(r))
                    .Where(r => r != null));
        }

        private ODataProductModel MapVariant(BaseProduct baseProduct, Variant variant)
        {
            var prices = priceListItemService.GetByVariant(variant.SystemId).ToDictionary(p => priceListService.Get(p.PriceListSystemId).Id, r => r.Price);
            return new ODataProductModel(baseProduct, variant, prices);
        }
    }
}
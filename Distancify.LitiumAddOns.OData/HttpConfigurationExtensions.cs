using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing;
using Microsoft.OData.Edm;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Litium.Owin.InversionOfControl;
using Distancify.LitiumAddOns.Foundation;
using Distancify.LitiumAddOns.OData.demo;

namespace Distancify.LitiumAddOns.OData
{
    public static class HttpConfigurationExtensions
    {
        public class WebApiConfiguration
        {
            private readonly HttpConfiguration _webApiConfig;
            private readonly ODataConventionModelBuilder _odataBuilder = new ODataConventionModelBuilder();

            internal WebApiConfiguration(HttpConfiguration webApiConfiguration)
            {
                _webApiConfig = webApiConfiguration;
            }

            public WebApiConfiguration WithProductModelBuilder<TBuilder>(TBuilder builder)
                where TBuilder : IProductModelBuilder
            {
                ODataProductsController.Builder = builder;
                //EntityTypeConfiguration entity = _odataBuilder.AddEntityType(builder.ModelType);
                EntityTypeConfiguration entity = _odataBuilder.AddEntityType(builder.ModelType);
                _odataBuilder.AddEntitySet("ODataProducts", entity);
                
                return this;
            }

            public ODataRoute Create()
            {
                var dispatcher = new HttpRoutingDispatcher(_webApiConfig);
                var server = new HttpServer(_webApiConfig, dispatcher);

                return _webApiConfig.MapODataServiceRoute("litiumodata", "odata", GetEdmModel(), new DefaultODataBatchHandler(server));
            }

            private IEdmModel GetEdmModel()
            {
                _odataBuilder.Namespace = "LitiumOData";
                _odataBuilder.ContainerName = "DefaultContainer";
                var edmModel = _odataBuilder.GetEdmModel();
                return edmModel;
            }
        }

        public static WebApiConfiguration UseLitiumOData(this HttpConfiguration configuration)
        {
            return new WebApiConfiguration(configuration);
        }
    }
}

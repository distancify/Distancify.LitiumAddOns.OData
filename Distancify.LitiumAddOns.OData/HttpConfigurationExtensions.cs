using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing;
using Microsoft.OData.Edm;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.OData;
using System;
using Litium.Owin.InversionOfControl;
using Distancify.LitiumAddOns.Foundation;
using Litium.Runtime.DependencyInjection;

namespace Distancify.LitiumAddOns.OData
{
    public static class HttpConfigurationExtensions
    {
        public class WebApiConfiguration
        {
            private readonly HttpConfiguration _webApiConfig;
            private readonly ODataConventionModelBuilder _odataBuilder;

            internal WebApiConfiguration(HttpConfiguration webApiConfiguration)
            {
                _webApiConfig = webApiConfiguration;

                _odataBuilder = new ODataConventionModelBuilder();
                _odataBuilder.Namespace = "LitiumOData";
                _odataBuilder.ContainerName = "DefaultContainer";
            }

            public WebApiConfiguration WithProductModelBuilder<TBuilder>(TBuilder builder)
                where TBuilder : IProductModelBuilder
            {
                ODataPrroductsController.Builder = builder;
                EntityTypeConfiguration entity = _odataBuilder.AddEntityType(builder.ModelType);
                _odataBuilder.AddEntitySet("Products", entity);

                return this;
            }

            public ODataRoute Create()
            {
                var dispatcher = new HttpRoutingDispatcher(_webApiConfig);
                var server = new HttpServer(_webApiConfig, dispatcher);
                
                return _webApiConfig.MapODataServiceRoute("litiumodataservice", "odata", _odataBuilder.GetEdmModel());
            }
        }

        public static WebApiConfiguration UseLitiumOData(this HttpConfiguration configuration)
        {
            return new WebApiConfiguration(configuration);
        }
    }
}

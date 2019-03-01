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
using System.Collections.Generic;

namespace Distancify.LitiumAddOns.OData
{
    public static class HttpConfigurationExtensions
    {
        public class WebApiConfiguration
        {
            private readonly HttpConfiguration _webApiConfig;
            private readonly ODataConventionModelBuilder _odataBuilder;
            private readonly ISet<string> _productModels = new HashSet<string>();

            internal WebApiConfiguration(HttpConfiguration webApiConfiguration)
            {
                _webApiConfig = webApiConfiguration;

                _odataBuilder = new ODataConventionModelBuilder();
                _odataBuilder.Namespace = "LitiumOData";
                _odataBuilder.ContainerName = "DefaultContainer";
            }

            /// <summary>
            /// Registers a product model builder to provide a model for products
            /// </summary>
            /// <typeparam name="TBuilder"></typeparam>
            /// <param name="name">This is the odata endpoint name for this model. You will be able to access this model at /odata/&lt;name&gt</param>
            /// <param name="builder"></param>
            /// <returns></returns>
            public WebApiConfiguration WithProductModel<TBuilder>(string name, TBuilder builder)
                where TBuilder : IProductModelBuilder
            {
                ODataProductsController.Builders.Add(name, builder);
                EntityTypeConfiguration entity = _odataBuilder.AddEntityType(builder.ModelType);
                _odataBuilder.AddEntitySet(name, entity);
                _productModels.Add(name);

                return this;
            }

            public ODataRoute Create()
            {
                var conventions = ODataRoutingConventions.CreateDefaultWithAttributeRouting("litiumodataservice", _webApiConfig);
                conventions.Insert(0, new ControllerConvention(_productModels));

                return _webApiConfig.MapODataServiceRoute(
                    "litiumodataservice",
                    "odata",
                    _odataBuilder.GetEdmModel(),
                    new DefaultODataPathHandler(),
                    conventions);
            }
        }

        public static WebApiConfiguration UseLitiumOData(this HttpConfiguration configuration)
        {
            return new WebApiConfiguration(configuration);
        }
    }
}

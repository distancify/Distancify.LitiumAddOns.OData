using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.OData.Routing.Conventions;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace Distancify.LitiumAddOns.OData
{
    public class ControllerConvention : IODataRoutingConvention
    {
        private readonly ISet<string> _productModels;

        public ControllerConvention(ISet<string> productModels)
        {
            _productModels = productModels;
        }

        public string SelectAction(ODataPath odataPath, HttpControllerContext controllerContext, ILookup<string, HttpActionDescriptor> actionMap)
        {
            if (odataPath.NavigationSource == null)
                return null;
            if (_productModels.Contains(odataPath.NavigationSource.Name))
            {
                controllerContext.RouteData.Values.Add("ODataModel", odataPath.NavigationSource.Name);
            }

            return null;
        }

        public string SelectController(ODataPath odataPath, HttpRequestMessage request)
        {
            if (odataPath.NavigationSource == null)
                return "Metadata";
            if (_productModels.Contains(odataPath.NavigationSource.Name))
            {
                return "ODataProducts";
            }
            return null;
        }
    }
}

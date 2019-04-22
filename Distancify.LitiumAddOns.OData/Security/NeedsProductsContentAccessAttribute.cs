using Litium;
using Litium.Application.Security;
using Litium.Security;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Distancify.LitiumAddOns.OData.Security
{
    public class NeedsProductsContentAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            IoC.Resolve<PrincipalContextService>().SetCurrentPrincipal(actionContext.RequestContext.Principal);
            if (!IoC.Resolve<AuthorizationService>().HasOperation(Operations.Function.Products.Content))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "User lacks sufficient access rights");
            }
            
        }
    }
}

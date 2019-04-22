using Litium;
using Litium.Security;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Distancify.LitiumAddOns.OData.Security
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authHeader = actionContext.Request.Headers.Authorization;

            if (authHeader != null)
            {
                var token = actionContext.Request.Headers.Authorization.Parameter;
                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                var parts = decoded.Split(':');
                var username = parts[0];
                var password = parts[1];

                // PasswordSignIn() will set the principal of the current request
                var result = IoC.Resolve<AuthenticationService>().PasswordSignIn(username, password);
                
                switch (result)
                {
                    case AuthenticationResult.Failure:
                    case AuthenticationResult.LockedOut:
                        Unauthorized(actionContext);
                        return;
                    case AuthenticationResult.RequiresChangedPassword:
                    case AuthenticationResult.Success:
                        // We allow access even though password requires change, as there might be no
                        // obvious way for the user to change password when accessing OData endpoint.
                        actionContext.ControllerContext.RequestContext.Principal = HttpContext.Current.User;
                        return;
                }

            }
            else
            {
                Unauthorized(actionContext);
            }
            
        }

        private void Unauthorized(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", $"Basic Scheme='Data' location = '{actionContext.Request.RequestUri.GetLeftPart(UriPartial.Authority)}'");
        }
    }
}

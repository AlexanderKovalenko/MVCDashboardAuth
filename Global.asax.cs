using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using MVCDashboardAuth.Models;

namespace MVCDashboardAuth {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            DashboardConfig.RegisterService(RouteTable.Routes);
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder();

            DevExpress.Web.ASPxWebControl.CallbackError += Application_Error;
        }

        protected void Application_Error(object sender, EventArgs e) {
            Exception exception = System.Web.HttpContext.Current.Server.GetLastError();
            //TODO: Handle Exception
        }

        protected void Application_AuthorizeRequest(object sender, EventArgs e) {
            var application = (HttpApplication)sender;
            var request = application.Context.Request;
            var token = request.Headers["Authorization"]?.Replace("Bearer ", string.Empty);

            if (!string.IsNullOrEmpty(token)) {
                var tokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = AuthOptions.ISSUER,
                    ValidAudience = AuthOptions.AUDIENCE,
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey()
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                try {
                    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);

                    application.Context.User = new ClaimsPrincipal(principal);
                }
                catch (SecurityTokenValidationException ex) {
                    // Handle invalid token
                    application.Response.StatusCode = 401; // Unauthorized
                    application.Response.StatusDescription = ex.Message;
                    application.CompleteRequest();
                }
                catch (Exception ex) {
                    // Handle other exceptions
                    application.Response.StatusCode = 500; // Internal Server Error
                    application.Response.StatusDescription = ex.Message;
                    application.CompleteRequest();
                }
            }
        }
    }
}
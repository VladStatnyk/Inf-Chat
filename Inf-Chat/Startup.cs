using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Inf_Chat.Providers;
using System;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(Inf_Chat.Startup))]
namespace Inf_Chat
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            ConfigureOAuth(app);
            var routes = RouteTable.Routes;
            routes.MapRoute(
                "SPA",
                "{*catchall}",
                new { controller = "Home", action = "Index" }
                );

            WebApiConfig.Register(config);
            app.UseCors(CorsOptions.AllowAll);

            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);

                map.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions()
                {
                    Provider = new ApplicationOAuthBearerAuthenticationProvider()
                });

                var hubConfiguration = new HubConfiguration
                {
                    Resolver = GlobalHost.DependencyResolver,
                    EnableDetailedErrors = true
                };
                map.RunSignalR(hubConfiguration);
            });
            GlobalHost.HubPipeline.RequireAuthentication();

            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new AuthorizationServerProvider("self")
            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
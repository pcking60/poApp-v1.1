using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Owin;
using PostOffice.Model.Models;
using PostOffice.Service;
using PostOffice.Web.Infrastructure.Core;
using PostOfiice.DAta;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(PostOffice.Web.App_Start.Startup))]

namespace PostOffice.Web.App_Start
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(PostOfficeDbContext.Create);

            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<UserManager<ApplicationUser>>(CreateManager);

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/oauth/token"),
                Provider = new AuthorizationServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60),
                AllowInsecureHttp = true,
            });
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/dang-nhap.html"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }

        public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
        {
            public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
            {
                await Task.FromResult(context.Validated());
            }

            public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
            {
                var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

                if (allowedOrigin == null) allowedOrigin = "*";

                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

                UserManager<ApplicationUser> userManager = context.OwinContext.GetUserManager<UserManager<ApplicationUser>>();
                ApplicationUser user;
                try
                {
                    user = await userManager.FindAsync(context.UserName, context.Password);
                }
                catch
                {
                    // Could not retrieve the user due to error.
                   
                    return;
                }
                if (user != null)
                {
                    var applicationGroupService = ServiceFactory.Get<IApplicationGroupService>();
                    var listGroup = applicationGroupService.GetListGroupByUserId(user.Id);

                   
                    ClaimsIdentity identity = await userManager.CreateIdentityAsync(
                                                           user,
                                                           DefaultAuthenticationTypes.ExternalBearer);
                    identity.AddClaim(new Claim("fullName", user.FullName));
                    identity.AddClaim(new Claim("userId", user.Id));
                    identity.AddClaim(new Claim("permissions", JsonConvert.SerializeObject(listGroup)));
                    identity.AddClaim(new Claim("createdDate", user.CreatedDate.ToString()));
                    identity.AddClaim(new Claim("createdBy", user.CreatedBy.ToString()));
                    var props = new AuthenticationProperties(new Dictionary<string, string> {
                        { "fullName", user.FullName },
                        { "userId", user.Id },
                        { "permissions",JsonConvert.SerializeObject(listGroup)},
                        { "createdDate", user.CreatedDate.ToString()},
                        { "createdBy", user.CreatedBy}
                    });
                    context.Validated(new AuthenticationTicket(identity, props));
                }
                else
                {
                    try
                    {
                        user = await userManager.FindByNameAsync(context.UserName);
                        if (user == null)
                        {
                            context.SetError("invalid_grant", "Tài khoản không tồn tại");
                        }
                        else
                        {
                            context.SetError("invalid_grant", "Sai mật khẩu");
                        }
                        //context.Rejected();
                    }
                    catch
                    {
                        context.SetError("server_error");
                        context.Rejected();
                    }    
                    
                }
            }
            public override Task TokenEndpoint(OAuthTokenEndpointContext context)
            {
                foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
                {
                    context.AdditionalResponseParameters.Add(property.Key, property.Value);
                }
                return Task.FromResult<object>(null);
            }
        }

        private static UserManager<ApplicationUser> CreateManager(IdentityFactoryOptions<UserManager<ApplicationUser>> options, IOwinContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context.Get<PostOfficeDbContext>());
            var owinManager = new UserManager<ApplicationUser>(userStore);
            return owinManager;
        }
    }
}
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using static IdentityServer4.IdentityServerConstants;

namespace Chatter.UI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterIdentityServer(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = ProtocolTypes.OpenIdConnect;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(ProtocolTypes.OpenIdConnect, options =>
            {
                options.Authority = "https://localhost:44331/";
                options.RequireHttpsMetadata = false;

                options.ClientId = "Chatter.UI";
                options.ResponseType = TokenTypes.IdentityToken;
                options.SaveTokens = true;
            });
        }
    }
}

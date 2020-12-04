using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;

namespace WebShop.IdentityServer
{
    public static class ISConfiguration
    {
        public static IEnumerable<IdentityResource> IdentityResources { get; set; }
            = new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> Apis { get; set; }
            = new List<ApiResource>
            {
                new ApiResource(Constants.WebShopApiResource, new[]
                {
                    JwtClaimTypes.Role
                })
            };

        public static IEnumerable<Client> Clients { get; set; }
            = new List<Client>
            {
                new Client
                {
                    ClientId = "angular-client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = new List<string>
                    {
                        $"{Constants.NG_CLIENT_URL}",
                    },

                    PostLogoutRedirectUris =
                        new List<string> {$"{Constants.NG_CLIENT_URL}"},

                    RequirePkce = true,

                    AllowAccessTokensViaBrowser = true,

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        Constants.WebShopApiResource
                    },

                    AllowedCorsOrigins = {$"{Constants.NG_CLIENT_URL}"},

                    RequireClientSecret = false,

                    RequireConsent = false,
                },

                new Client
                {
                    ClientId = "client_id_mvc",
                    ClientSecrets = {new Secret("client_secret_mvc".ToSha256())},

                    AllowedGrantTypes = GrantTypes.Code,

                    AllowedScopes =
                    {
                        OidcConstants.StandardScopes.OpenId,
                        OidcConstants.StandardScopes.Profile,
                        Constants.WebShopApiResource
                    },

                    RedirectUris = {"https://localhost:5005/signin-oidc"},
                    PostLogoutRedirectUris = {"https://localhost:5005/Home/Index"},

                    AllowOfflineAccess = true,

                    RequireConsent = false,

                    // puts all the claims in the id token
                    // AlwaysIncludeUserClaimsInIdToken = true
                },

                new Client
                {
                    ClientId = "api_swagger",
                    ClientName = "Swagger UI for API",
                    ClientSecrets = {new Secret("secret_swagger_api_bla_bla".Sha256())},

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = {"https://localhost:5003/swagger/oauth2-redirect.html"},
                    PostLogoutRedirectUris = {"https://localhost:5003/swagger/index.html"},
                    AllowedCorsOrigins = {"https://localhost:5003"},

                    RequireConsent = false,

                    AllowedScopes =
                    {
                        OidcConstants.StandardScopes.OpenId,
                        OidcConstants.StandardScopes.Profile,
                        Constants.WebShopApiResource
                    },
                }
            };
    }


    public static class Constants
    {
        public static string WEB_HOST_URL = "https://localhost:5001/";
        public static string NG_CLIENT_URL = "http://localhost:4200";
        public static string WebShopApiResource = "WebShop.Api";
    }
}

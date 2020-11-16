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
                    ClientName = "Angular-Client",
                    ClientId = "angular-client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = new List<string>
                    {
                        $"{Constants.NG_CLIENT_URL}/signin-callback",
                        //$"{Constants.NG_CLIENT_URL}/assets/silent-callback.html",
                    },
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
                    PostLogoutRedirectUris =
                        new List<string> {$"{Constants.NG_CLIENT_URL}/signout-callback"},
                    RequireConsent = false,
                    AccessTokenLifetime = 1000,
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

                    AllowOfflineAccess = true,

                    RequireConsent = false,

                    // puts all the claims in the id token
                    // AlwaysIncludeUserClaimsInIdToken = true
                }
            };
    }


    public static class Constants
    {
        public static string WEB_HOST_URL = "https://localhost:5001/";
        public static string NG_CLIENT_URL = "https://localhost:5007";
        public static string WebShopApiResource = "WebShop.Api";
    }
}


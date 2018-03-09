using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace ConsulService
{
    public static class Config
    {
        public static IEnumerable<ApiResource>GetApiResources()
        {
            return new List<ApiResource>{
                new ApiResource("api","MyApi")
            };
        }

        public static IEnumerable<Client>GetClient()
        {
            return new List<Client>{
                new Client{
                    ClientId="ConsulClient",
                    ClientName="MVC Client",
                    ClientUri="http://localhost:5001",
                    AllowRememberConsent=true,
                    AllowedGrantTypes=GrantTypes.Implicit,
                    ClientSecrets={new Secret("secret".Sha256())},
                    RequireConsent=true,
                    RedirectUris={"http://localhost:5001/signin-oidc"},
                    PostLogoutRedirectUris={"http://localhost:5001/signout-callback-oidc"},
                    AllowedScopes={
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email
                    }
                }
            };
        }

        public static IEnumerable<TestUser>GetTestUsers()
        {
            return new List<TestUser>{
                new TestUser{
                    SubjectId="10000",
                    Username="Simen",
                    Password="123456"
                }
            };
        }

        public static IEnumerable<IdentityResource>GetIdentityResources()
        {
            return new List<IdentityResource>{
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }
    }
}
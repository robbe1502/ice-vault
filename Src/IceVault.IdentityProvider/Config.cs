using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using IdentityModel;

namespace IceVault.IdentityProvider
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>()
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new("profile-data", "Additional Data profile data", new [] { "locale", "time_zone", "currency", "full_name" })
        };

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>()
        {
            new ApiScope("ice-vault-web-api", "IceVault Web Api", new [] { "sub", "email", "full_name", "currency", "time_zone", "locale" })
        };

        public static IEnumerable<Client> Clients => new List<Client>()
        {
            new Client()
            {
                ClientId = "7E9C08BE-2DE9-4E85-9060-B1A1CC78559F",
                ClientSecrets = new List<Secret>() { new Secret("E922F97F-38FD-4687-B7E5-4665FD4532A4".ToSha256()) },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes =
                {
                    "ice-vault-web-api", 
                    IdentityServerConstants.StandardScopes.OpenId, 
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Profile,
                    "profile-data"
                },
                AllowOfflineAccess = true,
            }
        };

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>()
            {
                new TestUser()
                {
                    SubjectId = Guid.NewGuid().ToString(),
                    Username = "robbe.van.bael@hotmail.com",
                    Password = "0py$4GuMK",
                }
            };
        }
    }
}
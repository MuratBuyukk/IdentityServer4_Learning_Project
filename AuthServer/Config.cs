using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace AuthServer
{
    public static class Config
    {
        // Api scopes defined
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("Garanti.Write", "Garanti bank writing operation"),
                new ApiScope("Garanti.Read", "Garanti bank reading operation"),
                new ApiScope("HalkBank.Write", "HalkBank writing operation"),
                new ApiScope("HalkBank.Read", "HalkBank reading operation"),
            };
        }
        // Apis defined
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("Garanti")
                {
                    ApiSecrets =
                    {
                        new Secret("garanti".Sha256())
                    },
                    Scopes =
                    {
                        "Garanti.Write",
                        "Garanti.Read"
                    }
                },
                new ApiResource("HalkBank")
                {
                        ApiSecrets =
                    {
                        new Secret("halkbank".Sha256())
                    },
                    Scopes =
                    {
                        "HalkBank.Write",
                        "HalkBank.Read" }
                },
            };
        }
        // Clients defined to Apis
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "Garanti",
                    ClientName = "Garanti",
                    ClientSecrets = {new Secret("garanti".Sha256())},
                    AllowedGrantTypes = {GrantType.ClientCredentials},
                    AllowedScopes = {"Garanti.Write", "Garanti.Read"},
                },
                new Client
                {
                    ClientId = "HalkBank",
                    ClientName = "HalkBank",
                    ClientSecrets = { new Secret("halkbank".Sha256()) },
                    AllowedGrantTypes = { GrantType.ClientCredentials },
                    AllowedScopes = { "HalkBank.Write", "HalkBank.Read" }
                },
                new Client
                {
                    ClientId = "OnlineBanking",
                    ClientName = "OnlineBanking",
                    ClientSecrets = { new Secret("OnlineBanking".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowedScopes = 
                    { 
                        IdentityServerConstants.StandardScopes.OpenId, 
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess, 
                        "Garanti.Write", 
                        "Garanti.Read", 
                        "HalkBank.Write", 
                        "HalkBank.Read",
                        "PositionAndAuthority",
                        "Roles",
                        "Nickname"
                    },
                    RedirectUris = { "https://localhost:4000/signin-oidc" },
                    RequirePkce = false,
                    AccessTokenLifetime = 2 * 60 * 60,
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = 2 * 60 * 60 + 10 * 60,
                    RequireConsent = true,
                    PostLogoutRedirectUris = { "https://localhost:4000/signout-callback-oidc" }
                },
            };
        }

        public static IEnumerable<TestUser> GetTestUsers()
        {
            return new List<TestUser> {
                new TestUser {
                    SubjectId = "test-user1",
                    Username = "test-user1",
                    Password = "12345",
                    Claims = {
                        new Claim("name","test user1"),
                        new Claim("given_name", "test user1 given"),
                        new Claim("website","https://wwww.testuser1.com"),
                        new Claim("gender","1"),
                        new Claim("position", "Test User 1"),
                        new Claim("authority", "Test 1"),
                        new Claim("role", "admin")   
                    }
                },
                new TestUser {
                    SubjectId = "test-user2",
                    Username = "test-user2",
                    Password = "12345",
                    Claims = {
                        new Claim("name","test user2"),
                        new Claim("website","https://wwww.testuser2.com"),
                        new Claim("gender","0"),
                        new Claim("position" , "Test User 2"),
                        new Claim("authority", "Test 2"),
                        new Claim("role", "moderator")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "PositionAndAuthority",
                    DisplayName = "Position And Authority",
                    Description = "User position and authorization",
                    UserClaims = {"positon", "authority"}
                },
                new IdentityResource 
                {
                    Name = "Roles",
                    DisplayName = "Roles",
                    Description = "User roles",
                    UserClaims = { "role" }
                },
                new IdentityResource
                {
                    Name = "Nickname",
                    DisplayName = "Nickname",
                    Description = "User Nickname",
                    UserClaims = {"nickname" } 
                }
            };
        }
    }
}

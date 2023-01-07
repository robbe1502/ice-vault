using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace IceVault.WebApi.Test.Setup;

internal static class FakeJwtTokens
{
    private static readonly JwtSecurityTokenHandler Handler = new();

    static FakeJwtTokens()
    {
        var key = Encoding.UTF8.GetBytes("4BE5E1B9-B4E4-4CC1-8118-1B45F80DE28C");
        SecurityKey = new SymmetricSecurityKey(key);
        SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
    }

    internal static string GenerateToken(IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(Issuer, null, claims, null, DateTime.UtcNow.AddHours(1), SigningCredentials);
        return Handler.WriteToken(token);
    }
    
    internal static string Issuer => "CE75810F-A8EB-44F6-B9B2-784BF6DF9F97";

    internal static SecurityKey SecurityKey { get; }
    
    private static SigningCredentials SigningCredentials { get; }
}
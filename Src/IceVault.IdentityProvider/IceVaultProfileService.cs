using System.Security.Claims;
using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Models;
using IceVault.IdentityProvider.Entities;
using Microsoft.AspNetCore.Identity;

namespace IceVault.IdentityProvider;

public class IceVaultProfileService : ProfileService<ApplicationUser>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IceVaultProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory) 
        : base(userManager, claimsFactory)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public IceVaultProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, ILogger<ProfileService<ApplicationUser>> logger) 
        : base(userManager, claimsFactory, logger)
    {
    }


    protected override async Task GetProfileDataAsync(ProfileDataRequestContext context, ApplicationUser user)
    {
        var principal = await GetUserClaimsAsync(user);

        var identity = (ClaimsIdentity)principal.Identity;

        identity.AddClaim(new Claim("full_name", $"{user.FirstName} {user.LastName}"));
        identity.AddClaim(new Claim("email", user.Email));
        identity.AddClaim(new Claim("locale", user.Locale));
        identity.AddClaim(new Claim("time_zone", user.TimeZone));
        identity.AddClaim(new Claim("currency", user.Currency));

        context.AddRequestedClaims(principal.Claims);
    }
}
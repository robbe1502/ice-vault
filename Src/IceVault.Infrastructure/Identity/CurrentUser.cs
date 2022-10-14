using IceVault.Common.Identity;
using Microsoft.AspNetCore.Http;

namespace IceVault.Infrastructure.Identity;

internal class CurrentUser : ICurrentUser
{
    private readonly HttpContext _context;

    public CurrentUser(IHttpContextAccessor accessor)
    {
        _context = accessor.HttpContext;
    }

    public string Id => _context.User.Claims.SingleOrDefault(el => el.Type == IceVaultClaimConstant.Id)?.Value;
}
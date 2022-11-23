using IceVault.IdentityProvider.Entities;
using IceVault.IdentityProvider.Models.Demands;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IceVault.IdentityProvider.Controllers;

[ApiController, Route("api/v1/users")]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _manager;

    public UserController(UserManager<ApplicationUser> manager)
    {
        _manager = manager ?? throw new ArgumentNullException(nameof(manager));
    }

    [Route("")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDemand demand)
    {
        var exists = await _manager.Users.FirstOrDefaultAsync(el => el.Email == demand.Email);
        if (exists != null) return Ok(new {exists.Id});

        var id = Guid.NewGuid();
        var user = new ApplicationUser()
        {
            Id = id.ToString(),
            FirstName = demand.FirstName,
            LastName = demand.LastName,
            Email = demand.Email,
            UserName = demand.Email,
            Currency = demand.Currency,
            Locale = demand.Locale,
            TimeZone = demand.TimeZone
        };

        var identity = await _manager.CreateAsync(user, demand.Password);
        if (identity.Succeeded) return Ok(new { Id = id });

        return BadRequest(identity.Errors);
    }

    [Route("{id}")]
    public async Task<IActionResult> GetUserById([FromRoute] string id)
    {
        var exists = await _manager.Users.FirstOrDefaultAsync(el => el.Id == id);
        if (exists != null) return Ok(exists);

        return BadRequest($"User with {id} was not found.");
    }
}
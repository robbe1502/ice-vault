using IceVault.Application.Authentication.Login;
using IceVault.Common.Messaging;
using IceVault.Presentation.Authentication.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IceVault.Presentation.Authentication;

[Route("api/v1/auth"), AllowAnonymous]
public class AuthenticationController : ControllerBase
{
	private readonly ICommandBus _bus;
    private readonly IQueryDispatcher _dispatcher;

    public AuthenticationController(ICommandBus bus, IQueryDispatcher dispatcher)
    {
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
    }

	[HttpPost, Route("login")]
	public async Task<LoginResult> LoginAsync([FromBody] LoginRequest request)
    {
        var query = new LoginQuery(request.Email, request.Password);
        return await _dispatcher.Dispatch(query);
    }
}

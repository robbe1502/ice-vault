using System.Net;
using AutoMapper;
using IceVault.Application.Authentication.Login;
using IceVault.Application.Authentication.Profile;
using IceVault.Application.Authentication.Refresh;
using IceVault.Application.Authentication.Register;
using IceVault.Common.Messaging;
using IceVault.Presentation.Authentication.Models.Demands;
using IceVault.Presentation.Authentication.Models.Requests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IceVault.Presentation.Authentication;

[Route("api/v1/auth")]
public class AuthenticationController : ControllerBase
{
	private readonly ICommandBus _bus;
    private readonly IQueryDispatcher _dispatcher;
    private readonly IMapper _mapper;

    public AuthenticationController(ICommandBus bus, IQueryDispatcher dispatcher, IMapper mapper)
    {
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

	[HttpPost, Route("login"), AllowAnonymous]
	public async Task<LoginResult> LoginAsync([FromBody] LoginRequest request)
    {
        var query = new LoginQuery(request.Email, request.Password);
        return await _dispatcher.Dispatch(query);
    }

    [HttpPost, Route("register"), AllowAnonymous]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDemand demand)
    {
        var command = _mapper.Map<RegisterCommand>(demand);
        await _bus.Send(command);
        
        return StatusCode((int) HttpStatusCode.Created);
    }

    [HttpGet, Route("profile")]
    public async Task<ProfileInformationResult> GetProfileInformationAsync()
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        
        var query = new ProfileInformationQuery(token);
        return await _dispatcher.Dispatch(query);
    }

    [HttpGet, Route("refresh/{token}"), AllowAnonymous]
    public async Task<LoginResult> RefreshTokenAsync([FromRoute] string token)
    {
        var query = new RefreshTokenQuery(token);
        return await _dispatcher.Dispatch(query);
    }
}

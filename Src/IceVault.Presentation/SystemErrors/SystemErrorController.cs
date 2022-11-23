using IceVault.Application.SystemErrors.Entities;
using IceVault.Application.SystemErrors.Queries;
using IceVault.Common.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace IceVault.Presentation.SystemErrors;

[Route("api/v1/system-errors")]
public class SystemErrorController : ControllerBase
{
    private readonly IQueryDispatcher _dispatcher;

    public SystemErrorController(IQueryDispatcher dispatcher)
    {
        _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
    }

    [HttpGet]
    public async Task<List<SystemError>> GetSystemErrorsAsync()
    {
        var query = new GetAllSystemErrorsQuery();
        return await _dispatcher.Dispatch(query);
    }
}
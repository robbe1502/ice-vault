using IceVault.Application.SystemErrors.Entities;
using IceVault.Application.SystemErrors.Queries;
using IceVault.Application.SystemErrors.Repositories;
using IceVault.Common.Messaging;

namespace IceVault.Application.SystemErrors.QueryHandlers;

public class GetAllSystemErrorsQueryHandler : IQueryHandler<GetAllSystemErrorsQuery, List<SystemError>>
{
    private readonly ISystemErrorRepository _repository;

    public GetAllSystemErrorsQueryHandler(ISystemErrorRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    public async Task<List<SystemError>> HandleAsync(GetAllSystemErrorsQuery query)
    {
        return await _repository.GetSystemErrors();
    }
}
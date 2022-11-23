using IceVault.Application.SystemErrors.Entities;

namespace IceVault.Application.SystemErrors.Repositories;

public interface ISystemErrorRepository
{
    Task<List<SystemError>> GetSystemErrors();
}
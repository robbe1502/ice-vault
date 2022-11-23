using System.Linq.Expressions;
using IceVault.Common.Entities;

namespace IceVault.Application.Repositories;

public interface IRepositoryAsync<T> where T : class
{
    Task<List<T>> FindAll(CancellationToken token = default);
    
    Task<List<T>> FindAll(Expression<Func<T, bool>> predicate, CancellationToken token = default);

    Task<List<T>> FindAll(Expression<Func<T, bool>> predicate, int limit, CancellationToken token = default);

    Task<T> Get(Expression<Func<T, bool>> predicate, CancellationToken token = default);

    Task<T> Find(Expression<Func<T, bool>> predicate, CancellationToken token = default);

    Task InsertAsync(T entity, CancellationToken token = default);

    Task ModifyAsync(T entity, CancellationToken token = default);

    Task DeleteAsync(T entity, CancellationToken token = default);
}
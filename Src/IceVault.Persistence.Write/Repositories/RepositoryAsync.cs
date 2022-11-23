using System.Linq.Expressions;
using IceVault.Application.Repositories;
using IceVault.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace IceVault.Persistence.Write.Repositories;

public abstract class RepositoryAsync<T> : IRepositoryAsync<T> where T : Entity
{
    private readonly DbSet<T> _dbSet;

    protected RepositoryAsync(DbContext context)
    {
        _dbSet = context.Set<T>();
    }
    
    public async Task<List<T>> FindAll(CancellationToken token = default)
    {
        return await _dbSet.ToListAsync(token);
    }

    public async Task<List<T>> FindAll(Expression<Func<T, bool>> predicate, CancellationToken token = default)
    {
        return await _dbSet.Where(predicate).ToListAsync(token);
    }

    public async Task<List<T>> FindAll(Expression<Func<T, bool>> predicate, int limit, CancellationToken token = default)
    {
        return await _dbSet.Where(predicate).Take(limit).ToListAsync(token);
    }

    public async Task<T> Get(Expression<Func<T, bool>> predicate, CancellationToken token = default)
    {
        return await _dbSet.SingleAsync(predicate, token);
    }

    public async Task<T> Find(Expression<Func<T, bool>> predicate, CancellationToken token = default)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate, token);
    }

    public async Task InsertAsync(T entity, CancellationToken token = default)
    {
        await _dbSet.AddAsync(entity, token);
    }

    public Task ModifyAsync(T entity, CancellationToken token = default)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity, CancellationToken token = default)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }
}
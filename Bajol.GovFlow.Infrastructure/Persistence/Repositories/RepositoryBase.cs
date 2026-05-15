using System.Linq.Expressions;
using Bajol.GovFlow.Domain.Common;
using Bajol.GovFlow.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bajol.GovFlow.Infrastructure.Persistence.Repositories;

public abstract class RepositoryBase<TEntity, TId>(AppDbContext dbContext) : IRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : notnull
{
    protected readonly AppDbContext DbContext = dbContext;

    public virtual async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default) =>
        await DbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Id!.Equals(id), cancellationToken);

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) =>
        await DbContext.Set<TEntity>().AnyAsync(predicate, cancellationToken);

    public void Add(TEntity entity) => DbContext.Set<TEntity>().Add(entity);
}

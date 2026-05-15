using Bajol.GovFlow.Domain.Correspondences;
using Bajol.GovFlow.Domain.Repositories;

namespace Bajol.GovFlow.Infrastructure.Persistence.Repositories;

public sealed class CorrespondenceRepository(AppDbContext dbContext) : RepositoryBase<Correspondence, Guid>(dbContext), ICorrespondenceRepository
{
}

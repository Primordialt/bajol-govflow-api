using Bajol.GovFlow.Domain.Correspondences;
using Bajol.GovFlow.Domain.Repositories;

namespace Bajol.GovFlow.Infrastructure.Persistence.Repositories;

public sealed class CorrespondenceRepository(GovFlowDbContext dbContext) : RepositoryBase<Correspondence, Guid>(dbContext), ICorrespondenceRepository
{
}

using Momentum.BuildingBlocks.Application;
using Microsoft.EntityFrameworkCore;
using Momentum.BuildingBlocks.Infrastructure.Persistence;
using Momentum.Modules.UserAccess.Application.Authorization.GetUserPermissions;
using Momentum.Modules.UserAccess.Application.Configuration.Queries;

namespace Momentum.Modules.UserAccess.Application.Authorization.GetAuthenticatedUserPermissions
{
    internal class GetAuthenticatedUserPermissionsQueryHandler : IQueryHandler<GetAuthenticatedUserPermissionsQuery, List<UserPermissionDto>>
    {
        private readonly IMainRepository _mainRepository;

        private readonly IExecutionContextAccessor _executionContextAccessor;

        public GetAuthenticatedUserPermissionsQueryHandler(
            IMainRepository mainRepository,
            IExecutionContextAccessor executionContextAccessor)
        {
            _mainRepository = mainRepository;
            _executionContextAccessor = executionContextAccessor;
        }

        public async Task<List<UserPermissionDto>> Handle(GetAuthenticatedUserPermissionsQuery request, CancellationToken cancellationToken)
        {
            if (!_executionContextAccessor.IsAvailable)
            {
                return [];
            }

            return await _mainRepository
                .Set<UserPermissionDto>()
                .AsNoTracking()
                .Where(permission => permission.UserId == _executionContextAccessor.UserId)
                .ToListAsync(cancellationToken);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Momentum.BuildingBlocks.Infrastructure.Persistence;
using Momentum.Modules.UserAccess.Application.Configuration.Queries;

namespace Momentum.Modules.UserAccess.Application.Authorization.GetUserPermissions
{
    internal class GetUserPermissionsQueryHandler : IQueryHandler<GetUserPermissionsQuery, List<UserPermissionDto>>
    {
        private readonly IMainRepository _mainRepository;

        public GetUserPermissionsQueryHandler(IMainRepository mainRepository)
        {
            _mainRepository = mainRepository;
        }

        public async Task<List<UserPermissionDto>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
        {
            return await _mainRepository
                .Set<UserPermissionDto>()
                .AsNoTracking()
                .Where(permission => permission.UserId == request.UserId)
                .ToListAsync(cancellationToken);
        }
    }
}

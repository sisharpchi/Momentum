using Momentum.BuildingBlocks.Application;
using Microsoft.EntityFrameworkCore;
using Momentum.BuildingBlocks.Infrastructure.Persistence;
using Momentum.Modules.UserAccess.Application.Configuration.Queries;
using Momentum.Modules.UserAccess.Application.Users.GetUser;

namespace Momentum.Modules.UserAccess.Application.Users.GetAuthenticatedUser
{
    internal class GetAuthenticatedUserQueryHandler : IQueryHandler<GetAuthenticatedUserQuery, UserDto>
    {
        private readonly IMainRepository _mainRepository;

        private readonly IExecutionContextAccessor _executionContextAccessor;

        public GetAuthenticatedUserQueryHandler(
            IMainRepository mainRepository,
            IExecutionContextAccessor executionContextAccessor)
        {
            _mainRepository = mainRepository;
            _executionContextAccessor = executionContextAccessor;
        }

        public async Task<UserDto> Handle(GetAuthenticatedUserQuery request, CancellationToken cancellationToken)
        {
            return await _mainRepository
                .Set<UserDto>()
                .AsNoTracking()
                .SingleAsync(user => user.Id == _executionContextAccessor.UserId, cancellationToken);
        }
    }
}

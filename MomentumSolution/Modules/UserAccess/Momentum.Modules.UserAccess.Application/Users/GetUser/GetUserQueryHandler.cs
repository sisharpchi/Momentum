using Microsoft.EntityFrameworkCore;
using Momentum.BuildingBlocks.Infrastructure.Persistence;
using Momentum.Modules.UserAccess.Application.Configuration.Queries;

namespace Momentum.Modules.UserAccess.Application.Users.GetUser
{
    internal class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserDto>
    {
        private readonly IMainRepository _mainRepository;

        public GetUserQueryHandler(IMainRepository mainRepository)
        {
            _mainRepository = mainRepository;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return await _mainRepository
                .Set<UserDto>()
                .AsNoTracking()
                .SingleAsync(user => user.Id == request.UserId, cancellationToken);
        }
    }
}

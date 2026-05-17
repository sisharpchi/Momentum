using Microsoft.EntityFrameworkCore;
using Momentum.BuildingBlocks.Infrastructure.Persistence;
using Momentum.Modules.UserAccess.Application.Configuration.Queries;
using AuthenticateUserDto = Momentum.Modules.UserAccess.Application.Authentication.Authenticate.UserDto;

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
                .Set<AuthenticateUserDto>()
                .AsNoTracking()
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    IsActive = user.IsActive,
                    Name = user.Name,
                    Login = user.Login,
                    Email = user.Email,
                })
                .SingleAsync(user => user.Id == request.UserId, cancellationToken);
        }
    }
}

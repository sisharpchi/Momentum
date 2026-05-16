using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Momentum.BuildingBlocks.Infrastructure.Persistence;
using Momentum.Modules.UserAccess.Application.Configuration.Commands;
using Momentum.Modules.UserAccess.Application.Contracts;

namespace Momentum.Modules.UserAccess.Application.Authentication.Authenticate
{
    internal class AuthenticateCommandHandler : ICommandHandler<AuthenticateCommand, AuthenticationResult>
    {
        private readonly IMainRepository _mainRepository;

        internal AuthenticateCommandHandler(IMainRepository mainRepository)
        {
            _mainRepository = mainRepository;
        }

        public async Task<AuthenticationResult> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            var user = await _mainRepository
                .Set<UserDto>()
                .AsNoTracking()
                .SingleOrDefaultAsync(user => user.Login == request.Login, cancellationToken);

            if (user == null)
            {
                return new AuthenticationResult("Incorrect login or password");
            }

            if (!user.IsActive)
            {
                return new AuthenticationResult("User is not active");
            }

            if (!PasswordManager.VerifyHashedPassword(user.Password, request.Password))
            {
                return new AuthenticationResult("Incorrect login or password");
            }

            user.Claims =
            [
                new Claim(CustomClaimTypes.Name, user.Name),
                new Claim(CustomClaimTypes.Email, user.Email)
            ];

            return new AuthenticationResult(user);
        }
    }
}

using Momentum.Modules.UserAccess.Application.Configuration.Commands;
using Momentum.BuildingBlocks.Infrastructure.Persistence;
using Momentum.Modules.UserAccess.Domain.Users;

namespace Momentum.Modules.UserAccess.Application.Users.CreateUser;

internal class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
{
    private readonly IMainRepository _mainRepository;

    public CreateUserCommandHandler(IMainRepository mainRepository)
    {
        _mainRepository = mainRepository;
    }

    public async Task Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var user = User.CreateUser(
            command.UserId,
            command.Login,
            command.Password,
            command.Email,
            command.FirstName,
            command.LastName);

        await _mainRepository.Set<User>().AddAsync(user, cancellationToken);
    }
}

using Momentum.BuildingBlocks.Infrastructure.Persistence;
using Momentum.Modules.Registrations.Application.UserRegistrations.GetUserRegistration;
using MediatR;

namespace Momentum.Modules.Registrations.Application.UserRegistrations.ConfirmUserRegistration;

public class UserRegistrationConfirmedNotificationHandler : INotificationHandler<UserRegistrationConfirmedNotification>
{
    private readonly IUserCreator _userCreator;

    private readonly IMainRepository _mainRepository;

    public UserRegistrationConfirmedNotificationHandler(IUserCreator userCreator, IMainRepository mainRepository)
    {
        _userCreator = userCreator;
        _mainRepository = mainRepository;
    }

    public async Task Handle(UserRegistrationConfirmedNotification notification, CancellationToken cancellationToken)
    {
        var registration = await UserRegistrationProvider.GetById(
            _mainRepository,
            notification.DomainEvent.UserRegistrationId.Value,
            cancellationToken);

        await _userCreator.Create(
            registration.Id,
            registration.Login,
            registration.Password,
            registration.Email,
            registration.FirstName,
            registration.LastName);
    }
}

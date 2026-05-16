using Microsoft.EntityFrameworkCore;
using Momentum.BuildingBlocks.Infrastructure.Persistence;
using Momentum.Modules.Registrations.Domain.UserRegistrations;

namespace Momentum.Modules.Registrations.Application.UserRegistrations.GetUserRegistration;

internal static class UserRegistrationProvider
{
    internal static async Task<UserRegistrationDto> GetById(
        IMainRepository mainRepository,
        Guid userRegistrationId,
        CancellationToken cancellationToken)
    {
        return await mainRepository.Query<UserRegistration>()
            .Where(userRegistration => userRegistration.Id == new UserRegistrationId(userRegistrationId))
            .Select(userRegistration => new UserRegistrationDto
            {
                Id = userRegistration.Id.Value,
                Login = EF.Property<string>(userRegistration, "_login"),
                Email = EF.Property<string>(userRegistration, "_email"),
                FirstName = EF.Property<string>(userRegistration, "_firstName"),
                LastName = EF.Property<string>(userRegistration, "_lastName"),
                Name = EF.Property<string>(userRegistration, "_name"),
                StatusCode = EF.Property<UserRegistrationStatus>(userRegistration, "_status").Value,
                Password = EF.Property<string>(userRegistration, "_password")
            })
            .SingleAsync(cancellationToken);
    }
}

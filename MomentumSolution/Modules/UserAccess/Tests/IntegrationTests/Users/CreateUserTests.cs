using Microsoft.EntityFrameworkCore;
using Momentum.Modules.UserAccess.Application.Users.CreateUser;
using Momentum.Modules.UserAccess.Application.Users.GetUser;
using Momentum.Modules.UserAccess.Domain.Users;
using Momentum.Modules.UserAccess.IntegrationTests.SeedWork;

namespace Momentum.Modules.UserAccess.IntegrationTests.Users;

public class CreateUserTests : TestBase
{
    [Fact]
    public async Task CreateUserTest()
    {
        await using var context = await CreateContextAsync();
        var command = new CreateUserCommand(
            Guid.NewGuid(),
            UserSampleData.Login,
            UserSampleData.Email,
            UserSampleData.FirstName,
            UserSampleData.LastName,
            UserSampleData.Password);

        await AddUserAsync(
            context,
            command.UserId,
            command.Login,
            command.Email,
            command.FirstName,
            command.LastName,
            command.Password);

        context.ChangeTracker.Clear();

        var user = await context.Users
            .AsNoTracking()
            .Select(savedUser => new UserDto
            {
                Id = savedUser.Id.Value,
                IsActive = EF.Property<bool>(savedUser, "_isActive"),
                Name = EF.Property<string>(savedUser, "_name"),
                Login = EF.Property<string>(savedUser, "_login"),
                Email = EF.Property<string>(savedUser, "_email"),
            })
            .SingleAsync(savedUser => savedUser.Id == command.UserId);

        Assert.Equal(UserSampleData.Login, user.Login);
        Assert.Equal(UserSampleData.Email, user.Email);
        Assert.Equal($"{UserSampleData.FirstName} {UserSampleData.LastName}", user.Name);
    }
}

public static class UserSampleData
{
    public static string Login => "jdoe";

    public static string Email => "jdoe@mail.com";

    public static string FirstName => "John";

    public static string LastName => "Doe";

    public static string Password => "qwerty";
}

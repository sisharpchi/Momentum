using Momentum.Modules.UserAccess.Domain.Users;
using Momentum.Modules.UserAccess.Domain.Users.Events;
using Momentum.Modules.UserAccess.UnitTests.SeedWork;

namespace Momentum.Modules.UserAccess.UnitTests.Users;

public class UserTests : TestBase
{
    [Fact]
    public void CreateUserPublishesUserCreatedDomainEvent()
    {
        var userId = Guid.NewGuid();

        var user = User.CreateUser(userId, "jdoe", "password", "jdoe@mail.com", "John", "Doe");

        var domainEvent = AssertPublishedDomainEvent<UserCreatedDomainEvent>(user);
        Assert.Equal(new UserId(userId), domainEvent.Id);
    }
}

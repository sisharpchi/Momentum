using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Momentum.BuildingBlocks.Infrastructure;
using Momentum.Modules.UserAccess.Domain.Users;
using Momentum.Modules.UserAccess.Infrastructure;
using Momentum.Modules.UserAccess.Infrastructure.Domain.Users;
using Momentum.Modules.UserAccess.IntegrationTests.SeedWork;

namespace Momentum.Modules.UserAccess.IntegrationTests.Users;

public class UserRepositoryTests : TestBase
{
    [Fact]
    public async Task RepositoryPersistsUser()
    {
        await using var context = await CreateContextAsync();
        var repository = CreateRepository(context);
        var userId = Guid.NewGuid();

        await repository.Set<User>().AddAsync(User.CreateUser(userId, "jdoe", "password", "jdoe@mail.com", "John", "Doe"));
        await CommitAsync(context);
        context.ChangeTracker.Clear();

        var user = await repository.Set<User>().SingleAsync(x => x.Id == new UserId(userId));

        Assert.Equal(new UserId(userId), user.Id);
    }
}

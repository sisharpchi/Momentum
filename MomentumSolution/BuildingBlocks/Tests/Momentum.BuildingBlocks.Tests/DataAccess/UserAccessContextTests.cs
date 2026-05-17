using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging.Abstractions;
using Momentum.BuildingBlocks.Infrastructure;
using Momentum.Modules.UserAccess.Infrastructure;

namespace Momentum.BuildingBlocks.Tests.DataAccess;

public class UserAccessContextTests
{
    [Fact]
    public void Model_can_be_created_with_user_view_mappings()
    {
        var options = new DbContextOptionsBuilder<UserAccessContext>()
            .UseNpgsql("Host=127.0.0.1;Port=5432;Database=momentum_test;Username=test;Password=test")
            .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>()
            .Options;

        using var context = new UserAccessContext(options, NullLoggerFactory.Instance);

        var model = context.Model;

        Assert.NotNull(model);
    }
}

using Microsoft.EntityFrameworkCore;
using Momentum.BuildingBlocks.Application.Emails;
using Momentum.BuildingBlocks.Infrastructure.Emails;

namespace Momentum.BuildingBlocks.Tests.Emails;

public class EmailSenderTests
{
    [Fact]
    public async Task SendEmail_stores_email_using_ef_core()
    {
        var options = new DbContextOptionsBuilder<TestEmailsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var dbContext = new TestEmailsDbContext(options);
        var sender = new EmailSender(
            Serilog.Core.Logger.None,
            new EmailsConfiguration("from@momentum.test"),
            dbContext);

        await sender.SendEmail(new EmailMessage(
            "user@momentum.test",
            "Welcome",
            "Hello from Momentum"));

        var email = await dbContext.Emails.SingleAsync();

        Assert.Equal("from@momentum.test", email.From);
        Assert.Equal("user@momentum.test", email.To);
        Assert.Equal("Welcome", email.Subject);
        Assert.Equal("Hello from Momentum", email.Content);
        Assert.NotEqual(Guid.Empty, email.Id);
    }

    private sealed class TestEmailsDbContext : DbContext, IEmailsDbContext
    {
        public TestEmailsDbContext(DbContextOptions<TestEmailsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Email> Emails => Set<Email>();
    }
}

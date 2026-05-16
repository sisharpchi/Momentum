using Microsoft.EntityFrameworkCore;

namespace Momentum.BuildingBlocks.Infrastructure.Emails
{
    public interface IEmailsDbContext
    {
        DbSet<Email> Emails { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

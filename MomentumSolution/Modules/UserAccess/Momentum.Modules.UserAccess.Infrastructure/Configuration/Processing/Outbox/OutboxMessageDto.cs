namespace Momentum.Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox
{
    public class OutboxMessageDto
    {
        public Guid Id { get; set; }

        public string Type { get; set; } = null!;

        public string Data { get; set; } = null!;
    }
}

namespace Momentum.Modules.UserAccess.Infrastructure.Configuration.Processing.Inbox
{
    public class InboxMessageDto
    {
        public Guid Id { get; set; }

        public string Type { get; set; } = null!;

        public string Data { get; set; } = null!;
    }
}

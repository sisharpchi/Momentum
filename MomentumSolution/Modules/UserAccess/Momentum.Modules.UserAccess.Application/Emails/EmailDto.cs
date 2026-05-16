namespace Momentum.Modules.UserAccess.Application.Emails
{
    public class EmailDto
    {
        public Guid Id { get; set; }

        public string From { get; set; } = null!;

        public string To { get; set; } = null!;

        public string Subject { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime Date { get; set; }
    }
}

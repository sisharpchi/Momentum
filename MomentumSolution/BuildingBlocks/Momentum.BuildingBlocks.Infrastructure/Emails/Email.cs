namespace Momentum.BuildingBlocks.Infrastructure.Emails
{
    public class Email
    {
        private Email()
        {
            From = null!;
            To = null!;
            Subject = null!;
            Content = null!;
        }

        public Email(
            string from,
            string to,
            string subject,
            string content)
        {
            Id = Guid.NewGuid();
            From = from;
            To = to;
            Subject = subject;
            Content = content;
            Date = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }

        public string From { get; private set; }

        public string To { get; private set; }

        public string Subject { get; private set; }

        public string Content { get; private set; }

        public DateTime Date { get; private set; }
    }
}

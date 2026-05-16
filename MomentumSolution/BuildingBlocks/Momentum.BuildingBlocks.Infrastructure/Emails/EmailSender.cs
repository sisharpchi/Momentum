using Momentum.BuildingBlocks.Application.Emails;
using Serilog;

namespace Momentum.BuildingBlocks.Infrastructure.Emails
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;

        private readonly EmailsConfiguration _configuration;

        private readonly IEmailsDbContext _dbContext;

        public EmailSender(
            ILogger logger,
            EmailsConfiguration configuration,
            IEmailsDbContext dbContext)
        {
            _logger = logger;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task SendEmail(EmailMessage message)
        {
            var email = new Email(
                _configuration.FromEmail,
                message.To,
                message.Subject,
                message.Content);

            _dbContext.Emails.Add(email);

            await _dbContext.SaveChangesAsync();

            _logger.Information(
                "Email sent. From: {From}, To: {To}, Subject: {Subject}, Content: {Content}.",
                _configuration.FromEmail,
                message.To,
                message.Subject,
                message.Content);
        }
    }
}

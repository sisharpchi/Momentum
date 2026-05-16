namespace Momentum.Modules.UserAccess.Domain.Inbox;

public class InboxMessage
{
    public Guid Id { get; private set; }

    public DateTime OccurredOn { get; private set; }

    public string Type { get; private set; }

    public string Data { get; private set; }

    public DateTime? ProcessedDate { get; private set; }

    public InboxMessage(Guid id, DateTime occurredOn, string type, string data)
    {
        Id = id;
        OccurredOn = occurredOn;
        Type = type;
        Data = data;
    }

    public void MarkAsProcessed(DateTime processedDate)
    {
        ProcessedDate = processedDate;
    }

    private InboxMessage()
    {
        Type = default!;
        Data = default!;
    }
}

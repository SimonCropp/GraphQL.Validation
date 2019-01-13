using NServiceBus;

public class MessageWithValidator : IMessage
{
    public string Content { get; set; }
}
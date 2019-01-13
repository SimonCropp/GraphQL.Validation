using NServiceBus;

public class MessageWithNoValidator : IMessage
{
    public string Content { get; set; }
}
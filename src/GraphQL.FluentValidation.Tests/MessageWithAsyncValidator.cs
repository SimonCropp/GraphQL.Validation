using NServiceBus;

public class MessageWithAsyncValidator : IMessage
{
    public string Content { get; set; }
}
using System.ComponentModel.DataAnnotations;
using NServiceBus;

public class MessageWithValidator : IMessage
{
    [Required]
    public string Content { get; set; }
}
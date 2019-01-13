using System.ComponentModel.DataAnnotations;
using NServiceBus;

public class MyMessage : IMessage
{
    [Required]
    public string Content { get; set; }
}
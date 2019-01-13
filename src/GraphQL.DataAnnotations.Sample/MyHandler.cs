﻿using System;
using System.Threading.Tasks;
using NServiceBus;

class MyHandler :
    IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine("Hello from MyHandler. MyMessage");
        return Task.FromResult(0);
    }
}
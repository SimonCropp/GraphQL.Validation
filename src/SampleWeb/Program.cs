using Microsoft.AspNetCore;
#pragma warning disable ASPDEPR008

WebHost.CreateDefaultBuilder()
    .UseStartup<Startup>()
    .Build().Run();
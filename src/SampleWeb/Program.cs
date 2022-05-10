using Microsoft.AspNetCore;

WebHost.CreateDefaultBuilder()
    .UseStartup<Startup>()
    .Build().Run();
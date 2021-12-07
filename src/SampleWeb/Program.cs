using Microsoft.AspNetCore;

var webHostBuilder = WebHost.CreateDefaultBuilder();
var hostBuilder = webHostBuilder.UseStartup<Startup>();
hostBuilder.Build().Run();
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

var webHostBuilder = WebHost.CreateDefaultBuilder();
var hostBuilder = webHostBuilder.UseStartup<Startup>();
hostBuilder.Build().Run();

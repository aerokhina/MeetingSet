using MeetingSet.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MeetingSet
{
  public class Program
  {
    public static void Main(string[] args)
    {
        var builder = CreateHostBuilder(args);
        var host = builder.Build();
        MigrateDatabase(host);
        host.Run();
    }

      public static IHostBuilder CreateHostBuilder(string[] args)
      {
        return Host.CreateDefaultBuilder(args)
          .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
      }
    
    public static void MigrateDatabase(IHost host)
    {
      using (var scope = host.Services.CreateScope())
      {
        var applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        applicationContext.Database.Migrate();
      }
    }
  }
}

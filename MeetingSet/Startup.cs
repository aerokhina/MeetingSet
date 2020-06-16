using MeetingSet.Data;
using MeetingSet.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MeetingSet
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      
      services.AddDbContext<ApplicationContext>(
        options =>
          options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")
            ));
      
      services.AddHostedService<MeetingNotificationBackgroundService>();

      services.AddSingleton<IEmailService, EmailService>();
      
      services.Configure<MeetingNotificationConfiguration>(Configuration.GetSection("MeetingNotification"));

      services.AddScoped<MeetingNotificationService>();
      
      services.Configure<EmailConfiguration>(Configuration.GetSection("Email"));

      services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      
      app.UseRouting();

      app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
  }
}

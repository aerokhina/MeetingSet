using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MeetingSet.Data;
using MeetingSet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace MeetingSet.Services
{
  public class MeetingNotificationBackgroundService : BackgroundService
  {
    private readonly IServiceProvider _services;
    private readonly NotificationConfiguration _notificationConfiguration;

    private bool _isWorking = true;

    public MeetingNotificationBackgroundService(
      IServiceProvider services,
      IOptions<NotificationConfiguration> notificationConfiguration
    )
    {
      _services = services;
      _notificationConfiguration = notificationConfiguration.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (_isWorking)
      {
        using (var scope = _services.CreateScope())
        {
          var meetingNotificationService = scope.ServiceProvider.GetRequiredService<MeetingNotificationService>();
          await meetingNotificationService.NotifyParticipantsMeeting();
        }
      }
      await Task.Delay(_notificationConfiguration.PollingDelaySeconds*1000, stoppingToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
      _isWorking = false;
      return base.StopAsync(cancellationToken);
    }
  }
}

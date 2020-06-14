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

namespace MeetingSet.Services
{
  public class NotificationService : BackgroundService
  {
    private readonly IServiceProvider _services;
    private readonly EmailService _emailService;

    private bool IsWorking = true;

    public NotificationService(IServiceProvider services, EmailService emailService)
    {
      _services = services;
      _emailService = emailService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (IsWorking)
      {
        var dateTimeNow = DateTime.Now;
        var dateTimePeriodDelay = dateTimeNow.AddMinutes(15.0);
        using var scope = _services.CreateScope();
        var applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

        var meetings = await applicationContext.Meetings
          .Where(
            m => m.StartDateTimeMeeting > dateTimeNow && m.StartDateTimeMeeting <= dateTimePeriodDelay && !m.IsNotified
          )
          .Include(p => p.MeetingParticipants)
          .ThenInclude(p => p.Participant)
          .ToListAsync();

        foreach (var meeting in meetings)
        {
            foreach (var meetingParticipant in meeting.MeetingParticipants)
            {
              await _emailService.SendEmailAsync(meetingParticipant.Participant.Email, meeting);
            }
            meeting.IsNotified = true;
            await applicationContext.SaveChangesAsync();
        }
        await Task.Delay(3000, stoppingToken);
      }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
      IsWorking = false;
      return base.StopAsync(cancellationToken);
    }
  }
}

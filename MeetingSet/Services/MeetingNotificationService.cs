using System;
using System.Linq;
using System.Threading.Tasks;
using MeetingSet.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MeetingSet.Services
{
  public class MeetingNotificationService
  {
    private readonly NotificationConfiguration _notificationConfiguration;

    private readonly ApplicationContext _context;

    private readonly EmailService _emailService;

    public MeetingNotificationService(
      ApplicationContext context,
      IOptions<NotificationConfiguration> notificationConfiguration,
      EmailService emailService
    )
    {
      _context = context;
      _notificationConfiguration = notificationConfiguration.Value;
      _emailService = emailService;
    }

    public async Task NotifyParticipantsMeeting()
    {
      var dateTimeNow = DateTime.Now;
      var dateTimePeriodDelay = dateTimeNow.AddMinutes(_notificationConfiguration.MeetingDelayMinutes);

      var meetings = await _context.Meetings
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
        await _context.SaveChangesAsync();
      }
    }
  }
}

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
    private readonly MeetingNotificationConfiguration _meetingNotificationConfiguration;

    private readonly ApplicationContext _context;

    private readonly IEmailService _emailService;

    public MeetingNotificationService(
      ApplicationContext context,
      IOptions<MeetingNotificationConfiguration> notificationConfiguration,
      IEmailService emailService
    )
    {
      _context = context;
      _meetingNotificationConfiguration = notificationConfiguration.Value;
      _emailService = emailService;
    }

    public async Task NotifyParticipantsMeeting()
    {
      var dateTimeNow = DateTime.Now;
      var dateTimePeriodDelay = dateTimeNow.AddMinutes(_meetingNotificationConfiguration.MeetingDelayMinutes);

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

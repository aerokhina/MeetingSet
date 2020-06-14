using System;
using MeetingSet.Data;
using MeetingSet.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace MeetingSet.Tests
{
  public class MeetingNotificztionServiceTests
  {
    private readonly ApplicationContext _context;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

    public MeetingNotificztionServiceTests()
    {
      var builder = new DbContextOptionsBuilder<ApplicationContext>();
      builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
      var options = builder.Options;
      _context = new ApplicationContext(options);
      _emailServiceMock = new Mock<IEmailService>();
      _dateTimeProviderMock = new Mock<IDateTimeProvider>();
    }

    [Fact]
    public async void NotifyParticipantsMeeting_Success()
    {
      var email = "sa@as.rt";
      var idMeeting = 1;
      var idParticipant = 1;
      var startDateTimeMeeting = new DateTime(2020, 06, 12, 21, 45, 00);
      var now = new DateTime(2020, 06, 12, 21, 30, 00);
      
      _context.Meetings.Add(
        new Meeting
        {
          Id = idMeeting,
          Name = "meeting",
          StartDateTimeMeeting = startDateTimeMeeting,
          EndDateTimeMeeting = new DateTime(2020, 06, 12, 23, 30, 00),
          IsNotified = false
        }
      );

      _context.Participants.Add(
        new Participant
        {
          Id = idParticipant,
          Name = "Sasha",
          Email = email
        }
      );

      _context.MeetingParticipants.Add(
        new MeetingParticipant()
        {
          MeetingId = idMeeting,
          ParticipantId = idParticipant
        }
      );

      _context.SaveChanges();

      _dateTimeProviderMock.Setup(x => x.Now()).Returns(now);

      var meetingNotificationConfiguration = new MeetingNotificationConfiguration()
      {
        MeetingDelayMinutes = 15
      };

      var meeting = await _context.Meetings.FindAsync(idMeeting);

      var meetingNotificationService = new MeetingNotificationService(
        _context, new OptionsWrapper<MeetingNotificationConfiguration>(meetingNotificationConfiguration),
        _emailServiceMock.Object, _dateTimeProviderMock.Object
      );

      await meetingNotificationService.NotifyParticipantsMeeting();
      
      _emailServiceMock.Verify(send => send.SendEmailAsync(email, meeting), Times.Once);

      Assert.True(meeting.IsNotified);
    }

    [Fact]
    public async void NotifyParticipantsMeeting_NotSendEmailWhenStartDateIsNotAtThePeriod()
    {
      var email = "sa@as.rt";
      var idMeeting = 1;
      var idParticipant = 1;
      var startDateTimeMeeting = new DateTime(2020, 06, 12, 22, 44, 00);
      var now = new DateTime(2020, 06, 12, 21, 30, 00);
      
      _context.Meetings.Add(
        new Meeting
        {
          Id = idMeeting,
          Name = "meeting",
          StartDateTimeMeeting = startDateTimeMeeting,
          EndDateTimeMeeting = new DateTime(2020, 06, 12, 23, 30, 00),
          IsNotified = false
        }
      );

      _context.Participants.Add(
        new Participant
        {
          Id = idParticipant,
          Name = "Sasha",
          Email = email
        }
      );

      _context.MeetingParticipants.Add(
        new MeetingParticipant()
        {
          MeetingId = idMeeting,
          ParticipantId = idParticipant
        }
      );

      _context.SaveChanges();
      
      _dateTimeProviderMock.Setup(x => x.Now()).Returns(now);

      var meetingNotificationConfiguration = new MeetingNotificationConfiguration()
      {
        MeetingDelayMinutes = 15
      };

      var meeting = await _context.Meetings.FindAsync(idMeeting);

      var meetingNotificationService = new MeetingNotificationService(
        _context, new OptionsWrapper<MeetingNotificationConfiguration>(meetingNotificationConfiguration),
        _emailServiceMock.Object, _dateTimeProviderMock.Object
      );

      await meetingNotificationService.NotifyParticipantsMeeting();
      
      _emailServiceMock.Verify(send => send.SendEmailAsync(email, meeting), Times.Never);

      Assert.False(meeting.IsNotified);
    }

    [Fact]
    public async void NotifyParticipantsMeeting_NotSendEmailWhenMeetingWasNotified()
    {
      var email = "sa@as.rt";
      var idMeeting = 1;
      var idParticipant = 1;
      var startDateTimeMeeting = new DateTime(2020, 06, 12, 21, 45, 00);
      var now = new DateTime(2020, 06, 12, 21, 30, 00);
      
      _context.Meetings.Add(
        new Meeting
        {
          Id = idMeeting,
          Name = "meeting",
          StartDateTimeMeeting = startDateTimeMeeting,
          EndDateTimeMeeting = new DateTime(2020, 06, 12, 23, 30, 00),
          IsNotified = true
        }
      );

      _context.Participants.Add(
        new Participant
        {
          Id = idParticipant,
          Name = "Sasha",
          Email = email
        }
      );

      _context.MeetingParticipants.Add(
        new MeetingParticipant()
        {
          MeetingId = idMeeting,
          ParticipantId = idParticipant
        }
      );

      _context.SaveChanges();
      
      _dateTimeProviderMock.Setup(x => x.Now()).Returns(now);

      var meetingNotificationConfiguration = new MeetingNotificationConfiguration()
      {
        MeetingDelayMinutes = 15
      };

      var meeting = await _context.Meetings.FindAsync(idMeeting);

      var meetingNotificationService = new MeetingNotificationService(
        _context, new OptionsWrapper<MeetingNotificationConfiguration>(meetingNotificationConfiguration),
        _emailServiceMock.Object, _dateTimeProviderMock.Object
      );

      await meetingNotificationService.NotifyParticipantsMeeting();
      
      _emailServiceMock.Verify(send => send.SendEmailAsync(email, meeting), Times.Never);

      Assert.True(meeting.IsNotified);

    }
  }
}

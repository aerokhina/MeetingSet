using System;
using System.Collections.Generic;
using System.Linq;
using MeetingSet.Controllers;
using MeetingSet.Data;
using MeetingSet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MeetingSet.Tests
{
  public class MeetingControllerTests
  {
    private readonly ApplicationContext _context;

    public MeetingControllerTests()
    {
      var builder = new DbContextOptionsBuilder<ApplicationContext>();
      builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
      var options = builder.Options;
      _context = new ApplicationContext(options);
    }

    [Fact]
    public async void CreateMeeting_Success()
    {
      var name = "meetingName";
      var startDate = new DateTime(2020, 06, 12, 21, 30, 00);
      var endDate = new DateTime(2020, 06, 12, 23, 30, 00);

      var controller = new MeetingController(_context);

      var result = await controller.Create(
        new MeetingInputModel()
        {
          Name = name,
          StartDateTimeMeeting = startDate,
          EndDateTimeMeeting = endDate
        }
      );

      Assert.Single(_context.Meetings);

      var meeting = _context.Meetings.First();
      Assert.Equal(name, meeting.Name);
      Assert.Equal(startDate, meeting.StartDateTimeMeeting);
      Assert.Equal(endDate, meeting.EndDateTimeMeeting);

      Assert.NotNull(result);
      Assert.IsType<OkObjectResult>(result);

      var okResult = (OkObjectResult) result;
      Assert.IsType<MeetingOutputModel>(okResult.Value);

      var meetingOutputModel = (MeetingOutputModel) okResult.Value;
      Assert.Equal(name, meetingOutputModel.Name);
      Assert.Equal(startDate, meetingOutputModel.StartDateTimeMeeting);
      Assert.Equal(endDate, meetingOutputModel.EndDateTimeMeeting);
    }

    [Fact]
    public async void CreateMeeting_FailsWhenDatesIncorrect()
    {
      var startDate = new DateTime(2020, 06, 12, 21, 30, 00);
      var endDate = new DateTime(2020, 06, 12, 19, 30, 00);

      var controller = new MeetingController(_context);
      var result = await controller.Create(
        new MeetingInputModel()
        {
          Name = "meetingName",
          StartDateTimeMeeting = startDate,
          EndDateTimeMeeting = endDate
        }
      );

      Assert.Empty(_context.Meetings);

      Assert.NotNull(result);
      Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async void DeleteMeeting_Success()
    {
      const int idMeeting = 1;
      _context.Meetings.Add(
        new Meeting
        {
          Id = idMeeting,
          Name = "meeting",
          StartDateTimeMeeting = new DateTime(2020, 06, 12, 21, 30, 00),
          EndDateTimeMeeting = new DateTime(2020, 06, 12, 23, 30, 00)
        }
      );

      _context.SaveChanges();

      var controller = new MeetingController(_context);

      var result = await controller.Delete(idMeeting);

      Assert.Empty(_context.Meetings);

      Assert.NotNull(result);
      Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async void DeleteMeeting_FailsWhenNotFound()
    {
      const int idMeeting = 1;
      _context.Meetings.Add(
        new Meeting
        {
          Id = 2,
          Name = "meeting",
          StartDateTimeMeeting = new DateTime(2020, 06, 12, 21, 30, 00),
          EndDateTimeMeeting = new DateTime(2020, 06, 12, 23, 30, 00)
        }
      );

      _context.SaveChanges();

      var controller = new MeetingController(_context);

      var result = await controller.Delete(idMeeting);

      Assert.Single(_context.Meetings);

      Assert.NotNull(result);
      Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async void AddParticipant_Success()
    {
      const int idMeeting = 1;
      const int idParticipant = 1;

      _context.Meetings.Add(
        new Meeting
        {
          Id = idMeeting,
          Name = "meeting",
          StartDateTimeMeeting = new DateTime(2020, 06, 12, 21, 30, 00),
          EndDateTimeMeeting = new DateTime(2020, 06, 12, 23, 30, 00)
        }
      );

      _context.Participants.Add(
        new Participant
        {
          Id = idParticipant,
          Name = "Sasha",
          Email = "sa@as.rt"
        }
      );

      _context.SaveChanges();

      var controller = new MeetingController(_context);

      var result = await controller.AddParticipant(idMeeting, idParticipant);

      Assert.Single(_context.MeetingParticipants);

      var meetingParticipant = _context.MeetingParticipants.First();

      Assert.Equal(idMeeting, meetingParticipant.MeetingId);
      Assert.Equal(idParticipant, meetingParticipant.ParticipantId);

      Assert.NotNull(result);
      Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async void AddParticipant_FailsWhenMeetingNotFound()
    {
      const int idMeeting = 1;
      const int idParticipant = 1;

      _context.Meetings.Add(
        new Meeting
        {
          Id = 2,
          Name = "meeting",
          StartDateTimeMeeting = new DateTime(2020, 06, 12, 21, 30, 00),
          EndDateTimeMeeting = new DateTime(2020, 06, 12, 23, 30, 00)
        }
      );

      _context.Participants.Add(
        new Participant
        {
          Id = idParticipant,
          Name = "Sasha",
          Email = "sa@as.rt"
        }
      );

      _context.SaveChanges();

      var controller = new MeetingController(_context);

      var result = await controller.AddParticipant(idMeeting, idParticipant);

      Assert.Empty(_context.MeetingParticipants);

      Assert.NotNull(result);
      Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async void AddParticipant_FailsWhenParticipantNotFound()
    {
      const int idMeeting = 1;
      const int idParticipant = 1;

      _context.Meetings.Add(
        new Meeting
        {
          Id = idMeeting,
          Name = "meeting",
          StartDateTimeMeeting = new DateTime(2020, 06, 12, 21, 30, 00),
          EndDateTimeMeeting = new DateTime(2020, 06, 12, 23, 30, 00)
        }
      );

      _context.Participants.Add(
        new Participant
        {
          Id = 2,
          Name = "Sasha",
          Email = "sa@as.rt"
        }
      );

      _context.SaveChanges();

      var controller = new MeetingController(_context);

      var result = await controller.AddParticipant(idMeeting, idParticipant);

      Assert.Empty(_context.MeetingParticipants);

      Assert.NotNull(result);
      Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async void AddParticipant_FailsWhenParticipantIsBusy()
    {
      const int idFirstMeeting = 1;
      const int idSecondMeeting = 2;
      const int idParticipant = 1;

      _context.Meetings.Add(
        new Meeting
        {
          Id = idFirstMeeting,
          Name = "meeting1",
          StartDateTimeMeeting = new DateTime(2020, 06, 12, 21, 30, 00),
          EndDateTimeMeeting = new DateTime(2020, 06, 12, 23, 30, 00)
        }
      );

      _context.Meetings.Add(
        new Meeting
        {
          Id = idSecondMeeting,
          Name = "meeting2",
          StartDateTimeMeeting = new DateTime(2020, 06, 12, 22, 00, 00),
          EndDateTimeMeeting = new DateTime(2020, 06, 12, 23, 00, 00)
        }
      );

      _context.Participants.Add(
        new Participant
        {
          Id = idParticipant,
          Name = "Sasha",
          Email = "sa@as.rt"
        }
      );

      _context.MeetingParticipants.Add(
        new MeetingParticipant
        {
          MeetingId = idFirstMeeting,
          ParticipantId = idParticipant
        }
      );

      _context.SaveChanges();

      var controller = new MeetingController(_context);

      var result = await controller.AddParticipant(idSecondMeeting, idParticipant);

      Assert.Single(_context.MeetingParticipants);

      Assert.NotNull(result);
      Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async void RemoveParticipant_Success()
    {
      const int idMeeting = 1;
      const int idParticipant = 1;

      _context.MeetingParticipants.Add(
        new MeetingParticipant()
        {
          MeetingId = idMeeting,
          ParticipantId = idParticipant
        }
      );

      _context.SaveChanges();

      var controller = new MeetingController(_context);

      var result = await controller.RemoveParticipant(idMeeting, idParticipant);

      Assert.Empty(_context.MeetingParticipants);

      Assert.NotNull(result);
      Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async void RemoveParticipant_FailsWhenNotFound()
    {
      const int idMeeting = 1;
      const int idParticipant = 1;

      _context.MeetingParticipants.Add(
        new MeetingParticipant()
        {
          MeetingId = 2,
          ParticipantId = 2
        }
      );

      _context.SaveChanges();

      var controller = new MeetingController(_context);

      var result = await controller.RemoveParticipant(idMeeting, idParticipant);

      Assert.Single(_context.MeetingParticipants);

      Assert.NotNull(result);
      Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async void GetList_Success()
    {
      var idMeeting = 1;
      var nameMeeting = "meeting";
      var startDate = new DateTime(2020, 06, 12, 21, 30, 00);
      var endDate = new DateTime(2020, 06, 12, 23, 30, 00);
      var idParticipant = 1;
      var nameParticipant = "Sasha";
      var email = "sa@as.rt";

      _context.Meetings.Add(
        new Meeting
        {
          Id = idMeeting,
          Name = nameMeeting,
          StartDateTimeMeeting = startDate,
          EndDateTimeMeeting = endDate
        }
      );

      _context.Participants.Add(
        new Participant
        {
          Id = idParticipant,
          Name = nameParticipant,
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

      var controller = new MeetingController(_context);

      var result = await controller.GetList();

      Assert.NotNull(result);
      Assert.IsType<OkObjectResult>(result);

      var okResult = (OkObjectResult) result;
      Assert.IsType<List<MeetingListItemModel>>(okResult.Value);

      var meetingList = (List<MeetingListItemModel>) okResult.Value;
      Assert.Single(meetingList);

      var meeting = meetingList.First();
      Assert.Equal(nameMeeting, meeting.Name);
      Assert.Equal(startDate, meeting.StartDateTimeMeeting);
      Assert.Equal(endDate, meeting.EndDateTimeMeeting);

      Assert.Single(meeting.Participants);

      var participant = meeting.Participants.First();
      Assert.Equal(nameParticipant, participant.Name);
      Assert.Equal(email, participant.Email);
    }
  }
}

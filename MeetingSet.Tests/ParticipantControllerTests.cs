using System;
using System.Linq;
using MeetingSet.Controllers;
using MeetingSet.Data;
using MeetingSet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MeetingSet.Tests
{
  public class ParticipantControllerTests
  {
    private readonly ApplicationContext _context;

    public ParticipantControllerTests()
    {
      var builder = new DbContextOptionsBuilder<ApplicationContext>();
      builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
      var options = builder.Options;
      _context = new ApplicationContext(options);
    }
    
    [Fact]
    public async void CreateParticipant_Success()
    {
      var name = "Sasha";
      var email = "sa@as.rt";

      var controller = new ParticipantController(_context);

      var result = await controller.Create(
        new ParticipantInputModel
        {
          Name = name,
          Email = email
        }
      );

      Assert.Single(_context.Participants);

      var participants = _context.Participants.First();
      Assert.Equal(name, participants.Name);
      Assert.Equal(email, participants.Email);

      Assert.NotNull(result);
      Assert.IsType<OkObjectResult>(result);

      var okResult = (OkObjectResult) result;
      Assert.IsType<ParticipantOutputModel>(okResult.Value);

      var participantOutputModel = (ParticipantOutputModel) okResult.Value;
      Assert.Equal(name, participantOutputModel.Name);
      Assert.Equal(email, participantOutputModel.Email);
    }

    [Fact]
    public async void DeleteParticipant_Success()
    {
      const int idParticipant = 1;
      _context.Participants.Add(
        new Participant()
        {
          Id = idParticipant,
          Name = "Sasha",
          Email = "sa@as.rt"
        }
      );

      _context.SaveChanges();

      var controller = new ParticipantController(_context);

      var result = await controller.Delete(idParticipant);

      Assert.Empty(_context.Participants);

      Assert.NotNull(result);
      Assert.IsType<OkResult>(result);
    }
    
    [Fact]
    public async void DeleteParticipant_FailsWhenNotFound()
    {
      const int idParticipant = 1;
      _context.Participants.Add(
        new Participant()
        {
          Id = 2,
          Name = "Sasha",
          Email = "sa@as.rt"
        }
      );

      _context.SaveChanges();

      var controller = new ParticipantController(_context);

      var result = await controller.Delete(idParticipant);

      Assert.Single(_context.Participants);

      Assert.NotNull(result);
      Assert.IsType<NotFoundResult>(result);
    }
  }
}

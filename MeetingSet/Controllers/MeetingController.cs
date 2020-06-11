using System;
using System.Threading.Tasks;
using MeetingSet.Data;
using MeetingSet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetingSet.Controllers
{
  [Route("[controller]")]
  public class MeetingController : Controller
  {
    private readonly ApplicationContext _context;

    public MeetingController(ApplicationContext context)
    {
      _context = context;
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Create([FromBody] MeetingInputModel model)
    {
      var item = new Meeting
      {
        Name = model.Name,
        DateTimeMeeting = model.DateTimeMeeting
      };
      _context.Add(item);
      await _context.SaveChangesAsync();
      return Ok(
        new MeetingOutputModel
        {
          Name = item.Name,
          DateTimeMeeting = model.DateTimeMeeting,
          Id = item.Id,
        }
      );
    }

    [HttpPost]
    [Route("[action]/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var item = await _context.Meetings
        .SingleOrDefaultAsync(x => x.Id == id);
      if (item == null)
      {
        throw new ArgumentException("Meeting not found");
      }

      _context.Remove(item);
      _context.SaveChanges();
      return Ok();
    }

    [HttpPost]
    [Route("{meetingId}/[action]/{participantId}")]
    public async Task<IActionResult> AddParticipant(int meetingId, int participantId)
    {
      var item = await _context.Meetings
        .AnyAsync(x => x.Id == meetingId);
      if (!item)
      {
        throw new ArgumentException("Meeting not found");
      }

      _context.Add(new MeetingParticipant {MeetingId = meetingId, ParticipantId = participantId});
      _context.SaveChanges();
      return Ok();
    }

    [HttpPost]
    [Route("{meetingId}/[action]/{participantId}")]
    public async Task<IActionResult> RemoveParticipant(int meetingId, int participantId)
    {
      var item = await _context.MeetingParticipants
        .SingleOrDefaultAsync(x => x.MeetingId == meetingId && x.ParticipantId == participantId);
      if (item == null)
      {
        throw new ArgumentException("Meeting participant not found");
      }

      _context.Remove(item);
      _context.SaveChanges();
      return Ok();
    }
  }
}

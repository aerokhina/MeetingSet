using System;
using System.Linq;
using System.Threading.Tasks;
using MeetingSet.Data;
using MeetingSet.Models;
using MeetingSet.Services;
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
      if (model.StartDateTimeMeeting > model.EndDateTimeMeeting)
      {
        ModelState.AddModelError("MeetingPeriod", "Start date of meeting should be less than end date");
      }

      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var item = new Meeting
      {
        Name = model.Name,
        StartDateTimeMeeting = model.StartDateTimeMeeting.Value,
        EndDateTimeMeeting = model.EndDateTimeMeeting.Value
      };
      _context.Add(item);
      await _context.SaveChangesAsync();
      return Ok(
        new MeetingOutputModel
        {
          Name = item.Name,
          StartDateTimeMeeting = model.StartDateTimeMeeting.Value,
          EndDateTimeMeeting = model.EndDateTimeMeeting.Value,
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
        return NotFound();
      }

      _context.Remove(item);
      _context.SaveChanges();
      return Ok();
    }

    [HttpPost]
    [Route("{meetingId}/[action]/{participantId}")]
    public async Task<IActionResult> AddParticipant(int meetingId, int participantId)
    {
      var meeting = await _context.Meetings
        .SingleOrDefaultAsync(x => x.Id == meetingId);

      if (meeting == null)
      {
        return NotFound();
      }

      var participant = await _context.Participants.SingleOrDefaultAsync(x => x.Id == participantId);

      if (participant == null)
      {
        return NotFound();
      }

      var isTimeValid = await _context.MeetingParticipants.Where(mp => mp.ParticipantId == participantId)
        .AllAsync(
          x => x.Meeting.StartDateTimeMeeting > meeting.EndDateTimeMeeting
               || x.Meeting.EndDateTimeMeeting < meeting.StartDateTimeMeeting
        );

      if (!isTimeValid)
      {
        ModelState.AddModelError("ParticipantMeeting", "Participant is busy at this time");
        return BadRequest(ModelState);
      }

      _context.Add(new MeetingParticipant {MeetingId = meetingId, ParticipantId = participantId});
      _context.SaveChanges();
      
      EmailService emailService = new EmailService();
      await emailService.SendEmailAsync(participant.Email, meeting);
      
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
        return NotFound();
      }

      _context.Remove(item);
      _context.SaveChanges();
      return Ok();
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> GetList()
    {
      var result = await _context.Meetings.Select(
        x => new MeetingListItemModel()
        {
          Name = x.Name,
          Id = x.Id,
          StartDateTimeMeeting = x.StartDateTimeMeeting,
          EndDateTimeMeeting = x.EndDateTimeMeeting,
          Participants = x.MeetingParticipants
            .Select(
              p => new ParticipantOutputModel()
              {
                Id = p.ParticipantId,
                Name = p.Participant.Name,
                Email = p.Participant.Email,
              }
            ).ToList()
        }
      ).ToListAsync();

      return Ok(result);
    }
  }
}

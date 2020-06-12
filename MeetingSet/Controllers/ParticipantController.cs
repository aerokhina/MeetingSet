using System;
using System.Threading.Tasks;
using MeetingSet.Data;
using MeetingSet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetingSet.Controllers
{
  [Route("[controller]")]
  public class ParticipantController : Controller
  {
    private readonly ApplicationContext _context;

    public ParticipantController(ApplicationContext context)
    {
      _context = context;
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Create([FromBody] ParticipantInputModel model)
    {
      var item = new Participant
      {
        Name = model.Name,
        Email = model.Email
      };
      _context.Add(item);
      await _context.SaveChangesAsync();
      return Ok(
        new ParticipantOutputModel
        {
          Name = item.Name,
          Email = model.Email,
          Id = item.Id,
        }
      );
    }

    [HttpPost]
    [Route("[action]/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var item = await _context.Participants
        .SingleOrDefaultAsync(x => x.Id == id);
      if (item == null)
      {
        throw new ArgumentException("Participant not found");
      }

      _context.Remove(item);
      _context.SaveChanges();
      return Ok();
    }
  }
}

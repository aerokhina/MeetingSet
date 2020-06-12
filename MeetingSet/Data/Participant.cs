using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeetingSet.Data
{
  public class Participant
  {
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Email { get; set; }

    public List<MeetingParticipant> MeetingParticipants { get; set; }

    public Participant()
    {
      MeetingParticipants = new List<MeetingParticipant>();
    }
  }
}

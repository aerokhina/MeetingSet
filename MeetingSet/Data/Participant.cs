using System.Collections.Generic;

namespace MeetingSet.Data
{
  public class Participant
  {
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public List<MeetingParticipant> MeetingParticipants { get; set; }
    
    public Participant()
    {
      MeetingParticipants = new List<MeetingParticipant>();
    }
  }
}

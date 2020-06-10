using System;
using System.Collections.Generic;

namespace MeetingSet.Data
{
  public class Meeting
  {
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public DateTime DateTimeMeeting { get; set; }
    
    public List<MeetingParticipant> MeetingParticipants { get; set; }
    
    public Meeting()
    {
      MeetingParticipants = new List<MeetingParticipant>();
    }
  }
}

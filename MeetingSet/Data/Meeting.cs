using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeetingSet.Data
{
  public class Meeting
  {
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
    
    public DateTime StartDateTimeMeeting { get; set; }
    
    public DateTime EndDateTimeMeeting { get; set; }

    public List<MeetingParticipant> MeetingParticipants { get; set; }
    
    public bool IsNotified { get; set; }

    public Meeting()
    {
      MeetingParticipants = new List<MeetingParticipant>();
    }
  }
}

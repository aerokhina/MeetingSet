using System;

namespace MeetingSet.Services
{
  public class DateTimeProvider
    : IDateTimeProvider
  {
    public DateTime Now()
    {
      return DateTime.Now;
    }
  }
}

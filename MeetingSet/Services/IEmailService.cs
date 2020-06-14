using System.Threading.Tasks;
using MeetingSet.Data;

namespace MeetingSet.Services
{
  public interface IEmailService
  {
    Task SendEmailAsync(string email, Meeting meeting);
  }
}

using OAuthTutorial.Helpers;
using Google.Apis.Calendar.v3;

namespace ApplicationCore.Interfaces
{
    public interface IGoogleCalendarService
    {
        public Task CreateEventAsync(string accessToken, string calendarId, string summary, string description, DateTime startDateTime, DateTime endDateTime, string timeZone);
    }
}

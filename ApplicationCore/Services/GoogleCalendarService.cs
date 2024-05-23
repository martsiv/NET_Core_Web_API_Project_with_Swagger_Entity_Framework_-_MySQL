using ApplicationCore.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;

namespace ApplicationCore.Services
{
    internal class GoogleCalendarService : IGoogleCalendarService
    {
        private readonly string ApplicationName = "OAuthUniversityApplication";

        public async Task CreateEventAsync(string accessToken, string calendarId, string summary, string description, DateTime startDateTime, DateTime endDateTime, string timeZone)
        {
            var credential = GoogleCredential.FromAccessToken(accessToken);
            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });

            var newEvent = new Event()
            {
                Summary = summary,
                Description = description,
                Start = new EventDateTime()
                {
                    DateTime = startDateTime,
                    TimeZone = timeZone,
                },
                End = new EventDateTime()
                {
                    DateTime = endDateTime,
                    TimeZone = timeZone,
                }
            };

            EventsResource.InsertRequest request = service.Events.Insert(newEvent, calendarId);
            await request.ExecuteAsync();
        }
    }
}

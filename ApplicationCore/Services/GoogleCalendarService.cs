using ApplicationCore.DTOs;
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
        private readonly ILessonEventService _lessonEventService;
        public GoogleCalendarService(ILessonEventService lessonEventService)
        {
            _lessonEventService = lessonEventService;
        }
        public async Task AddEventToOwnCalendarAsync(string accessToken, LessonEventDto lessonEventDto)
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
                Summary = lessonEventDto.Summary,
                Description = lessonEventDto.Description,
                Start = new EventDateTime()
                {
                    DateTime = lessonEventDto.StartDateTime,
                    TimeZone = lessonEventDto.TimeZone,
                },
                End = new EventDateTime()
                {
                    DateTime = lessonEventDto.EndDateTime,
                    TimeZone = lessonEventDto.TimeZone,
                }
            };

            EventsResource.InsertRequest request = service.Events.Insert(newEvent, lessonEventDto.CalendarId);
            await request.ExecuteAsync();
        }

        public async Task CreateEventInDbAsync(string calendarId, string summary, string description, DateTime startDateTime, DateTime endDateTime, string timeZone, int teacherId)
        {
            var lessonEventCreateDto = new CreateLessonEventDto()
            {
                CalendarId = calendarId,
                Summary = summary,
                Description = description,
                StartDateTime = startDateTime,
                EndDateTime = endDateTime,
                TimeZone = timeZone,
                TeacherId = teacherId
            };
            _lessonEventService.AddLessonEvent(lessonEventCreateDto);
        }
    }
}

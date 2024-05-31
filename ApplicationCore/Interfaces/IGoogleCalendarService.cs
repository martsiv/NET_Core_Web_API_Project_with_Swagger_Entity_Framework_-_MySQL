using ApplicationCore.DTOs;

namespace ApplicationCore.Interfaces
{
    public interface IGoogleCalendarService
    {
        public Task CreateEventInDbAsync(string calendarId, string summary, string description, DateTime startDateTime, DateTime endDateTime, string timeZone, int teacherId);
        public Task AddEventToOwnCalendarAsync(string accessToken, LessonEventDto lessonEventDto);
    }
}

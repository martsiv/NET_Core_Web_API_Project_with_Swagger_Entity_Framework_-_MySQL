﻿namespace ApplicationCore.DTOs
{
    public class LessonEventDto
    {
        public int Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string CalendarId { get; set; }
        public string TimeZone { get; set; }
        public int TeacherId { get; set; }
    }
}

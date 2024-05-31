using ApplicationCore.DTOs;

namespace ApplicationCore.Interfaces
{
    public interface ILessonEventService
    {
		void AddLessonEvent(CreateLessonEventDto lessonEventDto);
        void RemoveLessonEvent(int lessonEventId);
        void UpdateLessonEvent(int lessonEventId, LessonEventDto lessonEventDto);
        IEnumerable<LessonEventViewDto> GetAllLessonEvents();
        IEnumerable<LessonEventDto> GetLessonEventByTeacher(int teacherId);
        LessonEventDto GetLessonEventById(int lessonEventId);

        Task AddLessonEventAsync(CreateCourseDto course);
        Task RemoveLessonEventAsync(int courseId);
        Task UpdateLessonEventAsync(int courseId, CourseDto course);
        Task<IEnumerable<LessonEventViewDto>> GetAllLessonEventsAsync();
        Task<IEnumerable<LessonEventDto>> GetLessonEventByTeacherAsync(int teacherId);
        Task<LessonEventDto> GetLessonEventByIdAsync(int lessonEventId);
    }
}

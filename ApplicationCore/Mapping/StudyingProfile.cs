using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using AutoMapper;

namespace ApplicationCore.Mapping
{
	public class StudyingProfile : Profile
	{
        public StudyingProfile()
        {
			CreateMap<Teacher, TeacherDto>().ReverseMap();
			CreateMap<CreateTeacherDto, Teacher>();
			CreateMap<Student, StudentDto>().ReverseMap();
			CreateMap<CreateStudentDto, Student>();
			CreateMap<Course, CourseDto>().ReverseMap();
			CreateMap<CreateCourseDto, Course>();
			CreateMap<CourseStudent, CourseStudentDto>().ReverseMap();

			CreateMap<Teacher, TeacherViewDto>();
			CreateMap<Student, StudentViewDto>()
				.ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.CoursesStudents.Select(cs => cs.Course)));
			CreateMap<Course, CourseViewDto>()
				.ForMember(dest => dest.Teacher, opt => opt.MapFrom(src => src.Teacher))
				.ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.CoursesStudents.Select(cs => cs.Student)));

			CreateMap<TeacherViewDto, TeacherDto>().ReverseMap();
			CreateMap<StudentViewDto, StudentDto>().ReverseMap();
			CreateMap<CourseViewDto, CourseDto>().ReverseMap();

			CreateMap<LessonEvent, LessonEventViewDto>().ReverseMap();
			CreateMap<CreateLessonEventDto, LessonEvent>();
			CreateMap<LessonEvent, LessonEventDto>().ReverseMap();
			CreateMap<LessonEventViewDto, LessonEventDto>().ReverseMap();
		}
	}
}

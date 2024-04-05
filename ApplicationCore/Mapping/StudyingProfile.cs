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
			CreateMap<CreateTeacherDto, TeacherDto>();
			CreateMap<Student, StudentDto>().ReverseMap();
			CreateMap<CreateStudentDto, StudentDto>();
			CreateMap<Course, CourseDto>().ReverseMap();
			CreateMap<CreateCourseDto, CourseDto>();
		}
    }
}

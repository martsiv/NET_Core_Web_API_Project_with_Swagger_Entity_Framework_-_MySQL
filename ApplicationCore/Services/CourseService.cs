using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using AutoMapper;

namespace ApplicationCore.Services
{
	public class CourseService : ICourseService
	{
		private readonly IMapper _mapper;
		private readonly IRepository<Course> _coursesRepo;
        public CourseService(IMapper mapper,
							IRepository<Course> coursesRepo)
        {
            this._mapper = mapper;
			this._coursesRepo = coursesRepo;
        }
        public void AddCourse(CreateCourseDto course)
		{
			var entity = _mapper.Map<Course>(course);
			_coursesRepo.Insert(entity);
			_coursesRepo.Save();
		}

		public IEnumerable<CourseViewDto> GetAllCourses()
		{
			var entities = _coursesRepo.GetListBySpec(new CourseSpecs.All());
			return _mapper.Map<IEnumerable<CourseViewDto>>(entities);
		}

		public CourseViewDto? GetCourseById(int courseId)
		{
			var entity = _coursesRepo.GetItemBySpec(new CourseSpecs.ById(courseId));
			if (entity == null) return null;
			return _mapper.Map<CourseViewDto>(entity);
		}

		public IEnumerable<CourseDto> GetCoursesByStudent(int studentId)
		{
			var entities = _coursesRepo.GetListBySpec(new CourseSpecs.ByStudent(studentId));
			return _mapper.Map<IEnumerable<CourseDto>>(entities);
		}

		public IEnumerable<CourseDto> GetCoursesByTeacher(int teacherId)
		{
			var entities = _coursesRepo.GetListBySpec(new CourseSpecs.ByTeacher(teacherId));
			return _mapper.Map<IEnumerable<CourseDto>>(entities);
		}

		public void RemoveCourse(int courseId)
		{
			var entity = _coursesRepo.GetByID(courseId);
			if (entity != null)
			{
				_coursesRepo.Delete(courseId);
				_coursesRepo.Save();
			}
		}

		public void UpdateCourse(int courseId, CourseDto course)
		{
			var existingEntity = _coursesRepo.GetByID(courseId);
			if (existingEntity != null)
			{
				_mapper.Map(course, existingEntity);
				_coursesRepo.Update(existingEntity);
				_coursesRepo.Save();
			}
		}
	}
}

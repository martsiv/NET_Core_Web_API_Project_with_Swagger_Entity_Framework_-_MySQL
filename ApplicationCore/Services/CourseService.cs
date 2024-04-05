using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
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
        public void AddCourse(CourseDto course)
		{
			var entity = _mapper.Map<Course>(course);
			_coursesRepo.Insert(entity);
			_coursesRepo.Save();
		}

		public IEnumerable<CourseDto> GetAllCourses()
		{
			var entities = _coursesRepo.GetAll();
			return _mapper.Map<IEnumerable<CourseDto>>(entities);
		}

		public CourseDto? GetCourseById(int courseId)
		{
			var entity = _coursesRepo.GetByID(courseId);
			if (entity == null) return null;
			return _mapper.Map<CourseDto>(entity);
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

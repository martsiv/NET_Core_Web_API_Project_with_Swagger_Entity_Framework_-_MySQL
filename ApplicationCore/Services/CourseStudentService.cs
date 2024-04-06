using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using AutoMapper;

namespace ApplicationCore.Services
{
	public class CourseStudentService : ICourseStudentService
	{
		private readonly IMapper _mapper;
		private readonly IRepository<CourseStudent> _courseStudentRepo;
        public CourseStudentService(IMapper mapper,
							IRepository<CourseStudent> courseStudentRepo)
		{
			this._mapper = mapper;
			this._courseStudentRepo = courseStudentRepo;
		}
		public void AddCourseStudent(CourseStudentDto courseStudent)
		{
			var entity = _mapper.Map<CourseStudent>(courseStudent);
			_courseStudentRepo.Insert(entity);
			_courseStudentRepo.Save();
		}

		public IEnumerable<CourseStudentDto> GetAllCourseStudent()
		{
			var entities = _courseStudentRepo.GetAll();
			return _mapper.Map<IEnumerable<CourseStudentDto>>(entities);
		}

		public CourseStudentDto? GetCourseStudentById(int courseStudentId)
		{
			var entity = _courseStudentRepo.GetByID(courseStudentId);
			if (entity == null) return null;
			return _mapper.Map<CourseStudentDto>(entity);
		}

		CourseStudentDto? ICourseStudentService.GetCourseStudentByIds(int courseId, int studentId)
		{
			var entity = _courseStudentRepo.GetItemBySpec(new CourseStudentSpecs.ByCourseAndStudent(courseId, studentId));
			return _mapper.Map<CourseStudentDto>(entity);
		}

		public void RemoveCourseStudent(int courseStudentId)
		{
			var entity = _courseStudentRepo.GetByID(courseStudentId);
			if (entity != null)
			{
				_courseStudentRepo.Delete(courseStudentId);
				_courseStudentRepo.Save();
			}
		}

		public void UpdateCourseStudent(int courseStudentId, CourseStudentDto courseStudent)
		{
			var existingEntity = _courseStudentRepo.GetByID(courseStudentId);
			if (existingEntity != null)
			{
				_mapper.Map(courseStudent, existingEntity);
				_courseStudentRepo.Update(existingEntity);
				_courseStudentRepo.Save();
			}
		}
	}
}

using ApplicationCore.CustomException;
using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using AutoMapper;
using System.Net;

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
			try
			{
				_courseStudentRepo.Insert(entity);
				_courseStudentRepo.Save();
			}
			catch
			{
				throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
			}
		}

		public IEnumerable<CourseStudentDto> GetAllCourseStudent()
		{
			var entities = _courseStudentRepo.GetAll();
			return _mapper.Map<IEnumerable<CourseStudentDto>>(entities);
		}

		public CourseStudentDto GetCourseStudentById(int courseStudentId)
		{
			if (courseStudentId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var entity = _courseStudentRepo.GetByID(courseStudentId);
			if (entity == null) throw new HttpException("CourseStudent record not found.", HttpStatusCode.NotFound);

			return _mapper.Map<CourseStudentDto>(entity);
		}

		CourseStudentDto ICourseStudentService.GetCourseStudentByIds(int courseId, int studentId)
		{
			if (courseId < 0 || studentId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var entity = _courseStudentRepo.GetItemBySpec(new CourseStudentSpecs.ByCourseAndStudent(courseId, studentId));
			if (entity == null) throw new HttpException("CourseStudent record not found.", HttpStatusCode.NotFound);

			return _mapper.Map<CourseStudentDto>(entity);
		}

		public void RemoveCourseStudent(int courseStudentId)
		{
			if (courseStudentId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var entity = _courseStudentRepo.GetByID(courseStudentId);
			if (entity == null) throw new HttpException("CourseStudent record not found.", HttpStatusCode.NotFound);

			try
			{
				_courseStudentRepo.Delete(courseStudentId);
				_courseStudentRepo.Save();
			}
			catch
			{
				throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
			}
		}

		public void UpdateCourseStudent(int courseStudentId, CourseStudentDto courseStudent)
		{
			if (courseStudentId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var existingEntity = _courseStudentRepo.GetByID(courseStudentId);

			if (existingEntity == null) throw new HttpException("Course not found.", HttpStatusCode.NotFound);

			_mapper.Map(courseStudent, existingEntity);
			try
			{
				_courseStudentRepo.Update(existingEntity);
				_courseStudentRepo.Save();
			}
			catch
			{
				throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
			}
		}

		public async Task AddCourseStudentAsync(CourseStudentDto courseStudent)
		{
			var entity = _mapper.Map<CourseStudent>(courseStudent);
			try
			{
				await _courseStudentRepo.InsertAsync(entity);
				await _courseStudentRepo.SaveAsync();
			}
			catch
			{
				throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
			}
		}

		public async Task<IEnumerable<CourseStudentDto>> GetAllCourseStudentAsync()
		{
			var entities = await _courseStudentRepo.GetAllAsync();
			return _mapper.Map<IEnumerable<CourseStudentDto>>(entities);
		}

		public async Task<CourseStudentDto> GetCourseStudentByIdAsync(int courseStudentId)
		{
			if (courseStudentId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var entity = await _courseStudentRepo.GetByIDAsync(courseStudentId);
			if (entity == null) throw new HttpException("CourseStudent record not found.", HttpStatusCode.NotFound);

			return _mapper.Map<CourseStudentDto>(entity);
		}

		public async Task<CourseStudentDto> GetCourseStudentByIdsAsync(int courseId, int studentId)
		{
			if (courseId < 0 || studentId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var entity = await _courseStudentRepo.GetItemBySpecAsync(new CourseStudentSpecs.ByCourseAndStudent(courseId, studentId));
			if (entity == null) throw new HttpException("CourseStudent record not found.", HttpStatusCode.NotFound);

			return _mapper.Map<CourseStudentDto>(entity);
		}

		public async Task RemoveCourseStudentAsync(int courseStudentId)
		{
			if (courseStudentId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var entity = await _courseStudentRepo.GetByIDAsync(courseStudentId);
			if (entity == null) throw new HttpException("CourseStudent record not found.", HttpStatusCode.NotFound);

			try
			{
				await _courseStudentRepo.DeleteAsync(entity);
				await _courseStudentRepo.SaveAsync();
			}
			catch
			{
				throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
			}
		}

		public async Task UpdateCourseStudentAsync(int courseStudentId, CourseStudentDto courseStudent)
		{
			if (courseStudentId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var existingEntity = await _courseStudentRepo.GetByIDAsync(courseStudentId);
			if (existingEntity == null) throw new HttpException("CourseStudent record not found.", HttpStatusCode.NotFound);

			_mapper.Map(courseStudent, existingEntity);
			try
			{
				_courseStudentRepo.Update(existingEntity);
				await _courseStudentRepo.SaveAsync();
			}
			catch
			{
				throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
			}
		}
	}
}

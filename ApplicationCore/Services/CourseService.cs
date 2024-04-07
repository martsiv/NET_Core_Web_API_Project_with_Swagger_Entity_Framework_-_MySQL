using ApplicationCore.CustomException;
using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using AutoMapper;
using FluentValidation;
using System.Net;

namespace ApplicationCore.Services
{
	public class CourseService : ICourseService
	{
		private readonly IMapper _mapper;
		private readonly IRepository<Course> _coursesRepo;
		private readonly IValidator<CreateCourseDto> _createValidator;
		private readonly IValidator<CourseDto> _editValidator;

		public CourseService(IMapper mapper,
							IRepository<Course> coursesRepo,
							IValidator<CreateCourseDto> createValidator,
							IValidator<CourseDto> editValidator)
        {
            this._mapper = mapper;
			this._coursesRepo = coursesRepo;
			this._createValidator = createValidator;
			this._editValidator = editValidator;
        }
        public void AddCourse(CreateCourseDto course)
		{
			try
			{
				_createValidator.ValidateAndThrow(course);
			}
			catch (ValidationException ex)
			{
				string message = "Validation error occurred. ";
				if (ex.Errors.Count() > 0)
				{
					foreach(var error in ex.Errors) 
					message += error.ToString();
				}

				throw new HttpException(message, HttpStatusCode.BadRequest);
			}
			catch (Exception ex)
			{
				throw new HttpException("An error occurred.", HttpStatusCode.InternalServerError);
			}

			var entity = _mapper.Map<Course>(course);
			try
			{
				_coursesRepo.Insert(entity);
				_coursesRepo.Save();
			}
			catch
			{
				throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
			}
		}

		public IEnumerable<CourseViewDto> GetAllCourses()
		{
			var entities = _coursesRepo.GetListBySpec(new CourseSpecs.All());
			return _mapper.Map<IEnumerable<CourseViewDto>>(entities);
		}

		public CourseViewDto GetCourseById(int courseId)
		{
			if (courseId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var entity = _coursesRepo.GetItemBySpec(new CourseSpecs.ById(courseId));
			if (entity == null) throw new HttpException("Course not found.", HttpStatusCode.NotFound);

			return _mapper.Map<CourseViewDto>(entity);
		}

		public IEnumerable<CourseDto> GetCoursesByStudent(int studentId)
		{
			if (studentId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var entities = _coursesRepo.GetListBySpec(new CourseSpecs.ByStudent(studentId));
			return _mapper.Map<IEnumerable<CourseDto>>(entities);
		}

		public IEnumerable<CourseDto> GetCoursesByTeacher(int teacherId)
		{
			if (teacherId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var entities = _coursesRepo.GetListBySpec(new CourseSpecs.ByTeacher(teacherId));
			return _mapper.Map<IEnumerable<CourseDto>>(entities);
		}

		public void RemoveCourse(int courseId)
		{
			if (courseId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var entity = _coursesRepo.GetByID(courseId);
			if (entity == null) throw new HttpException("Course not found.", HttpStatusCode.NotFound);

			try
			{
				_coursesRepo.Delete(courseId);
				_coursesRepo.Save();
			}
			catch
			{
				throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
			}
		}

		public void UpdateCourse(int courseId, CourseDto course)
		{
			if (courseId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			try
			{
				_editValidator.ValidateAndThrow(course);
			}
			catch (ValidationException ex)
			{
				string message = "Validation error occurred. ";
				if (ex.Errors.Count() > 0)
				{
					foreach (var error in ex.Errors)
						message += error.ToString();
				}

				throw new HttpException(message, HttpStatusCode.BadRequest);
			}
			catch (Exception ex)
			{
				throw new HttpException("An error occurred.", HttpStatusCode.InternalServerError);
			}

			var existingEntity = _coursesRepo.GetByID(courseId);
			if (existingEntity == null) throw new HttpException("Course not found.", HttpStatusCode.NotFound);

			_mapper.Map(course, existingEntity);
			try
			{
				_coursesRepo.Update(existingEntity);
				_coursesRepo.Save();
			}
			catch
			{
				throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
			}
		}

		public async Task AddCourseAsync(CreateCourseDto course)
		{
			try
			{
				await _createValidator.ValidateAndThrowAsync(course);
			}
			catch (ValidationException ex)
			{
				string message = "Validation error occurred. ";
				if (ex.Errors.Count() > 0)
				{
					foreach (var error in ex.Errors)
						message += error.ToString();
				}

				throw new HttpException(message, HttpStatusCode.BadRequest);
			}
			catch (Exception ex)
			{
				throw new HttpException("An error occurred.", HttpStatusCode.InternalServerError);
			}

			var entity = _mapper.Map<Course>(course);
			try
			{
				await _coursesRepo.InsertAsync(entity);
				await _coursesRepo.SaveAsync();
			}
			catch
			{
				throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
			}
		}

		public async Task<IEnumerable<CourseViewDto>> GetAllCoursesAsync()
		{
			var entities = await _coursesRepo.GetListBySpecAsync(new CourseSpecs.All());
			return _mapper.Map<IEnumerable<CourseViewDto>>(entities);
		}

		public async Task<CourseViewDto> GetCourseByIdAsync(int courseId)
		{
			if (courseId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var entity = await _coursesRepo.GetItemBySpecAsync(new CourseSpecs.ById(courseId));
			if (entity == null) throw new HttpException("Course not found.", HttpStatusCode.NotFound);

			return _mapper.Map<CourseViewDto>(entity);
		}

		public async Task<IEnumerable<CourseDto>> GetCoursesByStudentAsync(int studentId)
		{
			if (studentId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var entities = await _coursesRepo.GetListBySpecAsync(new CourseSpecs.ByStudent(studentId));
			return _mapper.Map<IEnumerable<CourseDto>>(entities);
		}

		public async Task<IEnumerable<CourseDto>> GetCoursesByTeacherAsync(int teacherId)
		{
			if (teacherId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var entities = await _coursesRepo.GetListBySpecAsync(new CourseSpecs.ByTeacher(teacherId));
			return _mapper.Map<IEnumerable<CourseDto>>(entities);
		}

		public async Task RemoveCourseAsync(int courseId)
		{
			if (courseId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var entity = await _coursesRepo.GetByIDAsync(courseId);
			if (entity == null) throw new HttpException("Course not found.", HttpStatusCode.NotFound);

			try
			{
				_coursesRepo.Delete(entity);
				await _coursesRepo.SaveAsync();
			}
			catch
			{
				throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
			}
		}

		public async Task UpdateCourseAsync(int courseId, CourseDto course)
		{
			if (courseId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			try
			{
				await _editValidator.ValidateAndThrowAsync(course);
			}
			catch (ValidationException ex)
			{
				string message = "Validation error occurred. ";
				if (ex.Errors.Count() > 0)
				{
					foreach (var error in ex.Errors)
						message += error.ToString();
				}

				throw new HttpException(message, HttpStatusCode.BadRequest);
			}
			catch (Exception ex)
			{
				throw new HttpException("An error occurred.", HttpStatusCode.InternalServerError);
			}

			var existingEntity = await _coursesRepo.GetByIDAsync(courseId);
			if (existingEntity == null) throw new HttpException("Course not found.", HttpStatusCode.NotFound);

			_mapper.Map(course, existingEntity);
			try
			{
				_coursesRepo.Update(existingEntity);
				await _coursesRepo.SaveAsync();
			}
			catch
			{
				throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
			}
		}
	}
}

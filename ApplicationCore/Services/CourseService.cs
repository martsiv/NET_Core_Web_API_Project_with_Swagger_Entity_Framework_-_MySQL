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

		public CourseViewDto? GetCourseById(int courseId)
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
	}
}

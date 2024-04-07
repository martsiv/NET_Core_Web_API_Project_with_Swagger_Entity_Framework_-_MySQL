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
	public class StudentService : IStudentService
	{
		private readonly IMapper _mapper;
		private readonly IRepository<Student> _studentsRepo;
		private readonly IValidator<CreateStudentDto> _createValidator;
		private readonly IValidator<StudentDto> _editValidator;
		public StudentService(IMapper mapper,
							IRepository<Student> studentRepo,
							IValidator<StudentDto> editValidator,
							IValidator<CreateStudentDto> createValidator)
		{
			this._mapper = mapper;
			this._studentsRepo = studentRepo;
			this._createValidator = createValidator;
			this._editValidator = editValidator;
		}
		public void AddStudent(CreateStudentDto student)
		{
			try
			{
				_createValidator.ValidateAndThrow(student);
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

			var entity = _mapper.Map<Student>(student);
			try
			{
				_studentsRepo.Insert(entity);
				_studentsRepo.Save();
			}
			catch
			{
				throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
			}
		}

		public IEnumerable<StudentViewDto> GetAllStudents()
		{
			var entities = _studentsRepo.GetListBySpec(new StudentSpecs.All());
			return _mapper.Map<IEnumerable<StudentViewDto>>(entities);
		}

		public StudentViewDto? GetStudentById(int studentId)
		{
			if (studentId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var entity = _studentsRepo.GetItemBySpec(new StudentSpecs.ById(studentId));
			if (entity == null) throw new HttpException("Student not found.", HttpStatusCode.NotFound);

			return _mapper.Map<StudentViewDto>(entity);
		}

		public IEnumerable<StudentDto> GetStudentsByCourse(int courseId)
		{
			if (courseId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var entities = _studentsRepo.GetListBySpec(new StudentSpecs.ByCourse(courseId));
			return _mapper.Map<IEnumerable<StudentDto>>(entities);
		}

		public void RemoveStudent(int studentId)
		{
			if (studentId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var entity = _studentsRepo.GetByID(studentId);
			if (entity == null) throw new HttpException("Student not found.", HttpStatusCode.NotFound);

			try
			{
				_studentsRepo.Delete(studentId);
				_studentsRepo.Save();
			}
			catch
			{
				throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
			}
		}

		public void UpdateStudent(int studentId, StudentDto student)
		{
			if (studentId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			try
			{
				_editValidator.ValidateAndThrow(student);
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

			var existingEntity = _studentsRepo.GetByID(studentId);
			if (existingEntity == null) throw new HttpException("Student not found.", HttpStatusCode.NotFound);

			_mapper.Map(student, existingEntity);
			try
			{
				_studentsRepo.Update(existingEntity);
				_studentsRepo.Save();
			}
			catch
			{
				throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
			}
		}
	}
}

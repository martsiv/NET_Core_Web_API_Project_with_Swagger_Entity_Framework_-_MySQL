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
	public class TeacherService : ITeacherService
	{
		private readonly IMapper _mapper;
		private readonly IRepository<Teacher> _teachersRepo;
		private readonly IValidator<CreateTeacherDto> _createValidator;
		private readonly IValidator<TeacherDto> _editValidator;

		public TeacherService(IMapper mapper,
							IRepository<Teacher> teachersRepo,
							IValidator<CreateTeacherDto> createValidator,
							IValidator<TeacherDto> editValidator)
		{
			this._mapper = mapper;
			this._teachersRepo = teachersRepo;
			this._createValidator = createValidator;
			this._editValidator = editValidator;
		}
		public void AddTeacher(CreateTeacherDto teacher)
		{
			try
			{
				_createValidator.ValidateAndThrow(teacher);
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

			var entity = _mapper.Map<Teacher>(teacher);

			try
			{
				_teachersRepo.Insert(entity);
				_teachersRepo.Save();
			}
			catch
			{
				throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
			}
		}

		public IEnumerable<TeacherViewDto> GetAllTeachers()
		{
			var entities = _teachersRepo.GetListBySpec(new TeacherSpecs.All());
			return _mapper.Map<IEnumerable<TeacherViewDto>>(entities);
		}

		public TeacherViewDto GetTeacherById(int teacherId)
		{
			if (teacherId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var entity = _teachersRepo.GetItemBySpec(new TeacherSpecs.ById(teacherId));
			if (entity == null) throw new HttpException("Teacher not found.", HttpStatusCode.NotFound);
			
			return _mapper.Map<TeacherViewDto>(entity);
		}

		public void RemoveTeacher(int teacherId)
		{
			if (teacherId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var entity = _teachersRepo.GetByID(teacherId);
			if (entity == null) throw new HttpException("Teacher not found.", HttpStatusCode.NotFound);

			try
			{
				_teachersRepo.Delete(teacherId);
				_teachersRepo.Save();
			}
			catch
			{
				throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
			}
		}

		public void UpdateTeacher(int teacherId, TeacherDto teacher)
		{
			if (teacherId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			try
			{
				_editValidator.ValidateAndThrow(teacher);
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

			var existingEntity = _teachersRepo.GetByID(teacherId);
			if (existingEntity == null) throw new HttpException("Teacher not found.", HttpStatusCode.NotFound);

			_mapper.Map(teacher, existingEntity);
			try
			{
				_teachersRepo.Update(existingEntity);
				_teachersRepo.Save();
			}
			catch
			{
				throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
			}
		}

		public async Task AddTeacherAsync(CreateTeacherDto teacher)
		{
			try
			{
				await _createValidator.ValidateAndThrowAsync(teacher);
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

			var entity = _mapper.Map<Teacher>(teacher);

			try
			{
				await _teachersRepo.InsertAsync(entity);
				await _teachersRepo.SaveAsync();
			}
			catch
			{
				throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
			}
		}

		public async Task<IEnumerable<TeacherViewDto>> GetAllTeachersAsync()
		{
			var entities = await _teachersRepo.GetListBySpecAsync(new TeacherSpecs.All());
			return _mapper.Map<IEnumerable<TeacherViewDto>>(entities);
		}

		public async Task<TeacherViewDto> GetTeacherByIdAsync(int teacherId)
		{
			if (teacherId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var entity = await _teachersRepo.GetItemBySpecAsync(new TeacherSpecs.ById(teacherId));
			if (entity == null) throw new HttpException("Teacher not found.", HttpStatusCode.NotFound);

			return _mapper.Map<TeacherViewDto>(entity);
		}

		public async Task RemoveTeacherAsync(int teacherId)
		{
			if (teacherId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			var entity = await _teachersRepo.GetByIDAsync(teacherId);
			if (entity == null) throw new HttpException("Teacher not found.", HttpStatusCode.NotFound);

			try
			{
				await _teachersRepo.DeleteAsync(entity);
				await _teachersRepo.SaveAsync();
			}
			catch
			{
				throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
			}
		}

		public async Task UpdateTeacherAsync(int teacherId, TeacherDto teacher)
		{
			if (teacherId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

			try
			{
				await _editValidator.ValidateAndThrowAsync(teacher);
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

			var existingEntity = await _teachersRepo.GetByIDAsync(teacherId);
			if (existingEntity == null) throw new HttpException("Teacher not found.", HttpStatusCode.NotFound);

			_mapper.Map(teacher, existingEntity);
			try
			{
				_teachersRepo.Update(existingEntity);
				await _teachersRepo.SaveAsync();
			}
			catch
			{
				throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
			}
		}

	}
}

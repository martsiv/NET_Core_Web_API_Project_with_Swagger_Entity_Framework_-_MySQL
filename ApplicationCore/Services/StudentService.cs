﻿using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using AutoMapper;
using FluentValidation;

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
			_createValidator.ValidateAndThrow(student);

			var entity = _mapper.Map<Student>(student);
			_studentsRepo.Insert(entity);
			_studentsRepo.Save();
		}

		public IEnumerable<StudentViewDto> GetAllStudents()
		{
			var entities = _studentsRepo.GetListBySpec(new StudentSpecs.All());
			return _mapper.Map<IEnumerable<StudentViewDto>>(entities);
		}

		public StudentViewDto? GetStudentById(int studentId)
		{
			var entity = _studentsRepo.GetItemBySpec(new StudentSpecs.ById(studentId));
			if (entity == null) return null;
			return _mapper.Map<StudentViewDto>(entity);
		}

		public IEnumerable<StudentDto> GetStudentsByCourse(int courseId)
		{
			var entities = _studentsRepo.GetListBySpec(new StudentSpecs.ByCourse(courseId));
			return _mapper.Map<IEnumerable<StudentDto>>(entities);
		}

		public void RemoveStudent(int studentId)
		{
			var entity = _studentsRepo.GetByID(studentId);
			if (entity != null)
			{
				_studentsRepo.Delete(studentId);
				_studentsRepo.Save();
			}
		}

		public void UpdateStudent(int studentId, StudentDto student)
		{
			_editValidator.ValidateAndThrow(student);

			var existingEntity = _studentsRepo.GetByID(studentId);
			if (existingEntity != null)
			{
				_mapper.Map(student, existingEntity);
				_studentsRepo.Update(existingEntity);
				_studentsRepo.Save();
			}
		}
	}
}

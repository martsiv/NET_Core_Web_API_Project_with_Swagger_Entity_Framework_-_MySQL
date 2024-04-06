using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using AutoMapper;
using static ApplicationCore.Specifications.StudentSpecs;

namespace ApplicationCore.Services
{
	public class StudentService : IStudentService
	{
		private readonly IMapper _mapper;
		private readonly IRepository<Student> _studentsRepo;
        public StudentService(IMapper mapper,
							IRepository<Student> studentRepo)
		{
			this._mapper = mapper;
			this._studentsRepo = studentRepo;
		}
		public void AddStudent(StudentDto student)
		{
			var entity = _mapper.Map<Student>(student);
			_studentsRepo.Insert(entity);
			_studentsRepo.Save();
		}

		public IEnumerable<StudentDto> GetAllStudents()
		{
			var entities = _studentsRepo.GetAll();
			return _mapper.Map<IEnumerable<StudentDto>>(entities);
		}

		public StudentDto? GetStudentById(int studentId)
		{
			var entity = _studentsRepo.GetByID(studentId);
			if (entity == null) return null;
			return _mapper.Map<StudentDto>(entity);
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

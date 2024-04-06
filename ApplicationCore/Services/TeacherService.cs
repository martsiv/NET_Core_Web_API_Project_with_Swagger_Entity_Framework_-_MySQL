using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using AutoMapper;

namespace ApplicationCore.Services
{
	public class TeacherService : ITeacherService
	{
		private readonly IMapper _mapper;
		private readonly IRepository<Teacher> _teachersRepo;
		public TeacherService(IMapper mapper,
							IRepository<Teacher> teachersRepo)
		{
			this._mapper = mapper;
			this._teachersRepo = teachersRepo;
		}
		public void AddTeacher(CreateTeacherDto teacher)
		{
			var entity = _mapper.Map<Teacher>(teacher);
			_teachersRepo.Insert(entity);
			_teachersRepo.Save();
		}

		public IEnumerable<TeacherViewDto> GetAllTeachers()
		{
			var entities = _teachersRepo.GetListBySpec(new TeacherSpecs.All());
			return _mapper.Map<IEnumerable<TeacherViewDto>>(entities);
		}

		public TeacherViewDto? GetTeacherById(int teacherId)
		{
			var entity = _teachersRepo.GetItemBySpec(new TeacherSpecs.ById(teacherId));
			if (entity == null) return null;
			return _mapper.Map<TeacherViewDto>(entity);
		}

		public void RemoveTeacher(int teacherId)
		{
			var entity = _teachersRepo.GetByID(teacherId);
			if (entity != null)
			{
				_teachersRepo.Delete(teacherId);
				_teachersRepo.Save();
			}
		}

		public void UpdateTeacher(int teacherId, TeacherDto teacher)
		{
			var existingEntity = _teachersRepo.GetByID(teacherId);
			if (existingEntity != null)
			{
				_mapper.Map(teacher, existingEntity);
				_teachersRepo.Update(existingEntity);
				_teachersRepo.Save();
			}
		}
	}
}

using ApplicationCore.CustomException;
using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
using FluentValidation;
using System.Net;

namespace ApplicationCore.Services
{
    public class LessonEventService : ILessonEventService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<LessonEvent> _lessonEventRepo;
        private readonly IValidator<CreateLessonEventDto> _createValidator;
        private readonly IValidator<LessonEventDto> _editValidator;

        public LessonEventService(IMapper mapper,
                            IRepository<LessonEvent> lessonEventRepo,
                            IValidator<CreateLessonEventDto> createValidator,
                            IValidator<LessonEventDto> editValidator)
        {
            this._mapper = mapper;
            this._lessonEventRepo = lessonEventRepo;
            this._createValidator = createValidator;
            this._editValidator = editValidator;
        }
        public void AddLessonEvent(CreateLessonEventDto lessonEventDto)
        {
            try
            {
                _createValidator.ValidateAndThrow(lessonEventDto);
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

            var entity = _mapper.Map<LessonEvent>(lessonEventDto);
            try
            {
                _lessonEventRepo.Insert(entity);
                _lessonEventRepo.Save();
            }
            catch
            {
                throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
            }
        }

        public Task AddLessonEventAsync(CreateCourseDto course)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LessonEventViewDto> GetAllLessonEvents()
        {
            var entities = _lessonEventRepo.GetAll();
            return _mapper.Map<IEnumerable<LessonEventViewDto>>(entities);
        }

        public Task<IEnumerable<LessonEventViewDto>> GetAllLessonEventsAsync()
        {
            throw new NotImplementedException();
        }

        public LessonEventDto GetLessonEventById(int lessonEventId)
        {
            if (lessonEventId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

            var entity = _lessonEventRepo.GetByID(lessonEventId);
            if (entity == null) throw new HttpException("Lesson event not found.", HttpStatusCode.NotFound);

            return _mapper.Map<LessonEventDto>(entity);
        }

        public Task<LessonEventDto> GetLessonEventByIdAsync(int lessonEventId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LessonEventDto> GetLessonEventByTeacher(int teacherId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LessonEventDto>> GetLessonEventByTeacherAsync(int teacherId)
        {
            throw new NotImplementedException();
        }

        public void RemoveLessonEvent(int lessonEventId)
        {
            if (lessonEventId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

            var entity = _lessonEventRepo.GetByID(lessonEventId);
            if (entity == null) throw new HttpException("Lesson event not found.", HttpStatusCode.NotFound);

            try
            {
                _lessonEventRepo.Delete(lessonEventId);
                _lessonEventRepo.Save();
            }
            catch
            {
                throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
            }
        }

        public Task RemoveLessonEventAsync(int courseId)
        {
            throw new NotImplementedException();
        }

        public void UpdateLessonEvent(int lessonEventId, LessonEventDto lessonEventDto)
        {
            if (lessonEventId < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

            try
            {
                _editValidator.ValidateAndThrow(lessonEventDto);
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

            var existingEntity = _lessonEventRepo.GetByID(lessonEventId);
            if (existingEntity == null) throw new HttpException("Course not found.", HttpStatusCode.NotFound);

            _mapper.Map(lessonEventDto, existingEntity);
            try
            {
                _lessonEventRepo.Update(existingEntity);
                _lessonEventRepo.Save();
            }
            catch
            {
                throw new HttpException("An error occurred while updating the database.", HttpStatusCode.BadRequest);
            }
        }

        public Task UpdateLessonEventAsync(int courseId, CourseDto course)
        {
            throw new NotImplementedException();
        }
    }
}

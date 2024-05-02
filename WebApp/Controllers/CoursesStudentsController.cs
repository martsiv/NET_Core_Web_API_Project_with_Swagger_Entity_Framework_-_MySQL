using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using AutoMapper;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CoursesStudentsController : ControllerBase
	{
		private readonly ICourseStudentService _courseStudentService;

		public CoursesStudentsController(ICourseStudentService courseStudentService)
		{
			this._courseStudentService = courseStudentService;
		}


		[HttpGet]
		public async Task<IActionResult> GetCourseStudent([FromQuery] int courseId, [FromQuery] int studentId)
		{
			var entity = await _courseStudentService.GetCourseStudentByIdsAsync(courseId, studentId);
			return Ok(entity);
		}

		[HttpPost]
		public async Task<IActionResult> PostCourseStudent([FromQuery] int courseId, [FromQuery] int studentId)
		{
			var courseStudent = new CourseStudentDto() { CourseId = courseId, StudentId = studentId };
			_courseStudentService.AddCourseStudent(courseStudent);
			return Ok();
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteCourseStudent([FromQuery] int courseId, [FromQuery] int studentId)
		{
			var courseStudent = await _courseStudentService.GetCourseStudentByIdsAsync(courseId, studentId);
			_courseStudentService.RemoveCourseStudent(courseStudent.Id);
			return Ok(courseStudent);
		}
	}
}

using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CoursesController : ControllerBase
	{
		private readonly ICourseService _courseService;

		public CoursesController(ICourseService courseService)
		{
			this._courseService = courseService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await _courseService.GetAllCoursesAsync());
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> Get([FromRoute] int id)
		{
			return Ok(await _courseService.GetCourseByIdAsync(id));
		}

		[HttpGet("byStudent")]
		public async Task<IActionResult> GetCoursesByStudent([FromQuery] int studentId)
		{
			var courses = await _courseService.GetCoursesByStudentAsync(studentId);
			if (courses.Count() == 0)
				return NotFound(new { message = "No items were found matching the given search criteria." });
			return Ok(courses);
		}

		[HttpGet("byTeacher")]
		public async Task<IActionResult> GetCoursesByTeacher([FromQuery] int teacherId)
		{
			var courses = await _courseService.GetCoursesByTeacherAsync(teacherId);
			if (courses.Count() == 0)
				return NotFound(new { message = "No items were found matching the given search criteria." });
			return Ok(courses);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateCourseDto course)
		{
			_courseService.AddCourse(course);
			return Ok();
		}

		[HttpPut]
		public async Task<IActionResult> Edit([FromBody] CourseDto courseDto)
		{
			_courseService.UpdateCourse(courseDto.Id, courseDto);
			return Ok();
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			_courseService.RemoveCourse(id);
			return Ok();
		}
	}
}

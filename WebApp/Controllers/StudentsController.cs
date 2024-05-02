using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StudentsController : ControllerBase
	{
		private readonly IStudentService _studentService;

		public StudentsController(IStudentService studentService)
		{
			this._studentService = studentService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await _studentService.GetAllStudentsAsync());
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> Get([FromRoute] int id)
		{
			return Ok(await _studentService.GetStudentByIdAsync(id));
		}

		[HttpGet("byCourse/{courseId:int}")]
		public async Task<IActionResult> GetStudentsByCourse([FromRoute] int courseId)
		{
			var students = await _studentService.GetStudentsByCourseAsync(courseId);
			if (students.Count() == 0)
				return NotFound(new { message = "No items were found matching the given search criteria." });
			return Ok(students);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateStudentDto student)
		{
			_studentService.AddStudent(student);
			return Ok();
		}

		[HttpPut]
		public async Task<IActionResult> Edit([FromBody] StudentDto studentDto)
		{
			_studentService.UpdateStudent(studentDto.Id, studentDto);
			return Ok();
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			_studentService.RemoveStudent(id);
			return Ok();
		}
	}
}

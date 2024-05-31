using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TeachersController : ControllerBase
	{
		private readonly ITeacherService _teacherService;

		public TeachersController(ITeacherService teacherService)
		{
			this._teacherService = teacherService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await _teacherService.GetAllTeachersAsync());
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> Get([FromRoute] int id)
		{
			return Ok(await _teacherService.GetTeacherByIdAsync(id));
		}


		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateTeacherDto teacher)
		{
			_teacherService.AddTeacher(teacher);
			return Ok();
		}

		[HttpPut]
		public async Task<IActionResult> Edit([FromBody] TeacherDto teacherDto)
		{
			_teacherService.UpdateTeacher(teacherDto.Id, teacherDto);
			return Ok();
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			_teacherService.RemoveTeacher(id);
			return Ok();
		}
	}
}

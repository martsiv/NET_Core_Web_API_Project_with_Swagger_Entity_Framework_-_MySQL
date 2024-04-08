using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApp.Controllers;

namespace WebApp.Tests.ControllersTests
{
	public class TeachersControllerTests
	{
		[Test]
		public async Task GetAll_ReturnsOkResult()
		{
			// Arrange
			var mockTeacherService = new Mock<ITeacherService>();
			var controller = new TeachersController(mockTeacherService.Object);

			// Act
			var result = await controller.GetAll();

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			mockTeacherService.Verify(service => service.GetAllTeachersAsync(), Times.Once);
		}

		[Test]
		public async Task Get_ReturnsOkResult()
		{
			// Arrange
			var mockTeacherService = new Mock<ITeacherService>();
			var controller = new TeachersController(mockTeacherService.Object);
			var teacherId = 1;

			// Act
			var result = await controller.Get(teacherId);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			mockTeacherService.Verify(service => service.GetTeacherByIdAsync(teacherId), Times.Once);
		}

		[Test]
		public async Task Create_ReturnsOkResult()
		{
			// Arrange
			var mockTeacherService = new Mock<ITeacherService>();
			var controller = new TeachersController(mockTeacherService.Object);
			var teacher = new CreateTeacherDto { Name = "John", Surname = "Doe" };

			// Act
			var result = await controller.Create(teacher);

			// Assert
			Assert.IsInstanceOf<OkResult>(result);
			mockTeacherService.Verify(service => service.AddTeacherAsync(teacher), Times.Once);
		}

		[Test]
		public async Task Edit_ReturnsOkResult()
		{
			// Arrange
			var mockTeacherService = new Mock<ITeacherService>();
			var controller = new TeachersController(mockTeacherService.Object);
			var teacherDto = new TeacherDto { Id = 1, Name = "John", Surname = "Doe" };

			// Act
			var result = await controller.Edit(teacherDto);

			// Assert
			Assert.IsInstanceOf<OkResult>(result);
			mockTeacherService.Verify(service => service.UpdateTeacherAsync(teacherDto.Id, teacherDto), Times.Once);
		}

		[Test]
		public async Task Delete_ReturnsOkResult()
		{
			// Arrange
			var mockTeacherService = new Mock<ITeacherService>();
			var controller = new TeachersController(mockTeacherService.Object);
			var teacherId = 1;

			// Act
			var result = await controller.Delete(teacherId);

			// Assert
			Assert.IsInstanceOf<OkResult>(result);
			mockTeacherService.Verify(service => service.RemoveTeacherAsync(teacherId), Times.Once);
		}
	}
}

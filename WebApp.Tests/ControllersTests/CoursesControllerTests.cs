using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApp.Controllers;



namespace WebApp.Tests.ControllersTests
{
	public class CoursesControllerTests
	{
		[Test]
		public async Task GetAll_ReturnsOkResult()
		{
			// Arrange
			var mockCourseService = new Mock<ICourseService>();
			var controller = new CoursesController(mockCourseService.Object);

			// Act
			var result = await controller.GetAll();

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			mockCourseService.Verify(service => service.GetAllCoursesAsync(), Times.Once);
		}

		[Test]
		public async Task Get_ReturnsOkResult()
		{
			// Arrange
			var mockCourseService = new Mock<ICourseService>();
			var controller = new CoursesController(mockCourseService.Object);
			var courseId = 1;

			// Act
			var result = await controller.Get(courseId);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			mockCourseService.Verify(service => service.GetCourseByIdAsync(courseId), Times.Once);
		}

		[Test]
		public async Task GetCoursesByStudent_ReturnsNotFound_WhenNoCoursesFound()
		{
			// Arrange
			int studentId = 1;
			var mockCourseService = new Mock<ICourseService>();
			mockCourseService.Setup(service => service.GetCoursesByStudentAsync(studentId)).ReturnsAsync(new List<CourseDto>());
			var controller = new CoursesController(mockCourseService.Object);

			// Act
			var result = await controller.GetCoursesByStudent(studentId);

			// Assert
			Assert.IsInstanceOf<NotFoundObjectResult>(result);
		}


		[Test]
		public async Task Create_ReturnsOkResult()
		{
			// Arrange
			var mockCourseService = new Mock<ICourseService>();
			var controller = new CoursesController(mockCourseService.Object);
			var course = new CreateCourseDto { Name = "Math", TeacherId = 1 };

			// Act
			var result = await controller.Create(course);

			// Assert
			Assert.IsInstanceOf<OkResult>(result);
			mockCourseService.Verify(service => service.AddCourseAsync(course), Times.Once);
		}

		[Test]
		public async Task Edit_ReturnsOkResult()
		{
			// Arrange
			var mockCourseService = new Mock<ICourseService>();
			var controller = new CoursesController(mockCourseService.Object);
			var courseDto = new CourseDto { Id = 1, Name = "Math", TeacherId = 1 };

			// Act
			var result = await controller.Edit(courseDto);

			// Assert
			Assert.IsInstanceOf<OkResult>(result);
			mockCourseService.Verify(service => service.UpdateCourseAsync(courseDto.Id, courseDto), Times.Once);
		}

		[Test]
		public async Task Delete_ReturnsOkResult()
		{
			// Arrange
			var mockCourseService = new Mock<ICourseService>();
			var controller = new CoursesController(mockCourseService.Object);
			var courseId = 1;

			// Act
			var result = await controller.Delete(courseId);

			// Assert
			Assert.IsInstanceOf<OkResult>(result);
			mockCourseService.Verify(service => service.RemoveCourseAsync(courseId), Times.Once);
		}
	}
}

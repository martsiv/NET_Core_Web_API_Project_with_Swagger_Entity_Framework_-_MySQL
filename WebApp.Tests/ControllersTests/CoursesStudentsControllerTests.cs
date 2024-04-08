using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApp.Controllers;

namespace WebApp.Tests.ControllersTests
{
	[TestFixture]
	public class CoursesStudentsControllerTests
	{
		[Test]
		public async Task GetCourseStudent_ReturnsOkResult()
		{
			// Arrange
			var mockCourseStudentService = new Mock<ICourseStudentService>();
			var controller = new CoursesStudentsController(mockCourseStudentService.Object);
			var courseId = 1;
			var studentId = 1;
			var courseStudent = new CourseStudentDto { CourseId = courseId, StudentId = studentId };
			mockCourseStudentService.Setup(service => service.GetCourseStudentByIdsAsync(courseId, studentId)).ReturnsAsync(courseStudent);

			// Act
			var result = await controller.GetCourseStudent(courseId, studentId);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var okResult = result as OkObjectResult;
			Assert.IsNotNull(okResult);
			Assert.AreEqual(courseStudent, okResult.Value);
		}

		[Test]
		public async Task PostCourseStudent_ReturnsOkResult()
		{
			// Arrange
			var mockCourseStudentService = new Mock<ICourseStudentService>();
			var controller = new CoursesStudentsController(mockCourseStudentService.Object);
			var courseId = 1;
			var studentId = 1;

			// Act
			var result = await controller.PostCourseStudent(courseId, studentId);

			// Assert
			Assert.IsInstanceOf<OkResult>(result);
			mockCourseStudentService.Verify(service => service.AddCourseStudentAsync(It.IsAny<CourseStudentDto>()), Times.Once);
		}

		[Test]
		public async Task DeleteCourseStudent_ReturnsOkResult()
		{
			// Arrange
			var mockCourseStudentService = new Mock<ICourseStudentService>();
			var controller = new CoursesStudentsController(mockCourseStudentService.Object);
			var courseId = 1;
			var studentId = 1;
			var courseStudentId = 123;
			var courseStudent = new CourseStudentDto { Id = courseStudentId, CourseId = courseId, StudentId = studentId };
			mockCourseStudentService.Setup(service => service.GetCourseStudentByIdsAsync(courseId, studentId)).ReturnsAsync(courseStudent);

			// Act
			var result = await controller.DeleteCourseStudent(courseId, studentId);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var okResult = result as OkObjectResult;
			Assert.IsNotNull(okResult);
			Assert.AreEqual(courseStudent, okResult.Value);
			mockCourseStudentService.Verify(service => service.RemoveCourseStudentAsync(courseStudentId), Times.Once);
		}
	}

}

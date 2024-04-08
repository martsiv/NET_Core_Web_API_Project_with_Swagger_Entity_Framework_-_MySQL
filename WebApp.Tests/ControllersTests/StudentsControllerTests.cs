using ApplicationCore.CustomException;
using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using WebApp.Controllers;

namespace WebApp.Tests.ControllersTests
{
	[TestFixture]
	public class StudentsControllerTests
	{
		
		[Test]
		public async Task GetAll_ReturnsOkResult_WithListOfStudents()
		{
			// Arrange
			var mockStudentService = new Mock<IStudentService>();
			var students = new List<StudentViewDto>
			{
				new StudentViewDto { Id = 1, Name = "John" },
				new StudentViewDto { Id = 2, Name = "Jane" }
			};
			mockStudentService.Setup(service => service.GetAllStudentsAsync()).ReturnsAsync(students);
			var controller = new StudentsController(mockStudentService.Object);

			// Act
			var result = await controller.GetAll();

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var okResult = result as OkObjectResult;
			Assert.IsNotNull(okResult);
			var returnedStudents = okResult.Value as IEnumerable<StudentViewDto>;
			Assert.IsNotNull(returnedStudents);
			Assert.AreEqual(2, returnedStudents.Count());
		}

		[Test]
		public async Task Get_ReturnsOkResult_WithStudent()
		{
			// Arrange
			int studentId = 1;
			var mockStudentService = new Mock<IStudentService>();
			var student = new StudentViewDto { Id = studentId, Name = "John" };
			mockStudentService.Setup(service => service.GetStudentByIdAsync(studentId)).ReturnsAsync(student);
			var controller = new StudentsController(mockStudentService.Object);

			// Act
			var result = await controller.Get(studentId);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var okResult = result as OkObjectResult;
			Assert.IsNotNull(okResult);
			var returnedStudent = okResult.Value as StudentViewDto;
			Assert.IsNotNull(returnedStudent);
			Assert.AreEqual(studentId, returnedStudent.Id);
		}

		[Test]
		public async Task GetStudentsByCourse_ReturnsNotFound_WhenNoStudentsFound()
		{
			// Arrange
			int courseId = 1;
			var mockStudentService = new Mock<IStudentService>();
			mockStudentService.Setup(service => service.GetStudentsByCourseAsync(courseId)).ReturnsAsync(new List<StudentDto>());
			var controller = new StudentsController(mockStudentService.Object);

			// Act
			var result = await controller.GetStudentsByCourse(courseId);

			// Assert
			Assert.IsInstanceOf<NotFoundObjectResult>(result);
		}

		[Test]
		public async Task Create_ReturnsOkResult_AfterAddingStudent()
		{
			// Arrange
			var mockStudentService = new Mock<IStudentService>();
			var controller = new StudentsController(mockStudentService.Object);
			var student = new CreateStudentDto { Name = "John", Surname = "Doe", BirthDate = DateTime.Now, GroupName = "Group A" };

			// Act
			var result = await controller.Create(student);

			// Assert
			Assert.IsInstanceOf<OkResult>(result);
			mockStudentService.Verify(service => service.AddStudentAsync(student), Times.Once);
		}

		[Test]
		public async Task Edit_ReturnsOkResult_AfterUpdatingStudent()
		{
			// Arrange
			var mockStudentService = new Mock<IStudentService>();
			var controller = new StudentsController(mockStudentService.Object);
			var studentDto = new StudentDto { Id = 1, Name = "John", Surname = "Doe", BirthDate = DateTime.Now, GroupName = "Group A" };

			// Act
			var result = await controller.Edit(studentDto);

			// Assert
			Assert.IsInstanceOf<OkResult>(result);
			mockStudentService.Verify(service => service.UpdateStudentAsync(studentDto.Id, studentDto), Times.Once);
		}

		[Test]
		public async Task Delete_ReturnsOkResult_AfterRemovingStudent()
		{
			// Arrange
			var mockStudentService = new Mock<IStudentService>();
			var controller = new StudentsController(mockStudentService.Object);
			int studentId = 1;

			// Act
			var result = await controller.Delete(studentId);

			// Assert
			Assert.IsInstanceOf<OkResult>(result);
			mockStudentService.Verify(service => service.RemoveStudentAsync(studentId), Times.Once);
		}
	}
}

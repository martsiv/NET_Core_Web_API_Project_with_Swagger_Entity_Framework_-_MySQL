using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using FluentValidation;
using ApplicationCore.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationCore
{
	public static class ServiceExtensions
	{
		public static void AddAutoMapper(this IServiceCollection services)
		{
			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
		}

		public static void AddCustomServices(this IServiceCollection services)
		{
			services.AddScoped<ICourseService, CourseService>();
			services.AddScoped<IStudentService, StudentService>();
			services.AddScoped<ITeacherService, TeacherService>();
			services.AddScoped<ICourseStudentService, CourseStudentService>();
		}

		public static void AddFluentValidator(this IServiceCollection services)
		{
			services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
		}
	}
}

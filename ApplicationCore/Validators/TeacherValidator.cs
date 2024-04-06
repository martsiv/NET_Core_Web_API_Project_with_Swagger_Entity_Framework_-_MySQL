using ApplicationCore.DTOs;
using FluentValidation;

namespace ApplicationCore.Validators
{
	public class TeacherValidator : AbstractValidator<TeacherDto>
	{
		public TeacherValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty();

			RuleFor(x => x.Surname)
				.NotEmpty();

			RuleFor(x => x.Id)
				.NotEmpty()
				.GreaterThanOrEqualTo(1);
		}
	}
}

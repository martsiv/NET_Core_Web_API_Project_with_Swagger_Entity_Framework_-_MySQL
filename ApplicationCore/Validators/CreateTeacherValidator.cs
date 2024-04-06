using ApplicationCore.DTOs;
using FluentValidation;

namespace ApplicationCore.Validators
{
	public class CreateTeacherValidator : AbstractValidator<CreateTeacherDto>
	{
		public CreateTeacherValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty();

			RuleFor(x => x.Surname)
				.NotEmpty();
		}
	}
}

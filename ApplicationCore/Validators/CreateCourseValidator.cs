using ApplicationCore.DTOs;
using FluentValidation;

namespace ApplicationCore.Validators
{
	public class CreateCourseValidator : AbstractValidator<CreateCourseDto>
	{
		public CreateCourseValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty();

			RuleFor(x => x.TeacherId)
				.NotEmpty()
				.GreaterThanOrEqualTo(1);
		}
	}
}

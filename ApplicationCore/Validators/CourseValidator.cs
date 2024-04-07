using ApplicationCore.DTOs;
using FluentValidation;

namespace ApplicationCore.Validators
{
	public class CourseValidator : AbstractValidator<CourseDto>
	{
		public CourseValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty();

			RuleFor(x => x.Id)
				.NotEmpty()
				.GreaterThanOrEqualTo(1);

			RuleFor(x => x.TeacherId)
				.NotEmpty()
				.GreaterThanOrEqualTo(1);
		}
	}
}

using ApplicationCore.DTOs;
using FluentValidation;

namespace ApplicationCore.Validators
{
	public class StudentValidator : AbstractValidator<StudentDto>
	{
		public StudentValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty();

			RuleFor(x => x.Surname)
				.NotEmpty();

			RuleFor(x => x.GroupName)
				.NotEmpty();

			RuleFor(x => x.Id)
				.NotEmpty()
				.GreaterThanOrEqualTo(1);
		}
	}
}

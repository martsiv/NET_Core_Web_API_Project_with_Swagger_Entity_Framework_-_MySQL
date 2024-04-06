using ApplicationCore.DTOs;
using FluentValidation;

namespace ApplicationCore.Validators
{
	public class CreateStudentValidator : AbstractValidator<CreateStudentDto>
	{
		public CreateStudentValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty();

			RuleFor(x => x.Surname)
				.NotEmpty();

			RuleFor(x => x.GroupName)
				.NotEmpty();
		}
	}
}

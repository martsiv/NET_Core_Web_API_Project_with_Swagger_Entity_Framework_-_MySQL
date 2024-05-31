using ApplicationCore.DTOs;
using FluentValidation;

namespace ApplicationCore.Validators
{
    public class CreateLessonEventValidator : AbstractValidator<CreateLessonEventDto>
    {
        public CreateLessonEventValidator()
        {
            RuleFor(x => x.TeacherId)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);
        }
    }
}

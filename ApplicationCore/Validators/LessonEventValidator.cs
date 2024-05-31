using ApplicationCore.DTOs;
using FluentValidation;

namespace ApplicationCore.Validators
{
    public class LessonEventValidator : AbstractValidator<LessonEventDto>
    {
        public LessonEventValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.TeacherId)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);
        }
    }
}

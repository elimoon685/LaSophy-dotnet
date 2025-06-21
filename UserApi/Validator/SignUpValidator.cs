using FluentValidation;
using UserApi.DTO;

namespace UserApi.Validator
{
    public class SignUpValidator:AbstractValidator<RegisterRequestDto>
    {
        public SignUpValidator()
        {

            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).MinimumLength(6);
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords do not match.");
        }

    }
}

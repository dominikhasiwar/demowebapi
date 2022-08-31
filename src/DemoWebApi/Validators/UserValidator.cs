using DemoWebApi.Models;
using FluentValidation;

namespace DemoWebApi.Validators
{
    public class UserValidator : AbstractValidator<SaveUserModel>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();

            RuleFor(x => x.LastName).NotEmpty();

            RuleFor(x => x.UserName).NotEmpty();
        }
    }
}

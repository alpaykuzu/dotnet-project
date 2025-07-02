using API.DTOs.User;
using FluentValidation;
using System;

namespace API.Validator
{
    public class RegisterValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterValidator() 
        {
            RuleFor(x => x.FirstName).NotNull().MinimumLength(2).MaximumLength(15);
            RuleFor(x => x.LastName).NotNull().MinimumLength(2).MaximumLength(15);
            RuleFor(x => x.Email).NotNull().MinimumLength(2).MaximumLength(30).EmailAddress();
            RuleFor(x => x.Password).NotNull().MinimumLength(10).MaximumLength(50);
        }
    }
}

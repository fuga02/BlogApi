using System.Text.RegularExpressions;
using BlogApi.Entities;
using FluentValidation;

namespace BlogApi.Validators;

public class UserValidator:AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Name).NotNull().Length(3,32).WithErrorCode("Please fill the context");
        RuleFor(u => u.Username).NotNull().Length(6, 32).NotEqual("fuga02").NotEqual("fuga_02");
        RuleFor(u => u.PasswordHash).NotNull().Length(6, 32);

    }
}
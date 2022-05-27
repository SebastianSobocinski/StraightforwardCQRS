using FluentValidation;

namespace StraightforwardCQRS.Samples.Common.Commands.CreateUser;

public class CreateUserValidator : AbstractValidator<CreateUser>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Dto.Name).NotEmpty().NotEmpty().MinimumLength(5);
    }
}
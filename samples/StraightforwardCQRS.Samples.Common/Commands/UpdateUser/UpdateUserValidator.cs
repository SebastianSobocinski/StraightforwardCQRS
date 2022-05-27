using FluentValidation;

namespace StraightforwardCQRS.Samples.Common.Commands.UpdateUser;

public class UpdateUserValidator : AbstractValidator<UpdateUser>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Dto.Name).NotEmpty().NotEmpty().MinimumLength(5);
    }
}
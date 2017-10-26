using FluentValidation;

namespace Oxagile.Internal.Api.Dtos.Validation
{
    public class CreateCompanyValidator : AbstractValidator<CreateCompanyDto>
    {
        public CreateCompanyValidator()
        {
            RuleFor(_ => _.Name)
                .NotEmpty()
                .Length(2, 32);
        }
    }
}
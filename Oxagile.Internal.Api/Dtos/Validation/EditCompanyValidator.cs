using FluentValidation;

namespace Oxagile.Internal.Api.Dtos.Validation
{
    public class EditCompanyValidator : AbstractValidator<EditCompanyDto>
    {
        public EditCompanyValidator()
        {
            RuleFor(_ => _.Name)
                .NotEmpty()
                .Length(2, 32);
        }
    }
}
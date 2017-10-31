using System;
using FluentValidation;
using Microsoft.Extensions.Options;
using Oxagile.Demos.Data.Repositories;

namespace Oxagile.Demos.Api.Dtos.Validation
{
    public class EditUserValidator : AbstractValidator<EditUserDto>
    {
        public EditUserValidator(ICompanyRepository companyRepository, IOptions<Settings> options)
        {
            RuleFor(_ => _.Name)
                .NotEmpty()
                .Length(2, 32);
            RuleFor(_ => _.Surname)
                .NotEmpty()
                .Length(2, 32);
            RuleFor(_ => _.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(_ => _.BirthDate)
                .NotEmpty()
                .LessThan(DateTime.Today)
                .WithMessage("You cannot enter a birth date in the future.");
            RuleFor(_ => _.CompanyId)
                .NotEmpty()
                .WithMessage("You must specify a company for the user.");
            RuleFor(_ => _.CompanyId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .MustAsync(async (companyId, token) =>
                {
                    var company = await companyRepository.Get(companyId);
                    return company != null;
                })
                .WithMessage("Invalid company id. Company does not exist")
                .MustAsync(async (companyId, token) =>
                {
                    var company = await companyRepository.Get(companyId);
                    return company.Users.Count < options.Value.UsersPerCompanyAllowed;
                })
                .WithMessage("Company's user limit reached");
        }
    }
}
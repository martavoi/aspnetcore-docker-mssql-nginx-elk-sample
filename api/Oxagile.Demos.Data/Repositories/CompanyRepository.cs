using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Oxagile.Demos.Data.Entities;

namespace Oxagile.Demos.Data.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly UserCompanyContext context;

        public CompanyRepository(UserCompanyContext context)
        {
            this.context = context;
        }

        public async Task<Company> Create(Company c)
        {
            await context.Companies.AddAsync(c);
            return c;
        }

        public async Task<IEnumerable<Company>> Get()
        {
            return await context
                .Companies
                .Include(c => c.Users)
                .ToArrayAsync();   
        }

        public async Task<Company> Get(int id)
        {
            var company = await context
                .Companies
                .Include(c => c.Users)
                .SingleOrDefaultAsync(c => c.Id == id);

            return company;
        }

        public Company Update(Company c)
        {
            context.Companies.Update(c);
            return c;
        }

        public void Delete(Company company)
        {
            context.Companies.Remove(company);
        }
    }   
}
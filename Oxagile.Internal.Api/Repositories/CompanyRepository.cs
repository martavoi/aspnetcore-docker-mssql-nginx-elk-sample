using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Oxagile.Internal.Api.Entities;

namespace Oxagile.Internal.Api.Repositories
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
            await context.SaveChangesAsync();
            return c;
        }

        public async Task<IEnumerable<Company>> Get()
        {
            return await context
                .Companies
                .AsNoTracking()
                .ToArrayAsync();   
        }

        public async Task<Company> Get(int id)
        {
            var company = await context
                .Companies
                .Include(c => c.Users)
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == id);

            return company;
        }

        public async Task<Company> Update(Company c)
        {
            context.Companies.Update(c);
            await context.SaveChangesAsync();
            return c;
        }

        public async Task<bool> Delete(int id)
        {
            var company = await context
                .Companies
                .SingleOrDefaultAsync(c => c.Id == id);
            if (company == null)
            {
                throw new ArgumentException($"Company with id = {id} doesn't exist");
            }

            context.Companies.Remove(company);
            var result = await context.SaveChangesAsync();
            return result == 0;
        }
    }   
}
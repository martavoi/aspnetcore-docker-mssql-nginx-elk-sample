using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Oxagile.Demos.Data.Entities;

namespace Oxagile.Demos.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserCompanyContext context;

        public UserRepository(UserCompanyContext context)
        {
            this.context = context;
        }

        public async Task<User> Create(User c)
        {
            await context.Users.AddAsync(c);
            return c;
        }

        public async Task<IEnumerable<User>> Get()
        {
            return await context
                .Users
                .Include(u => u.Company)
                .ToArrayAsync();   
        }

        public async Task<User> Get(int id)
        {
            var user = await context
                .Users
                .Include(c => c.Company)
                .SingleOrDefaultAsync(c => c.Id == id);

            return user;
        }

        public User Update(User u)
        {
            context.Users.Update(u);
            return u;
        }

        public void Delete(User u)
        {
            context.Users.Remove(u);
        }
    }   
}
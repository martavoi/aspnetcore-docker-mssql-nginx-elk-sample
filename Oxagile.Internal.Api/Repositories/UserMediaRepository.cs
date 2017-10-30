using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Oxagile.Internal.Api.Entities;

namespace Oxagile.Internal.Api.Repositories
{
    public class UserMediaRepository : IUserMediaRepository
    {
        private readonly UserCompanyContext context;

        public UserMediaRepository(UserCompanyContext context)
        {
            this.context = context;
        }

        public async Task<UserMedia> Add(UserMedia m)
        {
            await context.UserMedia.AddAsync(m);
            await context.SaveChangesAsync();
            return m;
        }

        public async Task<UserMedia> GetUserPic(int userId)
        {
            return await context
                .UserMedia
                .Where(m => m.Rel == MediaRelationType.UserPic)
                .OrderByDescending(m => m.Uploaded)
                .FirstOrDefaultAsync();
        }
    }   
}
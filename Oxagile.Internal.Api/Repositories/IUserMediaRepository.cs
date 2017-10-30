using System.Threading.Tasks;
using Oxagile.Internal.Api.Entities;

namespace Oxagile.Internal.Api.Repositories
{
    public interface IUserMediaRepository
    {
        Task<UserMedia> Add(UserMedia m);
        Task<UserMedia> GetUserPic(int userId);
    }
}
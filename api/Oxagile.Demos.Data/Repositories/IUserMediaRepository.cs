using System.Threading.Tasks;
using Oxagile.Demos.Data.Entities;

namespace Oxagile.Demos.Data.Repositories
{
    public interface IUserMediaRepository
    {
        Task<UserMedia> Add(UserMedia m);
        Task<UserMedia> GetUserPic(int userId);
    }
}
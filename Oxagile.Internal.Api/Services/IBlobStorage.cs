using System.Threading.Tasks;

namespace Oxagile.Internal.Api.Services
{
    public interface IBlobStorage
    {
        Task<string> SaveAsync(byte[] stream);
        Task<byte[]> LoadAsync(string blobPath);
        bool Exists(string blobPath);
    }
}
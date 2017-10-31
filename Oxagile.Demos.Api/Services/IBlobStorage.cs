using System.Threading.Tasks;

namespace Oxagile.Demos.Api.Services
{
    public interface IBlobStorage
    {
        Task<string> SaveAsync(byte[] stream);
        Task<byte[]> LoadAsync(string blobPath);
        bool Exists(string blobPath);
    }
}
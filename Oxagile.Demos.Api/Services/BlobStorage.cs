using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Oxagile.Demos.Api.Services
{
    public class BlobStorage : IBlobStorage
    {
        private readonly Settings settings;
        private readonly IHostingEnvironment env;

        public BlobStorage(IHostingEnvironment environment, IOptions<Settings> options)
        {
            this.settings = options.Value;
            this.env = environment;
        }

        public async Task<string> SaveAsync(byte[] stream)
        {
            var fileStoragePath = Path.Combine(env.ContentRootPath, settings.FileStoragePath);
            EnsureDirExists(fileStoragePath);

            var fileName = Path.GetRandomFileName();
            using (var fileStream =
                new FileStream(Path.Combine(fileStoragePath, fileName), FileMode.Create))
            {
                await fileStream.WriteAsync(stream, 0, stream.Length);
            }

            return fileName;
        }

        public async Task<byte[]> LoadAsync(string blobPath)
        {
            var path = ExpandBlobPath(blobPath);
            return await File.ReadAllBytesAsync(path);
        }

        public bool Exists(string blobPath)
        {
            var path = ExpandBlobPath(blobPath);
            return System.IO.File.Exists(path);
        }

        private string ExpandBlobPath(string blobPath)
        {
            return Path.Combine(env.ContentRootPath, settings.FileStoragePath, blobPath);
        }

        private void EnsureDirExists(string fileStoragePath)
        {
            if (!Directory.Exists(fileStoragePath))
            {
                Directory.CreateDirectory(fileStoragePath);
            }
        }
    }
}
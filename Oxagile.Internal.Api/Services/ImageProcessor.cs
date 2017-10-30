using System.Collections.Generic;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Oxagile.Internal.Api.Services
{
    public class ImageProcessor : IImageProcessor
    {
        public IEnumerable<string> SupportedMimeTypes => new [] { "image/jpeg", "image/jpg" };

        public string GetImageMimeType(byte[] stream)
        {
            IImageFormat format;
            using (var image = Image.Load(stream, out format))
            {
                return format.DefaultMimeType;
            }
        }

        public string GetImageExtension(byte[] stream)
        {
            IImageFormat format;
            using (var image = Image.Load(stream, out format))
            {
                return format.FileExtensions.First();
            }
        }

        public (byte[], string) ToJpeg(byte[] stream, int w, int h)
        {
            IImageFormat format;
            using (var image = Image.Load(stream, out format))
            {
                image.Mutate(x => x.Resize(w, h));
                using (var memStream = new MemoryStream())
                {
                    image.Save(memStream, new JpegEncoder());
                    return (memStream.ToArray(), format.DefaultMimeType);
                }
            }
        }
    }
}
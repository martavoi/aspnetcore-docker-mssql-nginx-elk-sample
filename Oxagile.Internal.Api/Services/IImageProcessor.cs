using System.Collections.Generic;
using System.IO;

namespace Oxagile.Internal.Api.Services
{
    public interface IImageProcessor
    {
        IEnumerable<string> SupportedMimeTypes {get;}
        string GetImageMimeType(byte[] stream);
        string GetImageExtension(byte[] stream);
        (byte[], string) ToJpeg(byte[] stream, int w, int h);
    }
}
using Microsoft.AspNetCore.Mvc;

namespace Oxagile.Internal.Api.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string ActionUserPic(this IUrlHelper helper, string blobPath)
        {
            return helper.Action("Get", "Pic", new { id = $"{blobPath}", w = 200, h = 200});
        }
    }
}
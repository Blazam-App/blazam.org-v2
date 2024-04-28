using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
namespace blazam.org.Shared
{
    public static class Helpers
    {
        public static bool IsUrlLocalToHost(this string url)
        {
            if (url.StartsWith("https://localhost")) return true;
            if (url == "") return true;
            return url[0] == '/' && (url.Length == 1 ||
                    url[1] != '/' && url[1] != '\\') ||   // "/" or "/foo" but not "//" or "/\"
                    url.Length > 1 &&
                     url[0] == '~' && url[1] == '/';   // "~/" or "~/foo"
        }

        public static bool IsNullOrEmpty(this string? str)
        {
            return str == null || str.Length < 1;
        }

        public static SecureString ToSecureString(this string plainText)
        {
            return new NetworkCredential("", plainText).SecurePassword;
        }
        public static string ToPlainText(this SecureString? secureString)
        {
            if (secureString == null) return string.Empty;
            IntPtr bstrPtr = Marshal.SecureStringToBSTR(secureString);
            try
            {
                var plainText = Marshal.PtrToStringBSTR(bstrPtr);
                if (plainText == null)
                    plainText = string.Empty;
                return plainText;

            }
            finally
            {
                Marshal.ZeroFreeBSTR(bstrPtr);
            }
        }
        /// <summary>
        /// Resizes a raw byte array, assumed to be an image, to the maximum dimension provided
        /// </summary>
        /// <param name="rawImage"></param>
        /// <param name="maxDimension"></param>
        /// <param name="cropToSquare"></param>
        /// <returns></returns>
        public static byte[] ResizeRawImage(this byte[] rawImage, int maxDimension, bool cropToSquare = false)
        {
            using (var image = Image.Load(rawImage))
            {
                if (image.Height > image.Width)
                {
                    if (cropToSquare)
                        image.Mutate(x => x.Crop(image.Width, image.Width));
                    image.Mutate(x => x.Resize(0, maxDimension));

                }
                else
                {
                    if (cropToSquare)
                        image.Mutate(x => x.Crop(image.Height, image.Height));
                    image.Mutate(x => x.Resize(maxDimension, 0));
                }
                using (var ms = new MemoryStream())
                {
                    image.SaveAsPng(ms);
                    return ms.ToArray();
                }
            }
        }
        public static async Task<byte[]?> ToByteArrayAsync(this IBrowserFile file, int maxReadBytes = 5000000)
        {
            byte[] fileBytes;
            using (var stream = file.OpenReadStream(5000000))
            {
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }
            }
            return fileBytes;
        }
    }
}

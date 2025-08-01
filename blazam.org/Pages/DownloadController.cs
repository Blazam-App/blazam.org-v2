using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using Octokit;

namespace blazam.org.Pages
{
    [Route("[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private IWebHostEnvironment _hostingEnvironment;

        public DownloadController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        [HttpGet("zip")]
        public async Task<IActionResult> GetZip()
        {
            var client = new GitHubClient(new ProductHeaderValue("BLAZAM-APP"));
            var releases = await client.Repository.Release.GetAll("Blazam-App", "Blazam");
            var branchReleases = releases.Where(r => r.TagName.Contains("Release", StringComparison.OrdinalIgnoreCase));
            var latestRelese = branchReleases.FirstOrDefault()?.Assets.FirstOrDefault();
            if (latestRelese != null)
            {
                // Create a Uri object to guarantee you have an absolute URL
                var downloadUri = new Uri(latestRelese.BrowserDownloadUrl);

                // Redirect using the absolute URI string
                return Redirect(downloadUri.AbsoluteUri);
            }
            return NotFound();
        }
        [HttpGet("setup.exe")]
        public async Task<IActionResult> GetExe()
        {
            var client = new GitHubClient(new ProductHeaderValue("BLAZAM-APP"));
            var releases = await client.Repository.Release.GetAll("Blazam-App", "BlazamSetup");
            var latestRelese = releases.FirstOrDefault()?.Assets.FirstOrDefault();
            if (latestRelese != null)
            {
                // Create a Uri object to guarantee you have an absolute URL
                var downloadUri = new Uri(latestRelese.BrowserDownloadUrl);
                // Redirect using the absolute URI string
                return Redirect(downloadUri.AbsoluteUri);
            }
            return NotFound();
        }

        [HttpGet("beta")]
        public async Task<IActionResult> GetBeta()
        {
            var client = new GitHubClient(new ProductHeaderValue("BLAZAM-APP"));
            var releases = await client.Repository.Release.GetAll("Blazam-App", "Blazam");
            var branchReleases = releases.Where(r => r.TagName.Contains("BetaDev1", StringComparison.OrdinalIgnoreCase));
            var latestRelese = branchReleases.FirstOrDefault()?.Assets.FirstOrDefault();
            if (latestRelese != null)
            {
                // Create a Uri object to guarantee you have an absolute URL
                var downloadUri = new Uri(latestRelese.BrowserDownloadUrl);

                // Redirect using the absolute URI string
                return Redirect(downloadUri.AbsoluteUri);
            }
            return NotFound();
        }

            [HttpGet("linux_install.sh")]
        public IActionResult GetLinuxScript()
        {
            return Redirect("https://raw.githubusercontent.com/Blazam-App/BLAZAM/refs/heads/v1-Dev-Beta/linux_install.sh");
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Octokit;

namespace blazam.org.Pages.API
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
                return Redirect(latestRelese.BrowserDownloadUrl);
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
                return Redirect(latestRelese.BrowserDownloadUrl);
            }
            return NotFound();
        }

        [HttpGet("linux_install.sh")]
        public IActionResult GetLinuxScript()
        {
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "install.sh");
            return PhysicalFile(path, "text/plain", "install.sh");
        }
    }
}

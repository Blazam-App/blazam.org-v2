using Octokit;

namespace blazam.org
{
    public class Update
    {

        public static async Task<ManualDownload?> GetLatestUpdateUri()
        {
     
                var client = new GitHubClient(new ProductHeaderValue("BLAZAM-APP"));
                var releases = await client.Repository.Release.GetAll("Blazam-App", "Blazam");
                var branchReleases = releases.Where(r => r.TagName.Contains("Release", StringComparison.OrdinalIgnoreCase));
                var latestRelese = branchReleases.FirstOrDefault()?.Assets.FirstOrDefault();
                var filename = Path.GetFileNameWithoutExtension(latestRelese.Name);
                var latestVersion = filename.Substring(filename.IndexOf("-v") + 2);
                if (latestRelese != null)
                {



                    return new ManualDownload
                    {
                        DownloadUri = latestRelese.BrowserDownloadUrl,
                        Version = latestVersion,
                    };


                }
            return null;
            

        }

        public static async Task<ManualDownload?> GetLatestSetupUri()
        {

            var client = new GitHubClient(new ProductHeaderValue("BLAZAM-APP"));
            var releases = await client.Repository.Release.GetAll("Blazam-App", "BlazamSetup");
            var branchReleases = releases;
            var latestRelese = branchReleases.FirstOrDefault()?.Assets.FirstOrDefault();
            var filename = Path.GetFileNameWithoutExtension(latestRelese.Name);
            //var latestVersion = filename.Substring(filename.IndexOf(".exe"));
            if (latestRelese != null)
            {



                return new ManualDownload
                {
                    DownloadUri = latestRelese.BrowserDownloadUrl,
                };


            }
            return null;


        }
    }
    public class ManualDownload
    {
        public string Version { get; set; }
        public string DownloadUri { get; set; }
    }
}

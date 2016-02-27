using System;
using System.Threading.Tasks;
using Hakudu.Bootstrapper.GitHub;
using SemVersion;

namespace Hakudu.Bootstrapper.Repositories
{
    public class GitHubPackageManager
    {
        const string GITHUB_USER = "hakudu";
        const string GITHUB_REPO = "hakudu";
        const string RELEASE_ASSET = "hakudu-engine.zip";

        readonly GitHubApiClient _githubClient;

        public GitHubPackageManager(string userAgent)
        {
            if (userAgent == null)
                throw new ArgumentNullException(nameof(userAgent));

            _githubClient = new GitHubApiClient(GITHUB_USER, GITHUB_REPO, userAgent);
        }

        public async Task<PackageInfo> GetLatest(SemanticVersion existingVersion, bool preRelease = false)
        {
            GitHubReleaseList releases = null;

            var releaseMatcher = new GitHubReleaseMatcher(RELEASE_ASSET)
            {
                AllowPreRelease = preRelease
            };

            do
            {
                releases = await _githubClient.GetReleases(releases);

                var foundPackage = releaseMatcher.FindMatching(releases.Releases);

                if (foundPackage != null)
                {
                    if (existingVersion == null || foundPackage.Version > existingVersion)
                        return foundPackage;

                    return null;
                }

            } while (releases.HasNext);

            return null;
        }

        public async Task<string> Download(PackageInfo package)
        {
            if (package == null)
                throw new ArgumentNullException(nameof(package));

            return await _githubClient.DownloadAsset(package.GitHubAsset);
        }
    }
}

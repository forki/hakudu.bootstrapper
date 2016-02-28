using System;
using Hakudu.Bootstrapper.GitHub;
using SemVersion;

namespace Hakudu.Bootstrapper.Repositories
{
    public class PackageInfo
    {
        public SemanticVersion Version { get; }
        public bool PreRelease { get; }

        public GitHubReleaseAsset GitHubAsset { get; }

        public PackageInfo(SemanticVersion version, bool preRelease, GitHubReleaseAsset githubAsset)
        {
            if (version == null)
                throw new ArgumentNullException(nameof(version));

            if (githubAsset == null)
                throw new ArgumentNullException(nameof(githubAsset));

            Version = version;
            PreRelease = preRelease;
            GitHubAsset = githubAsset;
        }
    }
}

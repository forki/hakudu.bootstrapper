using System;
using Hakudu.Bootstrapper.GitHub;
using SemVersion;

namespace Hakudu.Bootstrapper.Repositories
{
    public class PackageInfo
    {
        public SemanticVersion Version { get; }
        public GitHubReleaseAsset GitHubAsset { get; }

        public PackageInfo(SemanticVersion version, GitHubReleaseAsset githubAsset)
        {
            if (version == null)
                throw new ArgumentNullException(nameof(version));

            if (githubAsset == null)
                throw new ArgumentNullException(nameof(githubAsset));

            Version = version;
            GitHubAsset = githubAsset;
        }
    }
}

using System;
using System.Collections.Generic;
using Hakudu.Bootstrapper.GitHub;
using SemVersion;

namespace Hakudu.Bootstrapper.Repositories
{
    public class GitHubReleaseMatcher
    {
        const string ASSET_STATE_UPLOADED = "uploaded";

        readonly string _assetName;

        public bool AllowPreRelease { get; set; }

        public GitHubReleaseMatcher(string assetName)
        {
            if (assetName == null)
                throw new ArgumentNullException(nameof(assetName));

            _assetName = assetName;
        }

        public PackageInfo FindMatching(IEnumerable<GitHubRelease> releases)
        {
            if (releases == null)
                throw new ArgumentNullException(nameof(releases));

            foreach (var release in releases)
            {
                // Skipping the release if pre-releases are not allowed
                if (!AllowPreRelease && release.PreRelease)
                    continue;

                var version = GetReleaseVersion(release);

                // Skipping the release if version cannot be parsed
                if (version == null)
                    continue;

                var asset = release.GetAssetByName(_assetName);

                // Skipping the release if asset not found or not in ready state
                if (asset == null || asset.State != ASSET_STATE_UPLOADED)
                    continue;

                return new PackageInfo(version, asset);
            }

            return null;
        }

        static SemanticVersion GetReleaseVersion(GitHubRelease release)
        {
            SemanticVersion version;

            if (!SemanticVersion.TryParse(release.Name.TrimStart('v'), out version))
            {
                SemanticVersion.TryParse(release.TagName.TrimStart('v'), out version);
            }

            return version;
        }
    }
}

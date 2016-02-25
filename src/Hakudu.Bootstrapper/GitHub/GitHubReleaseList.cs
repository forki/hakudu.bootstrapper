using System;
using System.Collections.Generic;

namespace Hakudu.Bootstrapper.GitHub
{
    public class GitHubReleaseList
    {
        public IList<GitHubRelease> Releases { get; }

        public Uri NextUri { get; }

        public bool HasNext => NextUri != null;

        public GitHubReleaseList(IList<GitHubRelease> releases, Uri nextUri)
        {
            if (releases == null)
                throw new ArgumentNullException(nameof(releases));

            Releases = releases;
            NextUri = nextUri;
        }
    }
}

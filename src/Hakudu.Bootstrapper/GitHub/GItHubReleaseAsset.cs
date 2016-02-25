using System;
using System.Runtime.Serialization;

namespace Hakudu.Bootstrapper.GitHub
{
    [DataContract]
    public class GitHubReleaseAsset
    {
        [DataMember(Name = "id")]
        public string Id { get; private set; }

        [DataMember(Name = "name")]
        public string Name { get; private set; }

        [DataMember(Name = "state")]
        public string State { get; private set; }

        [DataMember(Name = "browser_download_url")]
        public Uri DownloadUrl { get; private set; }
    }
}

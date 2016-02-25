using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Hakudu.Bootstrapper.GitHub
{
    [DataContract]
    public class GitHubRelease
    {
        [DataMember(Name = "id")]
        public int Id { get; private set; }

        [DataMember(Name = "name")]
        public string Name { get; private set; }

        [DataMember(Name = "tag_name")]
        public string TagName { get; private set; }

        [DataMember(Name = "body_text")]
        public string Description { get; private set; }

        [DataMember(Name = "prerelease")]
        public bool PreRelease { get; private set; }

        [DataMember(Name = "assets")]
        public IList<GitHubReleaseAsset> Assets { get; private set; }
    }
}

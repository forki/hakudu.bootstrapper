using SemVersion;

namespace Hakudu.Bootstrapper.LocalStorage
{
    public class InstallationBrokenException : BootstrapperException
    {
        public SemanticVersion Version { get; }

        public InstallationBrokenException(SemanticVersion version, string message) : base(message)
        {
            Version = version;
        }
    }
}

using System;
using System.IO;
using SemVersion;

namespace Hakudu.Bootstrapper.LocalStorage
{
    public class EngineInfo
    {
        public SemanticVersion Version { get; }
        public DirectoryInfo InstallDir { get; }
        public string ExePath { get; }

        public EngineInfo(SemanticVersion version, DirectoryInfo installDir, string exePath)
        {
            if (version == null)
                throw new ArgumentNullException(nameof(version));

            if (installDir == null)
                throw new ArgumentNullException(nameof(installDir));

            if (exePath == null)
                throw new ArgumentNullException(nameof(exePath));

            InstallDir = installDir;
            Version = version;
            ExePath = exePath;
        }
    }
}

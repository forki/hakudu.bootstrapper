using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using SemVersion;

namespace Hakudu.Bootstrapper.LocalStorage
{
    public class EngineStorageManager
    {
        const string ENGINE_DIR = "Engine";
        const string ENGINE_EXE_NAME = "hakudueng.exe";

        readonly DirectoryInfo _engineInstallRoot;

        public EngineStorageManager(KuduEnvironment environment)
        {
            if (environment == null)
                throw new ArgumentNullException(nameof(environment));

            _engineInstallRoot = GetOrCreateDirectory(Path.Combine(environment.HakuduHome, ENGINE_DIR));
        }

        public EngineInfo GetInstalled()
        {
            return GetInstalledVersions().OrderByDescending(x => x.Version).FirstOrDefault();
        }

        public EngineInfo InstallFromZip(SemanticVersion version, string zipFileName)
        {
            var installDir = _engineInstallRoot.CreateSubdirectory(version.ToString());

            try
            {
                ZipFile.ExtractToDirectory(zipFileName, installDir.FullName);

                var exePath = Path.Combine(installDir.FullName, ENGINE_EXE_NAME);

                // Throw error if the main executable not found
                if (!File.Exists(exePath))
                {
                    var errorMessage = $"The downloaded package of Hakudu Engine {version} is broken.";
                    throw new InstallationBrokenException(version, errorMessage);
                }

                return new EngineInfo(version, installDir, exePath);
            }
            catch (Exception)
            {
                // Cleanup in case of installation failure
                installDir.Delete(true);
                throw;
            }
        }

        public void DeleteInstalled()
        {
            foreach (var installDir in _engineInstallRoot.EnumerateDirectories())
            {
                installDir.Delete(true);
            }
        }

        IEnumerable<EngineInfo> GetInstalledVersions()
        {
            foreach (var installDir in _engineInstallRoot.EnumerateDirectories())
            {
                SemanticVersion version;

                // Skip if version cannot be retrieved from the directory name
                if (!SemanticVersion.TryParse(installDir.Name, out version))
                    continue;

                var exePath = Path.Combine(installDir.FullName, ENGINE_EXE_NAME);

                // Skip if the main executable not found
                if (!File.Exists(exePath))
                    continue;

                yield return new EngineInfo(version, installDir, exePath);
            }
        }

        static DirectoryInfo GetOrCreateDirectory(string path)
        {
            var directory = new DirectoryInfo(path);

            if (!directory.Exists)
                directory.Create();

            return directory;
        }
    }
}

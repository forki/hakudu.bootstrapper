using System;
using System.Threading.Tasks;
using Hakudu.Bootstrapper.Engine;
using Hakudu.Bootstrapper.LocalStorage;
using Hakudu.Bootstrapper.Repositories;

namespace Hakudu.Bootstrapper
{
    public class ControllerForWebApp : IController
    {
        readonly BootstrapperContext _bootstrapper;

        readonly GitHubPackageManager _packageManager;
        readonly EngineStorageManager _storageManager;
        readonly EngineHost _engineHost;

        public ControllerForWebApp(BootstrapperContext bootstrapper)
        {
            if (bootstrapper == null)
                throw new ArgumentNullException(nameof(bootstrapper));

            _bootstrapper = bootstrapper;

            _packageManager = new GitHubPackageManager(_bootstrapper.UserAgent);
            _storageManager = new EngineStorageManager(_bootstrapper.Environment);
            _engineHost = new EngineHost(_bootstrapper);
        }

        public async Task<int> Run()
        {
            Console.WriteLine($"{_bootstrapper.Name} {_bootstrapper.Version} started...");

            // Check the instsalled engine and install a new version if necessary
            var installed = await InstallEngine();

            // Run the engine
            return await _engineHost.StartAsync(installed);
        }

        async Task<EngineInfo> InstallEngine()
        {
            // Checking for the locally installed version
            var installed = _storageManager.GetInstalled();

            if (installed == null)
            {
                Console.WriteLine("Hakudu engine not installed.");
            }
            else
                Console.WriteLine($"Hakudu {installed.Version} installed.");

            Console.WriteLine("Checking for the latest version...");

            // Checking for the latest version online
            var latest = await _packageManager.GetLatest(installed?.Version, preRelease: true);

            if (latest != null)
            {
                // A newer version of engine was found, downloading and installing...
                var version = latest.Version;

                if (latest.PreRelease)
                {
                    Console.WriteLine($"Downloading pre-release version {version}...");
                }
                else
                    Console.WriteLine($"Downloading version {version}...");

                // Download package
                var fileName = await _packageManager.Download(latest);

                Console.WriteLine($"Installing version {version}...");

                // Install package
                _storageManager.DeleteInstalled();
                installed = _storageManager.InstallFromZip(version, fileName);

                Console.WriteLine("Installation complete.");
            }
            else
            {
                // Fail if no version is available online and it's not installed locally
                // This may be caused by misconfiguration or other problems with online repository
                if (installed == null)
                    throw new BootstrapperException("No version of engine is available.");

                Console.WriteLine("The current version is up-to-date.");
            }

            return installed;
        }
    }
}

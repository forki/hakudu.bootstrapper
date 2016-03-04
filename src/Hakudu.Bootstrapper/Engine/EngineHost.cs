using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading.Tasks;
using Hakudu.Bootstrapper.LocalStorage;

namespace Hakudu.Bootstrapper.Engine
{
    public class EngineHost
    {
        const string ENV_BOOTSTRAPPER_EXE = "HAKUDU_BOOTSTRAPPER_EXE";
        const string ENV_BOOTSTRAPPER_VERSION = "HAKUDU_BOOTSTRAPPER_VERSION";

        readonly BootstrapperContext _bootstrapper;

        public EngineHost(BootstrapperContext bootstrapper)
        {
            if (bootstrapper == null)
                throw new ArgumentNullException(nameof(bootstrapper));

            _bootstrapper = bootstrapper;
        }

        public async Task<int> StartAsync(EngineInfo engine)
        {
            if (engine == null)
                throw new ArgumentNullException(nameof(engine));

            var startInfo = new ProcessStartInfo(engine.ExePath)
            {
                WorkingDirectory = engine.InstallDir.FullName,
                UseShellExecute = false
            };

            SetEnvironmentVariables(startInfo.EnvironmentVariables);

            return await StartProcessAsync(startInfo);
        }

        void SetEnvironmentVariables(StringDictionary env)
        {
            env[ENV_BOOTSTRAPPER_EXE] = _bootstrapper.ExePath;
            env[ENV_BOOTSTRAPPER_VERSION] = _bootstrapper.Version.ToString();
        }

        static Task<int> StartProcessAsync(ProcessStartInfo startInfo)
        {
            var tcs = new TaskCompletionSource<int>();

            var process = new Process
            {
                StartInfo = startInfo,
                EnableRaisingEvents = true
            };

            process.Exited += (sender, e) =>
            {
                tcs.SetResult(process.ExitCode);
                process.Dispose();
            };

            try
            {
                if (!process.Start())
                {
                    var errorMessage = $"Failed to start the Hakudu engine process ({startInfo.FileName}).";
                    tcs.SetException(new EngineExecutionException(errorMessage));
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to start the Hakudu engine process ({startInfo.FileName}). {ex.Message}";
                tcs.SetException(new EngineExecutionException(errorMessage, ex));
            }

            return tcs.Task;
        }
    }
}

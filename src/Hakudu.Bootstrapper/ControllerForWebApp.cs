using System;
using System.Threading.Tasks;

namespace Hakudu.Bootstrapper
{
    public class ControllerForWebApp : IController
    {
        readonly BootstrapperContext _bootstrapper;

        public ControllerForWebApp(BootstrapperContext bootstrapper)
        {
            if (bootstrapper == null)
                throw new ArgumentNullException(nameof(bootstrapper));

            _bootstrapper = bootstrapper;
        }

        public Task<int> Run()
        {
            Console.WriteLine($"{_bootstrapper.Name} {_bootstrapper.Version} started...");
            Console.WriteLine("Done.");

            return Task.FromResult((int) ExitCode.Success);
        }
    }
}

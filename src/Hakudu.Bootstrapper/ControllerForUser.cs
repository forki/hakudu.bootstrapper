using System;
using System.Threading.Tasks;

namespace Hakudu.Bootstrapper
{
    public class ControllerForUser : IController
    {
        readonly BootstrapperContext _bootstrapper;

        public ControllerForUser(BootstrapperContext bootstrapper)
        {
            if (bootstrapper == null)
                throw new ArgumentNullException(nameof(bootstrapper));

            _bootstrapper = bootstrapper;
        }

        public Task<int> Run()
        {
            Console.WriteLine($"{_bootstrapper.Name} {_bootstrapper.Version}");
            Console.WriteLine(_bootstrapper.Website);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Add me to your Azure Web App repo!");
            Console.ResetColor();

            return Task.FromResult((int) ExitCode.Success);
        }
    }
}

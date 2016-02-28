using System;

namespace Hakudu.Bootstrapper
{
    static class Program
    {
        static int Main(string[] args)
        {
            try
            {
                // Initializing the bootstrapper context and retrieving information
                // about the execution environment
                var context = new BootstrapperContext();

                IController controller;

                // The app behaves differently depending on the environment it was executed
                if (context.IsKuduEnvironment)
                {
                    // Azure Web App instance
                    controller = new ControllerForWebApp(context);
                }
                else
                {
                    // User's machine
                    controller = new ControllerForUser(context);
                }

                return controller.Run().Result;
            }
            catch (BootstrapperException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine("Error: " + ex.Message);
                Console.ResetColor();
                return (int) ExitCode.GeneralFailure;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(ex);
                Console.ResetColor();
                return (int) ExitCode.GeneralFailure;
            }
        }
    }
}

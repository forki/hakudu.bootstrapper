using System;

namespace Hakudu.Bootstrapper.Engine
{
    public class EngineExecutionException : BootstrapperException
    {
        public EngineExecutionException()
        {
        }

        public EngineExecutionException(string message) : base(message)
        {
        }

        public EngineExecutionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

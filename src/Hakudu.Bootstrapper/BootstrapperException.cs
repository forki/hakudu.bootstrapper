using System;

namespace Hakudu.Bootstrapper
{
    public class BootstrapperException : Exception
    {
        public BootstrapperException()
        {
        }

        public BootstrapperException(string message) : base(message)
        {
        }

        public BootstrapperException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

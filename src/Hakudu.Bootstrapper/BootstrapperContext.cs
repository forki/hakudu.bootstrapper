using System;
using System.Reflection;
using SemVersion;

namespace Hakudu.Bootstrapper
{
    public class BootstrapperContext
    {
        public string Name { get; }
        public SemanticVersion Version { get; }
        public string Copyright { get; }
        public string Website { get; }

        public string UserAgent { get; }

        public KuduEnvironment Environment { get; }
        public bool IsKuduEnvironment => Environment != null;

        public BootstrapperContext()
        {
            SemanticVersion version;
            if (!SemanticVersion.TryParse(GetAssemblyVersion(), out version))
                throw new Exception("Unable to determine the bootstrapper's version.");

            Name = GetAssemblyTitle();
            Version = version;
            Copyright = GetAssemblyCopyright();
            Website = "http://hakudu.io";

            UserAgent = $"Hakudu.Bootstrapper/{Version}";

            Environment = KuduEnvironment.GetIfExecutedInKudu();
        }

        static string GetAssemblyVersion()
        {
            var assembly = Assembly.GetEntryAssembly();
            var versionAttribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            return versionAttribute?.InformationalVersion;
        }

        static string GetAssemblyTitle()
        {
            var assembly = Assembly.GetEntryAssembly();
            var titleAttribute = assembly.GetCustomAttribute<AssemblyTitleAttribute>();
            return titleAttribute?.Title;
        }

        static string GetAssemblyCopyright()
        {
            var assembly = Assembly.GetEntryAssembly();
            var copyrightAttribute = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
            return copyrightAttribute?.Copyright;
        }
    }
}

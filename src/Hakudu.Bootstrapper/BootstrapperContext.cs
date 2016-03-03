using System;
using System.Reflection;
using SemVersion;

namespace Hakudu.Bootstrapper
{
    public class BootstrapperContext
    {
        static readonly Assembly _assembly = Assembly.GetExecutingAssembly();

        public string Name { get; }
        public SemanticVersion Version { get; }
        public string Copyright { get; }
        public string Website { get; } = "http://hakudu.io";

        public string UserAgent { get; }
        public string ExePath { get; }

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

            UserAgent = $"Hakudu.Bootstrapper/{Version}";
            ExePath = _assembly.Location;

            Environment = KuduEnvironment.GetIfExecutedInKudu();
        }

        static string GetAssemblyVersion()
        {
            var versionAttribute = _assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            return versionAttribute?.InformationalVersion;
        }

        static string GetAssemblyTitle()
        {
            var titleAttribute = _assembly.GetCustomAttribute<AssemblyTitleAttribute>();
            return titleAttribute?.Title;
        }

        static string GetAssemblyCopyright()
        {
            var copyrightAttribute = _assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
            return copyrightAttribute?.Copyright;
        }
    }
}

using System;
using System.IO;

namespace Hakudu.Bootstrapper
{
    public sealed class KuduEnvironment
    {
        const string ENV_KUDU_EXE = "KUDU_EXE";
        const string ENV_KUDU_HOME = "HOME";
        const string HAKUDU_DIR = "Hakudu";

        public string Home { get; }

        public string HakuduHome { get; }

        public static KuduEnvironment GetIfHostedByKudu()
        {
#if !DEBUG
            if (Environment.GetEnvironmentVariable(ENV_KUDU_EXE) == null)
                return null;
#endif
            return new KuduEnvironment();
        }

        KuduEnvironment()
        {
#if DEBUG
            Home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
#else
            Home = Environment.GetEnvironmentVariable(ENV_KUDU_HOME);
#endif
            HakuduHome = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), HAKUDU_DIR);
        }
    }
}

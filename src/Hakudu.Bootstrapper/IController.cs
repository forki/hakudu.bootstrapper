using System.Threading.Tasks;

namespace Hakudu.Bootstrapper
{
    public interface IController
    {
        Task<int> Run();
    }
}

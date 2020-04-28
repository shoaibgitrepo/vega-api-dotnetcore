using System.Threading.Tasks;

namespace vega_api_dotnetcore.Core
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
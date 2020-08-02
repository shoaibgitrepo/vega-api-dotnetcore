using System;
using System.Threading.Tasks;
using vega_api_dotnetcore.Core.Repositories;

namespace vega_api_dotnetcore.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IVehicleRepository Vehicles { get; }
        IPhotoRepository Photos { get; }
        IFeatureRepository Features { get; }
        IMakeRepository Makes { get; }

        Task CompleteAsync();
    }
}
using System.Threading.Tasks;
using vega_api_dotnetcore.Core;
using vega_api_dotnetcore.Core.Repositories;
using vega_api_dotnetcore.Persistence.Repositories;

namespace vega_api_dotnetcore.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VegaDbContext context;
        public UnitOfWork(VegaDbContext context)
        {
            this.context = context;
            Vehicles = new VehicleRepository(context);
            Photos = new PhotoRepository(context);
            Features = new FeatureRepository(context);
            Makes = new MakeRepository(context);
        }

        public IVehicleRepository Vehicles { get; private set; }
        public IPhotoRepository Photos { get; private set; }
        public IFeatureRepository Features { get; private set; }
        public IMakeRepository Makes { get; private set; }

        public async Task CompleteAsync()
        {
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.DisposeAsync();
        }
    }
}
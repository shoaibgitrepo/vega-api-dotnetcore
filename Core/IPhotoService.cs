using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using vega_api_dotnetcore.Core.Models;

namespace vega_api_dotnetcore.Core
{
    public interface IPhotoService
    {
        Task<Photo> UploadPhoto(Vehicle vehicle, IFormFile file, string uploadsFolderPath);
    }
}
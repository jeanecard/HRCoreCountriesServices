using HRGeoLocator.Models;
using System.Threading.Tasks;

namespace HRGeoLocator.Services.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHRGeonameService
    {
        /// <summary>
        /// Proxy on searchJSON
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        Task<GeoNameRootObject> GetGeonamesAsync(string pattern);
    }
}

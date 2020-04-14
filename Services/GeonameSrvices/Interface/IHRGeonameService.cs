using HRGeonameModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeonameServices.Interface
{
    /// <summary>
    /// Proxy on Geoname service
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

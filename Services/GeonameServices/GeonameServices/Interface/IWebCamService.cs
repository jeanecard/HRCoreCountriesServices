using HRGeoLocator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRGeoLocator.Services.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWebCamService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wgs84_lat"></param>
        /// <param name="wgs84_lon"></param>
        /// <param name="radiusInKilometers"></param>
        /// <returns></returns>
        Task<WebCamsTravelRootObject> GetWebCamsNearAsync(float wgs84_lat, float wgs84_lon, float radiusInKilometers);
    }
}

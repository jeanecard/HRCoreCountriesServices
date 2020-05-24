using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRGeoLocator.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GeoNameRootObject
    {
        /// <summary>
        /// 
        /// </summary>
        public int totalResultsCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Geoname> geonames { get; set; }
    }
}

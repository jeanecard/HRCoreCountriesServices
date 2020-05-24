using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRGeoLocator.Models
{
    /// <summary>
    /// Item retrieving by Geonmae service
    /// </summary>
    public class Geoname
    {
        /// <summary>
        /// Local code admin.
        /// </summary>
        public string adminCode1 { get; set; }
        /// <summary>
        /// Longitude
        /// </summary>
        public string lng { get; set; }
        /// <summary>
        /// ID in geoname service
        /// </summary>
        public int geonameId { get; set; }
        /// <summary>
        /// topony name in geoname service
        /// </summary>
        public string toponymName { get; set; }
        /// <summary>
        /// ID of country (ISO)
        /// </summary>
        public string countryId { get; set; }
        /// <summary>
        /// No idea :-(
        /// </summary>
        public string fcl { get; set; }
        /// <summary>
        /// Amount of population (estimated in date unknown)
        /// </summary>
        public int population { get; set; }
        /// <summary>
        /// The country code
        /// </summary>
        public string countryCode { get; set; }
        /// <summary>
        /// Name of country
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fclName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string countryName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fcodeName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string adminName1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fcode { get; set; }
    }
}

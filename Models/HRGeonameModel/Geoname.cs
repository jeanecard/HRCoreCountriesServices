using System;
using System.Collections.Generic;

namespace HRGeonameModel
{
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
    public class Geoname
    {
        /// <summary>
        /// 
        /// </summary>
        public string adminCode1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lng { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int geonameId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string toponymName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string countryId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fcl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int population { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string countryCode { get; set; }
        /// <summary>
        /// 
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

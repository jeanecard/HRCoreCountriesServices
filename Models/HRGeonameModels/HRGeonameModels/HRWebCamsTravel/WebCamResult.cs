using System.Collections.Generic;

namespace HRGeoLocator.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 
        /// </summary>
        public int offset { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int limit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Webcam> webcams { get; set; }
    }
}

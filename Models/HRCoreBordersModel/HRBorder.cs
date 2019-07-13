using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using System;

namespace HRCoreBordersModel
{
    /// <summary>
    /// A Model of HRBorder
    /// </summary>
    public class HRBorder
    {
        /// <summary>
        /// This property is used if and only if wkb_geometry is null. 
        /// </summary>
        private String wkt_geom = null;
        /// <summary>
        /// Geometry in binary Format  (OpenGIS specification)
        /// </summary>
        [JsonIgnore]
        public Geometry WKB_GEOMETRY { get; set; }
        /// <summary>
        /// Geometry in txt format (OpenGIS specification)
        /// </summary>
        public String WKT_GEOMETRY
        {
            get
            {
                if (WKB_GEOMETRY != null)
                {
                    return WKB_GEOMETRY.ToText();
                }
                else if (wkt_geom != null)
                {
                    return wkt_geom;
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                wkt_geom = value;
            }
        }
        /// <summary>
        /// FIPS country code (US government id of countries)
        /// </summary>
        public String FIPS { get; set; }
        /// <summary>
        /// ISO code Country (2 characters)
        /// </summary>
        public String ISO2 { get; set; }
        /// <summary>
        /// ISO code Country (3 characters)
        /// </summary>
        public String ISO3 { get; set; }
        /// <summary>
        /// UN (Unikted Nations) code Country
        /// </summary>
        public short UN { get; set; }
        /// <summary>
        /// Country name.
        /// </summary>
        public String NAME { get; set; }
        /// <summary>
        /// Area of country
        /// </summary>
        public double AREA { get; set; }
        /// <summary>
        /// Total of human population in 2005
        /// </summary>
        public int POP2005 { get; set; }
        /// <summary>
        /// Region
        /// </summary>
        public String REGION { get; set; }
        /// <summary>
        /// SubRegion
        /// </summary>
        public String SUBREGION { get; set; }
        /// <summary>
        /// Longitude of capital.
        /// </summary>
        public double LON { get; set; }
        /// <summary>
        /// Lattitude of capital.
        /// </summary>
        public double LAT { get; set; }
    }
}

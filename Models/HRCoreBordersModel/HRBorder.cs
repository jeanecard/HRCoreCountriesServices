using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using System;

namespace HRCoreBordersModel
{
    public class HRBorder
    {
        /// <summary>
        /// This property is used if and only if wkb_geometry is null. 
        /// </summary>
        private String wkt_geom = null;
        [JsonIgnore]
        public Geometry WKB_GEOMETRY { get; set; }
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
        public String FIPS { get; set; }
        public String ISO2 { get; set; }
        public String ISO3 { get; set; }
        public short UN { get; set; }
        public String NAME { get; set; }
        public double AREA { get; set; }
        public int POP2005 { get; set; }
        public String REGION { get; set; }
        public String SUBREGION { get; set; }
        public double LON { get; set; }
        public double LAT { get; set; }
    }
}

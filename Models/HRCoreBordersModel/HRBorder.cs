using System;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;

namespace HRCoreBordersModel
{
    public class HRBorder
    {
        [JsonIgnore]
        public Geometry BorderGeometry { get; set; }
        public String  WKT_GEOMETRY
        {

            get
            {
                if (BorderGeometry != null)
                {
                    return BorderGeometry.ToText();
                }
                else
                {
                    return String.Empty;
                }
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

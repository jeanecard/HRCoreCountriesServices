using System;
using System.Collections.Generic;

namespace HREFBirdRepository.Models
{
    public partial class Boundaries
    {
        public int QcId { get; set; }
        public string Fips { get; set; }
        public string Iso2 { get; set; }
        public string Iso3 { get; set; }
        public int? Un { get; set; }
        public string Name { get; set; }
        public int? Area { get; set; }
        public int? Pop2005 { get; set; }
        public int? Region { get; set; }
        public int? Subregion { get; set; }
        public double? Lon { get; set; }
        public double? Lat { get; set; }
    }
}

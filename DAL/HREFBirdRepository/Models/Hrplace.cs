using System;
using System.Collections.Generic;

namespace HREFBirdRepository.Models
{
    public partial class Hrplace
    {
        public string IdBird { get; set; }
        public string[] IdCountry { get; set; }
        public string RegionCountry { get; set; }
        public int? PeriodPresence { get; set; }
        public short? FrequenceObservation { get; set; }
    }
}

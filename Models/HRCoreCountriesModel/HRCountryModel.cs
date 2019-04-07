using System;
using GeoJSON.Net.Feature;

namespace HRCoreCountriesModel
{
    public class HRCountryModel
    {
        private Feature _feature = null;
        public Feature Feature { get => _feature; set => _feature = value; }
    }
}

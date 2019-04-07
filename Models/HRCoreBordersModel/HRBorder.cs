using System;
using GeoJSON.Net.Feature;

namespace HRCoreBordersModel
{
    public class HRBorder
    {
        private Feature _feature = null;

        public Feature Feature { get => _feature; set => _feature = value; }
    }
}

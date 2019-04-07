using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRCoreCountriesModel;

namespace HRCoreCountriesRepository
{
    public class CountriesRepository : ICountriesRepository
    {
        public async Task<IEnumerable<HRCountryModel>> GetCountriesAsync()
        {
            return null;
        }
    }
}
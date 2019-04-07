using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRCoreCountriesModel;

namespace HRCoreCountriesRepository
{
    public interface ICountriesRepository
    {
        Task<IEnumerable<HRCountryModel>> GetCountriesAsync();
    }
}
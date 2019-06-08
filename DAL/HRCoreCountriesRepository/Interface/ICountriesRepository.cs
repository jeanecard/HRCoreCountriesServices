using QuickType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCoreCountriesRepository
{
    public interface ICountriesRepository
    {
        Task<IEnumerable<HRCountry>> GetCountriesAsync(String id = null);
    }
}
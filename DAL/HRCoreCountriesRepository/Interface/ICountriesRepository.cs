using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuickType;

namespace HRCoreCountriesRepository
{
    public interface ICountriesRepository
    {
        Task<IEnumerable<HRCountry>> GetCountriesAsync(String id = null);
    }
}
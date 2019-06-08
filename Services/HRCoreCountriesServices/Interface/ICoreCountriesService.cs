using QuickType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCoreCountriesServices
{
    public interface ICoreCountriesService
    {
        Task<IEnumerable<HRCountry>> GetCountriesAsync(String id = null);
    }
}
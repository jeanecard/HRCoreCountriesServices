using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuickType;

namespace HRCoreCountriesServices
{
    public interface ICoreCountriesService
    {
        Task<IEnumerable<HRCountry>> GetCountriesAsync(String id = null);
    }
}
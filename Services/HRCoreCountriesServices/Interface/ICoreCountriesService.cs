using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRCoreCountriesModel;

namespace HRCoreCountriesServices
{
    public interface ICoreCountriesService
    {
        Task<IEnumerable<HRCountryModel>> GetCountriesAsync();
    }
} 
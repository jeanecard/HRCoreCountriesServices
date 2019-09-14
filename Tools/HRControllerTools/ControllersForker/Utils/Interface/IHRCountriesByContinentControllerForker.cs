using HRCoreCountriesServices;
using QuickType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRControllersForker.Interface
{
    /// <summary>
    /// TODO
    /// </summary>
    public interface IHRCountriesByContinentControllerForker
    {
        //TODO
        Task<(int, IEnumerable<HRCountry>)> GetHRCountriesByContinentAsync(ICoreCountriesService service, String continentId);
    }
}

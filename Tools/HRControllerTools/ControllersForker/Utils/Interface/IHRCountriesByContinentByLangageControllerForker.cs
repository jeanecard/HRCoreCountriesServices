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
    public interface IHRCountriesByContinentByLangageControllerForker
    {
        //TODO
        Task<(int, IEnumerable<HRCountry>)> GetHRCountriesByContinentByLanguageAsync(ICoreCountriesService service, String continentId, String languageID);

    }
}

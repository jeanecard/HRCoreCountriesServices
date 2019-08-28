using HRCoreCountriesServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRBordersAndCountriesWebAPI2.Utils.Interface
{
    /// <summary>
    /// IHRContinentControllerForker util interface
    /// </summary>
    public interface IHRContinentControllerForker
    {
        /// <summary>
        /// Get Continents with HTTP status.
        /// </summary>
        /// <returns></returns>
        (int, IEnumerable<String>) Get(ICoreCountriesService service);
        /// <summary>
        /// Get a Continent with HTTP status.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        (int, String) Get(String id, ICoreCountriesService service);
    }
}

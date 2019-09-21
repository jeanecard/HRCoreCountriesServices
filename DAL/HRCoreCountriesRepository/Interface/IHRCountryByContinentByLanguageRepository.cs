using QuickType;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRCoreCountriesRepository.Interface
{
    public interface IHRCountryByContinentByLanguageRepository
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="region"></param>
        /// <param name="Iso6391_Or_Iso6392_Language"></param>
        /// <returns></returns>
        Task<IEnumerable<HRCountry>> GetHRCountriesByContinentByLanguageAsync(Region region, String Iso6391_Or_Iso6392_Language);
    }
}

using QuickType;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRCoreCountriesRepository.Interface
{
    public interface IHRCountryByContinentRepository
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        Task<IEnumerable<HRCountry>> GetHRCountriesByContinentAsync(Region region);
    }
}

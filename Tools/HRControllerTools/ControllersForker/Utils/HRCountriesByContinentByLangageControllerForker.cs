using HRControllersForker.Interface;
using HRCoreCountriesServices;
using Microsoft.AspNetCore.Http;
using QuickType;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ControllersForkerTools
{
    /// <summary>
    /// TODO
    /// </summary>
    public class HRCountriesByContinentByLangageControllerForker : IHRCountriesByContinentByLangageControllerForker
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="service"></param>
        /// <param name="continentId"></param>
        /// <param name="languageID"></param>
        /// <returns></returns>
        public async Task<(int, IEnumerable<HRCountry>)> GetHRCountriesByContinentByLanguageAsync(
            ICoreCountriesService service, 
            string continentId, 
            string languageID)
        {
            Region region;
            if (service == null)
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
            if (!Enum.TryParse(continentId, out region) || String.IsNullOrEmpty(languageID))
            {
                return (StatusCodes.Status400BadRequest, null);
            }
            else
            {
                try
                {
                    using (Task<IEnumerable<HRCountry>> task = service.GetHRCountriesByContinentByLanguageAsync(region, languageID))
                    {
                        await task;
                        return (StatusCodes.Status200OK, task.Result);
                    }
                }
                catch (Exception)
                {
                    return (StatusCodes.Status500InternalServerError, null);
                }
            }
        }
    }
}

using HRControllersForker.Interface;
using HRCoreCountriesServices;
using Microsoft.AspNetCore.Http;
using QuickType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRControllersForker
{
    /// <summary>
    /// TODO
    /// </summary>
    public class HRCountriesByContinentControllerForker : IHRCountriesByContinentControllerForker
    {

        /// <summary>
        /// 1- Check input consitency.
        /// 2- Convert String to enum and call CoreCountriesService
        /// </summary>
        /// <param name="service">a Countryservice</param>
        /// <param name="continentId">a contientID (e.g : Africa)</param>
        /// <returns>Countries. Does not throw any exception. </returns>
        public async Task<(int, IEnumerable<HRCountry>)> GetHRCountriesByContinentAsync(ICoreCountriesService service, String continentId)
        {
            //1-
            Region region;
            if (service == null)
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
            //2-
            if (!Enum.TryParse(continentId, out region))
            {
                return (StatusCodes.Status400BadRequest, null);
            }
            else
            {
                try
                {
                    using (Task<IEnumerable<HRCountry>> task = service.GetHRCountriesByContinentAsync(region))
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


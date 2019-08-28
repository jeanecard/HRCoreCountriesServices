using HRBordersAndCountriesWebAPI2.Utils.Interface;
using HRCoreCountriesServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRBordersAndCountriesWebAPI2.Utils
{
    /// <summary>
    /// Controller's util with HTTP status avaialble for UT.
    /// </summary>
    public class HRContinentControllerForker : IHRContinentControllerForker
    {
        /// <summary>
        /// Get All Continents.
        /// 1- Check input consistance.
        /// 2- Call service.
        /// </summary>
        /// <returns></returns>
        public (int, IEnumerable<string>) Get(ICoreCountriesService service)
        {
            //1- 
            if(service == null)
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
            //2- 
            try
            {
                IEnumerable<string> continents = service.GetContinents();
                return (StatusCodes.Status200OK, continents);
            }
            catch
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
        }
        /// <summary>
        /// Get a specific Continent.
        /// 1- Check input consistance.
        /// 2- Call service.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public (int, string) Get(string id, ICoreCountriesService service)
        {
            //1- 
            if (service == null)
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
            //2- 
            try
            {
                string continent = service.GetContinentByID(id);
                if (!String.IsNullOrEmpty(continent))
                {
                    return (StatusCodes.Status200OK, continent);
                }
                else
                {
                    return (StatusCodes.Status404NotFound, String.Empty);
                }
            }
            catch
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
        }
    }
}

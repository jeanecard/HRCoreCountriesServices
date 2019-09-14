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
    /// TO DO
    /// </summary>
    public class HRLangagesByContinentControllerForker : IHRLangagesByContinentControllerForker
    {
        /// <summary>
        /// 1- Check input consitency.
        /// 2- Convert String to enum and call CoreCountriesService
        /// </summary>
        /// <param name="service"></param>
        /// <param name="continent"></param>
        /// <returns></returns>
        public async Task<(int, IEnumerable<Language>)> GetLangagesByContinentAsync(ICoreCountriesService service, string continent)
        {
            //1-
            Region region;
            if (service == null)
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
            //2-
            if (!Enum.TryParse(continent, out region))
            {
                return (StatusCodes.Status400BadRequest, null);
            }
            else
            {
                try
                {
                    using (Task<IEnumerable<Language>> task = service.GetHRLangagesByContinentAsync(region))
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

        /// <summary>
        /// 1- Check input consitency.
        /// 2- call CoreCountriesService
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public async Task<(int, IEnumerable<Language>)> GetAllLangagesAsync(ICoreCountriesService service)
        {
            //1-
            if (service == null)
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
            //2-
            try
            {
                using (Task<IEnumerable<Language>> task = service.GetAllLangagesAsync())
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

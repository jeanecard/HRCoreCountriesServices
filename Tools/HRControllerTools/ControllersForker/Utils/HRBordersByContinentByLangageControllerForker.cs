using ControllersForkerTools.Utils.Interface;
using HRCoreBordersModel;
using HRCoreBordersServices;
using Microsoft.AspNetCore.Http;
using QuickType;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ControllersForkerTools.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class HRBordersByContinentByLangageControllerForker : IHRBordersByContinentByLangageControllerForker
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="service"></param>
        /// <param name="continentId"></param>
        /// <param name="langageId"></param>
        /// <returns></returns>
        public async Task<(int, IEnumerable<HRBorder>)> GetHRBordersByContinentByLanguageAsync(ICoreBordersService service, string continentId, string langageId)
        {
            Region region;
            if (service == null || !Enum.TryParse(continentId, out region))
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
            else
            {
                try
                {
                    using (Task<IEnumerable<HRBorder>> task = service.GetHRBorderByContinentByLanguageAsync(region, langageId))
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

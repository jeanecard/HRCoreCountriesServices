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
    public class HRBordersByContinentControllerForker : IHRBordersByContinentControllerForker
    {
        public async Task<(int, IEnumerable<HRBorder>)> GetHRBordersByContinentAsync(ICoreBordersService service, string id)
        {
            Region region;
            if (service == null)
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
            if (!Enum.TryParse(id, out region))
            {
                return (StatusCodes.Status400BadRequest, null);
            }
            else
            {
                try
                {
                    using (Task<IEnumerable<HRBorder>> task = service.GetHRBordersByContinentAsync(region))
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

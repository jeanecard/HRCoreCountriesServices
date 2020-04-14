using ControllersForkerTools.Utils.Interface;
using HRCoreBordersModel;
using HRCoreBordersServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuickType;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ControllersForkerTools.Utils
{
    public class HRBordersByContinentControllerForker : IHRBordersByContinentControllerForker
    {
        private readonly IHRBordersByContinentByLangageControllerForker _util = null;
        private readonly ICoreBordersService _service = null;
        private readonly ILogger<HRBordersByContinentControllerForker> _logger = null;

        private HRBordersByContinentControllerForker()
        {
            //Dummy for DI
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="util"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public HRBordersByContinentControllerForker(IHRBordersByContinentByLangageControllerForker util,
            ICoreBordersService service,
            ILogger<HRBordersByContinentControllerForker> logger)
        {
            _util = util;
            _service = service;
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
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
                catch 
                {
                    return (StatusCodes.Status500InternalServerError, null);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControllersForkerTools.Utils.Interface;
using HRCoreBordersModel;
using HRCoreBordersServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HRBordersAndCountriesWebAPI2.Controllers
{
    /// <summary>
    /// Class to manage HRBorders via Region.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HRBordersByContinentController : ControllerBase
    {
        private readonly IHRBordersByContinentControllerForker _util = null;
        private readonly ICoreBordersService _service = null;
        private readonly ILogger<HRBordersByContinentController> _logger = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">a Continent id : All, Africa, Americas, Asia, Empty, Europe, Oceania, Polar</param>
        /// <returns>HRBorder with continent id</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<HRBorder>>> GetHRBordersByContinentIDAsync(String id)
        {
            if (_util == null || _service == null)
            {
                if (_logger != null)
                {
                    _logger.LogError("No border service or UtilForker available");
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            using (Task<(int, IEnumerable<HRBorder>)> task = _util.GetHRBordersByContinentAsync(_service, id))
            {
                await task;
                if (task.Result.Item1 == StatusCodes.Status200OK)
                {
                    return task.Result.Item2.ToList();
                }
                else
                {
                    return StatusCode(task.Result.Item1);
                }
            }
        }
    }
}

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
    /// Controller to Get HRBorder by Continent and language.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HRBordersByContinentByLangageController : ControllerBase
    {
        private readonly IHRBordersByContinentByLangageControllerForker _util = null;
        private readonly ICoreBordersService _service = null;
        private readonly ILogger<HRBordersByContinentByLangageController> _logger = null;
        // GET: api/HRCountriesByContientByLangage/{continentID}/{LanguageID}
        /// <summary>
        /// Get All Borders of a specific Region for a specific Language
        /// </summary>
        /// <param name="continentId">a Continent id : All, Africa, Americas, Asia, Empty, Europe, Oceania, Polar</param>
        /// <param name="langageId">a iso language code (iso6391 Or Iso6392 eg : fr for France (no case sensitive))</param>
        /// <returns>Borders for these region ang language.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{continentId}/{langageId}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HRBorder>>> GetHRBordersByContinentByLangageIDAsync(String continentId, String langageId)
        {
            if (_util == null || _service == null)
            {
                if (_logger != null)
                {
                    _logger.LogError("No service or UtilForker available.");
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (String.IsNullOrEmpty(continentId) || String.IsNullOrEmpty(langageId))
            {
                if (_logger != null)
                {
                    _logger.LogError("No continent or language supplied.");
                }
                return StatusCode(StatusCodes.Status400BadRequest);

            }
            using (Task<(int, IEnumerable<HRBorder>)> task = _util.GetHRBordersByContinentByLanguageAsync(_service, continentId, langageId))
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

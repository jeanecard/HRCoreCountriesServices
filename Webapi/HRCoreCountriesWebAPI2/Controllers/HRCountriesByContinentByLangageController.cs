using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRControllersForker.Interface;
using HRCoreCountriesServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuickType;

namespace HRBordersAndCountriesWebAPI2.Controllers
{
    /// <summary>
    /// TODO
    /// </summary>
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class HRCountriesByContinentByLangageController : ControllerBase
    {
        private readonly IHRCountriesByContinentByLangageControllerForker _util = null;
        private readonly ICoreCountriesService _service = null;
        private readonly ILogger<HRCountriesByContinentByLangageController> _logger = null;
        private HRCountriesByContinentByLangageController()
        {
            //Dummy.
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="util"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public HRCountriesByContinentByLangageController(
            IHRCountriesByContinentByLangageControllerForker util,
            ICoreCountriesService service,
            ILogger<HRCountriesByContinentByLangageController> logger
            )
        {
            _util = util;
            _service = service;
            _logger = logger;
        }

        // GET: api/HRCountriesByContientByLangage/{continentID}/{LanguageID}
        /// <summary>
        /// Get All countries of a specific Region for a specific Language
        /// </summary>
        /// <param name="continentId">a Continent id : All, Africa, Americas, Asia, Empty, Europe, Oceania, Polar</param>
        /// <param name="langageId">a iso language code (iso6391 Or Iso6392 eg : fr for France (no case sensitive))</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{continentId}/{langageId}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HRCountry>>> GetHRCountriesByContinentByLangageIDAsync(String continentId, String langageId)
        {
            if (_util == null || _service == null)
            {
                if (_logger != null)
                {
                    _logger.LogError("No service or UtilForker available.");
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if(String.IsNullOrEmpty(continentId) || String.IsNullOrEmpty(langageId))
            {
                if (_logger != null)
                {
                    _logger.LogError("No continent or language supplied.");
                }
                return StatusCode(StatusCodes.Status400BadRequest);

            }
            using (Task<(int, IEnumerable<HRCountry>)> task = _util.GetHRCountriesByContinentByLanguageAsync(_service, continentId, langageId))
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

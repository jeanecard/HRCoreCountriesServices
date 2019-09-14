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
    /// Controller for Langages By Continent.
    /// </summary>
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class HRLangagesByContinentController : ControllerBase
    {
        private readonly ICoreCountriesService _service = null;
        private readonly IHRLangagesByContinentControllerForker _util = null;
        private readonly ILogger<HRContinentController> _logger = null;
        private HRLangagesByContinentController()
        {
            //Dummy for DI
        }
        /// <summary>
        /// HRLangagesByContinentController constructor available for DI.
        /// </summary>
        /// <param name="countriesService">a Country service.</param>
        /// <param name="logger">a Logger.</param>
        /// <param name="util"> util forker.</param>
        public HRLangagesByContinentController(
            ICoreCountriesService countriesService,
            ILogger<HRContinentController> logger,
            IHRLangagesByContinentControllerForker util)
        {
            _service = countriesService;
            _util = util;
            _logger = logger;
        }
        // GET: api/HRLangagesByContinent/Africa
        /// <summary>
        /// Get all Langages for a continent.
        /// </summary>
        /// <param name="id">a Continent id : Africa, Americas, Asia, Empty, Europe, Oceania, Polar</param>
        /// <returns>Langages if exist.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Language>>> GetHRLangagesByIDasync(String id)
        {
            if (_util == null || _service == null)
            {
                if (_logger != null)
                {
                    _logger.LogError("No continent service or UtilForker available");
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            using (Task<(int, IEnumerable<Language>)> task = _util.GetLangagesByContinentAsync(_service, id))
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
        // GET: api/HRLangagesByContinent
        /// <summary>
        /// Get all Langages distincts for all continents.
        /// </summary>
        /// <returns>Langage if exists.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Language>>> GetHRLangagesAsync()
        {
            if (_util == null || _service == null)
            {
                if (_logger != null)
                {
                    _logger.LogError("No continent service or UtilForker available");
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            using (Task<(int, IEnumerable<Language>)> task = _util.GetLangagesByContinentAsync(_service, Region.Empty.ToString()))
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

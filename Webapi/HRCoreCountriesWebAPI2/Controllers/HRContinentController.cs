using HRCoreBordersServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using QuickType;
using HRCoreCountriesServices;
using HRBordersAndCountriesWebAPI2.Utils.Interface;

namespace HRBordersAndCountriesWebAPI2.Controllers
{

    /// <summary>
    /// Controller for Continent.
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class HRContinentController : ControllerBase
    {
        private readonly ILogger<HRContinentController> _logger = null;
        private readonly ICoreCountriesService _continentService = null;
        private readonly IHRContinentControllerForker _util = null;

        /// <summary>
        /// Constructor for DI
        /// </summary>
        /// <param name="continentService">a continentservice (null not null)</param>
        /// <param name="logger">a logger (null allowed)</param>
        /// <param name="util">a IHRContinentControllerForker (null not allowed)</param>
        public HRContinentController(
            ICoreCountriesService continentService,
            ILogger<HRContinentController> logger,
            IHRContinentControllerForker util)
        {
            _continentService = continentService;
            _logger = logger;
            _util = util;
        }
        /// <summary>
        /// Private default constructor.
        /// </summary>
        private HRContinentController()
        {
            //Dummy.
        }

        /// <summary>
        /// Get All availables continent.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<String>> Get()
        {
            if (_util == null || _continentService == null)
            {
                if (_logger != null)
                {
                    _logger.LogError("No continent service or UtilForker available");
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            (int, IEnumerable<String>) result = _util.Get(_continentService);
            if(result.Item1 == StatusCodes.Status200OK)
            {
                return result.Item2.ToList();
            }
            else
            {
                return StatusCode(result.Item1);
            }
        }

        // GET: api/HRContinent/5
        /// <summary>
        /// Get a continent by its name (case sensitive)
        /// </summary>
        /// <param name="id">a continent name (case sensitive)</param>
        /// <returns>The name of continent if exists else 404 HTTP status</returns>
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<String> Get(String id)
        {

            if (_util == null || _continentService == null)
            {
                if (_logger != null)
                {
                    _logger.LogError("No continent service or UtilForker available");
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            (int, String) result = _util.Get(id, _continentService);
            if (result.Item1 == StatusCodes.Status200OK)
            {
                return result.Item2;
            }
            else
            {
                return StatusCode(result.Item1);
            }
        }
    }
}

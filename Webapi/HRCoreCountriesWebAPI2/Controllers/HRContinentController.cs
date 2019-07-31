using HRCoreBordersServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

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
        private readonly ICoreBordersService _continentService = null;

        /// <summary>
        /// Constructor for DI
        /// </summary>
        /// <param name="borderService">a borderservice (not null)</param>
        /// <param name="logger">a logger (null allowed)</param>
        public HRContinentController( ICoreBordersService borderService,
        ILogger<HRContinentController> logger)
        {
            _continentService = borderService;
            _logger = logger;
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
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            if(_continentService != null)
            {
                using (Task<IEnumerable<string>> serviceTask = _continentService.GetContinentsAsync())
                {
                    await serviceTask;
                    return  serviceTask.Result.ToList();
                }
            }
            else
            {
                if(_logger != null)
                {
                    _logger.LogError("No continent service available");
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
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
        public async Task<ActionResult<string>> Get(String id)
        {
            if (_continentService != null)
            {
                using (Task<string> serviceTask = _continentService.GetContinentByIDAsync(id))
                {
                    await serviceTask;
                    if(!String.IsNullOrEmpty(serviceTask.Result))
                    {
                        return serviceTask.Result;
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound);
                    }
                }
            }
            else
            {
                if (_logger != null)
                {
                    _logger.LogError("No continent service available");
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

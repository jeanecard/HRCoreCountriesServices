using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRCoreCountriesServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private HRLangagesByContinentController()
        {
            //Dummy for DI
        }
        /// <summary>
        /// HRLangagesByContinentController constructor available for DI.
        /// </summary>
        /// <param name="countriesService"></param>
        public HRLangagesByContinentController(ICoreCountriesService countriesService )
        {
            _service = countriesService;
        }
        // GET: api/HRLangagesByContinent/Africa
        /// <summary>
        /// Get all Langages for a continent. Use Continent = Empty to Get All Langages for all world.
        /// </summary>
        /// <param name="id">Continent ID (name not case sensitive)</param>
        /// <returns>Langage if exists.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IEnumerable<Language>> GetHRLangagesByIDasync(String id)
        {
            if (_service != null)
            {
                using (Task<IEnumerable<Language>> result = _service.GetHRLangagesByContinentAsync(id))
                {
                    await result;
                    return result.Result;
                }
            }
            return null;
        }
    }
}

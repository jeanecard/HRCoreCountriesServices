using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRCoreBordersModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRBordersAndCountriesWebAPI2.Controllers
{
    /// <summary>
    /// Controller to Get HRBorder by Continent and language.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HRBordersByContinentByLangageController : ControllerBase
    {
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
            await Task.Delay(1);
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}

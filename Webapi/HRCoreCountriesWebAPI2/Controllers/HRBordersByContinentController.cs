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
    /// Class to manage HRBorders via Region.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HRBordersByContinentController : ControllerBase
    {

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
            await Task.Delay(1);
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}

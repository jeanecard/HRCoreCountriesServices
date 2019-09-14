using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRBordersAndCountriesWebAPI2.Controllers
{
    /// <summary>
    /// TODO
    /// </summary>
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class HRCountriesByContinentByLangageController : ControllerBase
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        // GET: api/HRCountriesByContientByLangage
        [HttpGet]
        public IEnumerable<string> GetHRCountriesByContinentByLangage()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/HRCountriesByContientByLangage/5
        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public string GetHRCountriesByContinentByLangageID(int id)
        {
            return "value";
        }
    }
}

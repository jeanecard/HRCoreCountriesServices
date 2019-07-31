using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRBordersAndCountriesWebAPI2.Controllers
{
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class HRCountriesByContinentController : ControllerBase
    {
        // GET: api/HRCountriesByContient
        [HttpGet]
        public IEnumerable<string> GetHRCountriesByContinent()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/HRCountriesByContient/5
        [HttpGet("{id}")]
        public string GetHRCountriesByContinentID(int id)
        {
            return "value";
        }

        // POST: api/HRCountriesByContient
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/HRCountriesByContient/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

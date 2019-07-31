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
    public class HRLangagesByContinentController : ControllerBase
    {
        // GET: api/HRLangagesByContinent
        [HttpGet]
        public IEnumerable<string> GetHRLangagesByContinent()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/HRLangagesByContinent/5
        [HttpGet("{id}")]
        public string GetHRLangagesByContinentID(int id)
        {
            return "value";
        }

        // POST: api/HRLangagesByContinent
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/HRLangagesByContinent/5
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

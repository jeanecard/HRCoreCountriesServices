using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRCoreCountriesModel;
using HRCoreCountriesServices;
using Microsoft.AspNetCore.Mvc;

namespace HRCoreCountriesWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private ICoreCountriesService _countryService = null;
        public CountriesController(ICoreCountriesService cService)
        {
            _countryService = cService;
        }
        // GET api/countries
        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            if (_countryService != null)
            {
                Task<IEnumerable<HRCountryModel>> countriesTask = _countryService.GetCountriesAsync();
                IEnumerable<HRCountryModel> retour = await countriesTask;
                return JSON(retour);
            }
            else
            {
                return null;
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<HRCountryModel> Get(int id)
        {
            return null;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
            //Dummy
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            //Dummy
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //Dummy
        }
    }
}

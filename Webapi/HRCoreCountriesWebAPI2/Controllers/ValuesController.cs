using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuickType;
using HRCoreCountriesServices;
using HRCoreCountriesRepository;
using Microsoft.Extensions.Configuration;

namespace HRCoreCountriesWebAPI2.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class HRCountriesController : ControllerBase
    {
        private readonly ICoreCountriesService _service = null;
        private readonly IConfiguration _config;
        public HRCountriesController(ICoreCountriesService service, IConfiguration config)
        {
            _service = service;//new CoreCountriesService(new HardCodeCountriesRepository());
            _config = config;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<HRCountry>>> GetAllAsync()
        {
            Task<IEnumerable<HRCountry>> countriesAction = _service.GetCountriesAsync();
            await countriesAction;
            return countriesAction.Result.ToList();
        }

    }
}

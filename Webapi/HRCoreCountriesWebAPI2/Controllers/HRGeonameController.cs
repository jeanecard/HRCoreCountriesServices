using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRGeoLocator.Models;
using HRGeoLocator.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRGeoLocator.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class HRGeonameController : ControllerBase
    {
        private readonly IHRGeonameService _service = null;
        private HRGeonameController()
        {
            //Dummy for DI.
        }
        /// <summary>
        /// Constructor with GeoService
        /// </summary>
        /// <param name="service"></param>
        public HRGeonameController(IHRGeonameService service)
        {
            _service = service;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        // GET: api/HRGeoname/5
        [HttpGet("{pattern}")]
        public async Task<ActionResult<GeoNameRootObject>> GetGeonames(string pattern)
        {
            if (_service == null)
            {
                //TODO
            }
            using (var geonameTask = _service.GetGeonamesAsync(pattern))
            {
                await geonameTask;
                if (geonameTask.IsCompletedSuccessfully)
                {
                    return geonameTask.Result;
                }
            }
            return null;
        }

    }
}

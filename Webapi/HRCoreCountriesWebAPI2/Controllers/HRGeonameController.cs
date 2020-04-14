using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HRGeonameModel;
using GeonameSrvices;
using QuickType;
using GeonameServices.Interface;

namespace HRBordersAndCountriesWebAPI2.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
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
            if(_service == null)
            {

            }
            using (var geonameTask = _service.GetGeonamesAsync(pattern))
            {
                await geonameTask;
                if(geonameTask.IsCompletedSuccessfully)
                {
                    return geonameTask.Result;
                }
            }
            return null;
        }

    }
}

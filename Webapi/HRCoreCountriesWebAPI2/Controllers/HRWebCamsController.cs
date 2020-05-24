using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
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
    [Produces("application/json")]
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class HRWebCamsController : ControllerBase
    {
        private readonly IWebCamService _service = null;

        private HRWebCamsController()
        {
            //Dummy for DI.
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public HRWebCamsController(IWebCamService service)
        {
            _service = service;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wgs84_lat"></param>
        /// <param name="wgs84_lon"></param>
        /// <param name="radiusInKilometers"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<WebCamsTravelRootObject> Get([FromQuery] float wgs84_lat, [FromQuery] float wgs84_lon, [FromQuery] float radiusInKilometers)
        {
            if (_service == null)
            {
                //TODO
            }
            using (var task = _service.GetWebCamsNearAsync(wgs84_lat, wgs84_lon, radiusInKilometers))
            {
                await task;
                if (task.IsCompletedSuccessfully)
                {
                    return task.Result;
                }
            }
            return null;
        }
    }
}

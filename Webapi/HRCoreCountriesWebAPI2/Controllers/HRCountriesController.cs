using HRCommonModel;
using HRCommonTools;
using HRCommonTools.Interace;
using HRCoreCountriesServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QuickType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCoreCountriesWebAPI2.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class HRCountriesController : ControllerBase
    {
        private readonly IHRPaginer<HRCountry> _paginer = null;
        private readonly ICoreCountriesService _service = null;
        private readonly IConfiguration _config;
        private readonly ushort _maxPageSize = 100;
        public HRCountriesController(ICoreCountriesService service, IConfiguration config, IHRPaginer<HRCountry> paginer)
        {
            _service = service;
            _config = config;
            _paginer = paginer;
        }

        /// <summary>
        /// Get by ID Rest Method based on GetFromID(String id) method
        /// </summary>
        /// <param name="id">Country ID (MongoDB ID as hexadedcimal).</param>
        /// <returns>HRCountry corresponding</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<HRCountry>> Get([FromRoute] String id)
        {
            Task<(int, HRCountry)> result = GetFromID(id);
            await result;
            if (result.Result.Item2 != null)
            {
                return result.Result.Item2;
            }
            else
            {
                return StatusCode(result.Result.Item1);
            }
        }
        /// <summary>
        /// Get a Country by ID (_id)
        /// </summary>
        /// <param name="id">the country ID</param>
        /// <returns>the status code (http) and the Country.</returns>
        public async Task<(int, HRCountry)> GetFromID(string id)
        {
            //1-
            if (String.IsNullOrEmpty(id))
            {
                //Could not happen as Get(PageModel = null) exists)
                return (StatusCodes.Status400BadRequest, null);
            }
            if (_service == null)
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
            //2-
            try
            {
                Task<IEnumerable<HRCountry>> countriesAction = _service.GetCountriesAsync(id);
                await countriesAction;
                //3-
                if (countriesAction.Result != null)
                {
                    IEnumerator<HRCountry> enumerator = countriesAction.Result.GetEnumerator();
                    if (enumerator.MoveNext())
                    {
                        if (enumerator.Current != null
                            && enumerator.Current._id != null
                            && enumerator.Current._id.Equals(new MongoDB.Bson.ObjectId(id)))
                        {
                            return (StatusCodes.Status200OK, enumerator.Current);
                        }
                        else
                        {
                            return (StatusCodes.Status404NotFound, null);
                        }
                    }
                }
                return (StatusCodes.Status404NotFound, null);
            }
            catch (Exception)
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
        }


        /// <summary>
        /// Get by PagingInParameter based on GetFromPaging method
        /// </summary>
        /// <param name="pageModel">If pageModel is null return the first page else the querried one.</param>
        /// <returns>The expected PagingParameterOutModel or a null result with the http status code.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status416RequestedRangeNotSatisfiable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagingParameterOutModel<HRCountry>>> Get([FromQuery] PagingParameterInModel pageModel = null)
        {
            Task<(int, PagingParameterOutModel<HRCountry>)> result = GetFromPaging(pageModel);
            await result;
            if (result.Result.Item2 != null)
            {
                return result.Result.Item2;
            }
            else
            {
                return StatusCode(result.Result.Item1);
            }
        }
        /// <summary>
        /// 1- Process PagingInParameter if not supplied
        /// 2- Get the HRCountry from service
        /// 3- Paginate previous result
        /// </summary>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        public async Task<(int, PagingParameterOutModel<HRCountry>)> GetFromPaging([FromQuery] PagingParameterInModel pageModel = null)
        {
            if (_service != null && _paginer != null)
            {
                //1-
                if (pageModel == null)
                {
                    pageModel = GetDefaultPagingInParameter();
                }
                try
                {
                    //2-
                    Task<IEnumerable<HRCountry>> serviceAction = _service.GetCountriesAsync();
                    await serviceAction;
                    //3-
                    if (!_paginer.IsValid(pageModel, serviceAction.Result))
                    {
                        return (StatusCodes.Status416RequestedRangeNotSatisfiable, null);
                    }
                    else
                    {
                        return (StatusCodes.Status200OK, _paginer.GetPaginationFromFullList(pageModel, serviceAction.Result, _maxPageSize));
                    }
                }
                catch (Exception)
                {
                    return (StatusCodes.Status500InternalServerError, null);
                }
            }
            else
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
        }

        /// <summary>
        /// Create the default Paging without throwing any Exception.
        /// </summary>
        /// <returns></returns>
        private PagingParameterInModel GetDefaultPagingInParameter()
        {
            PagingParameterInModel retour = new PagingParameterInModel() { PageNumber = 0 };
            if (_config != null)
            {
                try
                {
                    retour.PageSize = _config.GetValue<ushort>("DefaultPagingInPageSize");
                }
                catch (Exception)
                {
                    retour.PageSize = 50;
                }
            }
            else
            {
                retour.PageSize = 50;
            }
            return retour;
        }
    }
}

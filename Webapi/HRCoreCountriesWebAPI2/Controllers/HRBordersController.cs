using HRCommonModel;
using HRCommonTools.Interace;
using HRCoreBordersModel;
using HRCoreBordersServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCoreCountriesWebAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HRBordersController : ControllerBase
    {
        private readonly ICoreBordersService _borderService = null;
        private readonly IHRPaginer<HRBorder> _paginer = null;
        private readonly IConfiguration _config = null;
        /// <summary>
        /// Public constructor with services DI
        /// </summary>
        /// <param name="paginer">a Paginer Implementation.</param>
        public HRBordersController(IHRPaginer<HRBorder> paginer, IConfiguration config, ICoreBordersService borderService)
        {
            _paginer = paginer;
            _config = config;
            _borderService = borderService;
        }
        /// <summary>
        /// Private default constructor.
        /// </summary>
        private HRBordersController()
        {
            //Dummy.
        }

        /// <summary>
        /// Get by ID Rest Method based on GetFromID(String id) method
        /// </summary>
        /// <param name="id">Border id</param>
        /// <returns>HRBorder corresponding</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<HRBorder>> Get([FromRoute] String id)
        {
            Task<(int, HRBorder)> result = GetFromID(id);
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
        /// 1- Check input consistance
        /// 2- Call service async
        /// 3- Process result of action as a single HRBorder.
        /// </summary>
        /// <param name="id">the FIPS value searched</param>
        /// <returns>StatusCode, HRBorder result</returns>
        public async Task<(int, HRBorder)> GetFromID(String id)
        {
            //1-
            if (String.IsNullOrEmpty(id))
            {
                //Could not happen as Get(PageModel = null) exists)
                return (StatusCodes.Status400BadRequest, null);
            }
            if (_borderService == null)
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
            //2-
            try
            {
                Task<IEnumerable<HRBorder>> bordersAction = _borderService.GetBordersAsync(id);
                await bordersAction;
                //3-
                if (bordersAction.Result != null)
                {
                    IEnumerator<HRBorder> enumerator = bordersAction.Result.GetEnumerator();
                    if (enumerator.MoveNext())
                    {
                        if (enumerator.Current != null
                            && !String.IsNullOrEmpty(enumerator.Current.FIPS)
                            && enumerator.Current.FIPS.ToUpper() == id.ToUpper())
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
        /// <param name="pageModel">The PagingInParameter. Can be null (will be set to server Default)</param>
        /// <returns>The HRBorders corresponding to pageModel parameter. Can throw MemberAccessException if any service is not consistant.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status416RequestedRangeNotSatisfiable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagingParameterOutModel<HRBorder>>> Get([FromQuery] PagingParameterInModel pageModel)
        {
            Task<(int, PagingParameterOutModel<HRBorder>)> result = GetFromPaging(pageModel);
            await result;
            if(result.Result.Item2 != null)
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
        /// 2- Get the HRBorders from service
        /// 3- Paginate previous result
        /// </summary>
        /// <param name="pageModel">The Paging Model</param>
        /// <returns>(http Status Code, PagingParameterOutModel)</returns>
        public async Task<(int, PagingParameterOutModel<HRBorder>)> GetFromPaging([FromQuery] PagingParameterInModel pageModel)
        {
            if (_borderService != null && _paginer != null)
            {
                //1-
                if (pageModel == null)
                {
                    pageModel = GetDefaultPagingInParameter();
                }
                try
                {
                    //2-
                    Task<IEnumerable<HRBorder>> bordersAction = _borderService.GetBordersAsync();
                    await bordersAction;
                    //3-
                    if (!_paginer.IsValid(pageModel, bordersAction.Result))
                    {
                        return (StatusCodes.Status416RequestedRangeNotSatisfiable, null);
                    }
                    else
                    {
                        PagingParameterOutModel<HRBorder> retour = _paginer.GetPagination(pageModel, bordersAction.Result);
                        return (StatusCodes.Status200OK, retour);
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
        /// No Post implemented.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        //[ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public IActionResult Post([FromBody] string value)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        /// <summary>
        /// No Put implemented.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        /// <summary>
        /// No Delete Implemented.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public IActionResult Delete(int id)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
        /// <summary>
        /// Set and return the Default PagingParameter for all the class. Does not throw any Exception.
        /// </summary>
        /// <returns>The default PagingInParamter</returns>
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
                    retour.PageSize = 20;
                }
            }
            else
            {
                retour.PageSize = 20;
            }
            return retour;
        }
    }
}
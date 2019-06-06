using HRCommonModel;
using HRCommonTools.Interace;
using HRCoreBordersModel;
using HRCoreBordersRepository;
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
        private ICoreBordersService _borderService = null;
        private IHRPaginer<HRBorder> _paginer = null;
        private IConfiguration _config = null;
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
        /// Get by ID Rest Method.
        /// 1- Check input consistance
        /// 2- Call service async
        /// 3- Process result of action as a single HRBorder.
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
            HRBorder retour = null;
            //1-
            if (String.IsNullOrEmpty(id))
            {
                //Could not happen as Get(PageModel = null) exists)
                return StatusCode(StatusCodes.Status400BadRequest);   
            }
            if (_borderService == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
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
                        retour = enumerator.Current;
                    }
                }
                if (retour != null)
                {
                    return retour;
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// 1- Process PagingInParameter if not supplied
        /// 2- Get the HRBorders from service
        /// 3- Paginate previous result
        /// </summary>
        /// <param name="pageModel">The PagingInParameter. Can be null (will be set to server Default)</param>
        /// <returns>The HRBorders corresponding to pageModel parameter. Can throw MemberAccessException if any service is not consistant.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status416RequestedRangeNotSatisfiable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagingParameterOutModel<HRBorder>>> Get([FromQuery] PagingParameterInModel pageModel)
        {
            //1-
            if (pageModel == null)
            {
                pageModel = GetDefaultPagingInParameter();
            }
            if (_borderService != null && pageModel != null && _paginer != null)
            {
                //2-
                try
                {
                    Task<IEnumerable<HRBorder>> bordersAction = _borderService.GetBordersAsync();
                    await bordersAction;
                    //3-
                    if (!_paginer.IsValid(pageModel, bordersAction.Result))
                    {
                        return StatusCode(StatusCodes.Status416RequestedRangeNotSatisfiable);
                    }
                    else
                    {
                        PagingParameterOutModel<HRBorder> retour = _paginer.GetPagination(pageModel, bordersAction.Result);
                        return retour;
                    }
                }
                catch(Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
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
        /// Set and return the Default PagingParameter for all the class
        /// </summary>
        /// <returns>The default PagingInParamter</returns>
        private PagingParameterInModel GetDefaultPagingInParameter()
        {

            PagingParameterInModel retour = new PagingParameterInModel();
            retour.PageNumber = 0;
            if (_config != null)
            {
                IConfigurationSection roger = _config.GetSection("DefaultPagingInPageSize");
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
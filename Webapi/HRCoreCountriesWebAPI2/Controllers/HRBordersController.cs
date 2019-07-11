using HRBordersAndCountriesWebAPI2.Utils;
using HRCommonModel;
using HRCommonModels;
using HRCommonTools;
using HRCoreBordersModel;
using HRCoreBordersServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace HRCoreCountriesWebAPI2.Controllers
{
    [Produces("application/json")]
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class HRBordersController : ControllerBase
    {
        private readonly ICoreBordersService _borderService = null;
        private readonly IConfiguration _config = null;
        private readonly static ushort _maxPageSize = 50;
        /// <summary>
        /// Public constructor with services DI
        /// </summary>
        /// <param name="paginer">a Paginer Implementation.</param>
        public HRBordersController(IConfiguration config, ICoreBordersService borderService)
        {
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
            Task<(int, HRBorder)> result = HRBordersControllersForker.GetFromID(id, _borderService);
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
        /// Get by PagingInParameter based on GetFromPaging method
        /// </summary>
        /// <param name="pageModel">The PagingInParameter. Can be null (will be set to server Default)</param>
        /// <param name="orderBy">The ordering param. Retrun a status 400 bad request is underlying services don't know how to order. Can be null.</param>
        /// <returns>The HRBorders corresponding to pageModel parameter. Can throw MemberAccessException if any service is not consistant.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status416RequestedRangeNotSatisfiable)]
        [ProducesResponseType(StatusCodes.Status413PayloadTooLarge)]
        public async Task<ActionResult<PagingParameterOutModel<HRBorder>>> Get(
            [FromQuery] PagingParameterInModel pageModel,
            [FromQuery]  HRSortingParamModel orderBy)
        {
            //1-
            if (pageModel == null)
            {
                pageModel = GetDefaultPagingInParameter();
            }

            Task<(int, PagingParameterOutModel<HRBorder>)> result = HRBordersControllersForker.GetFromPaging(
                pageModel, 
                orderBy,
                _borderService,
                _maxPageSize
                );
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
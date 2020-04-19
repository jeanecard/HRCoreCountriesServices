using HRCommonModel;
using HRCommonModels;
using HRControllersForker.Interface;
using HRCoreBordersModel;
using HRCoreBordersServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HRCoreCountriesWebAPI2.Controllers
{
    /// <summary>
    /// Controller  for HRBorder. Test
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class HRBordersController : ControllerBase
    {
        private readonly ILogger<HRBordersController> _logger = null;
        private readonly ICoreBordersService _borderService = null;
        private readonly IConfiguration _config = null;
        private readonly static ushort _maxPageSize = 50;
        private readonly IHRBordersControllersForker _util = null;
        /// <summary>
        /// Constructor for DI
        /// </summary>
        /// <param name="config">a MS Config</param>
        /// <param name="borderService">a IBorderService</param>
        /// <param name="util">a Commonutil</param>
        /// <param name="logger">a MS Logger</param>
        public HRBordersController(IConfiguration config, 
            ICoreBordersService borderService,
            IHRBordersControllersForker util,
            ILogger<HRBordersController> logger)
        {
            _config = config;
            _borderService = borderService;
            _util = util;
            _logger = logger;
            //TEST
        }
        /// <summary>
        /// Private default constructor.
        /// </summary>
        private HRBordersController()
        {
            //Dummy.
        }

        /// <summary>
        /// Method to get a specific HRBorder by its ISO2 code
        /// </summary>
        /// <param name="id">Border id (ISO2)</param>
        /// <returns>the corresponding HRBorder</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<HRBorder>> Get([FromRoute] String id)
        {
            if (_util != null)
            {
                using (Task<(int, HRBorder)> result = _util.GetFromIDAsync(id, _borderService))
                {
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
            }
            else
            {
                if(_logger != null)
                {
                    _logger.LogError("_util is null in HRBordersController");
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Method to get a list of ordered HRBorders. Server limit page size is 50 items. Beyond this value a http Status 416 : Requested Range Not Satisfiable is returned.
        /// </summary>
        /// <param name="pageModel">The PagingInParameter. Can be null (will be set to server Default)</param>
        /// <param name="orderBy">The ordering param. Retrun a status 400 bad request is underlying services don't know how to order. Can be null. Sample : "FIPS;ASC;ISO2;DESC"</param>
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
            if (_util != null)
            {
                if (pageModel == null)
                {
                    pageModel = GetDefaultPagingInParameter();
                }

                using (Task<(int, PagingParameterOutModel<HRBorder>)> result = _util.GetFromPagingAsync(
                    pageModel,
                    orderBy,
                    _borderService,
                    _maxPageSize
                    ))
                {
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
            }
            else
            {
                if (_logger != null)
                {
                    _logger.LogError("_util is null in HRBordersController");
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
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

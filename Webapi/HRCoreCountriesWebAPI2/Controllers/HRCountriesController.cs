using HRCommonModel;
using HRCommonModels;
using HRControllersForker.Interface;
using HRCoreCountriesServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QuickType;
using System;
using System.Threading.Tasks;

namespace HRCoreCountriesWebAPI2.Controllers
{
    /// <summary>
    /// HRCountries Controller
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1.0/[controller]")]
    [ApiController] 
    public class HRCountriesController : ControllerBase
    {
        private readonly ILogger<HRCountriesController> _logger = null;
        private readonly ICoreCountriesService _service = null;
        private readonly IConfiguration _config;
        private readonly ushort _maxPageSize = 100;
        private readonly IHRCountriesControllersForker _forker = null;
        /// <summary>
        /// Construcotr for DI.
        /// </summary>
        /// <param name="service">Country service.</param>
        /// <param name="config">MS Config.</param>
        /// <param name="forker">Country forker.</param>
        /// <param name="logger">MS logger.</param>
        public HRCountriesController(
            ICoreCountriesService service, 
            IConfiguration config,
            IHRCountriesControllersForker forker,
            ILogger<HRCountriesController> logger)
        {
            _service = service;
            _config = config;
            _forker = forker;
            _logger = logger;
        }
        private HRCountriesController()
        {
            //Dummy.
        }

        /// <summary>
        /// Get by ID Rest Method based on GetFromID(String id) method
        /// </summary>
        /// <param name="id">Get a Country by ID (ALPHA2_3CODE).</param>
        /// <returns>HRCountry corresponding</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<HRCountry>> Get([FromRoute] String id)
        {
            if (_forker != null)
            {
                using (Task<(int, HRCountry)> result = _forker.GetFromIDAsync(id, _service))
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
                    _logger.LogError("_forker instance is null in HRCountriesController.");
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Get by PagingInParameter based on GetFromPaging method
        /// </summary>
        /// <param name="pageModel">If pageModel is null return the first page else the querried one.</param>
        /// <param name="orderBy">Order by clause. Sample : "ISO2;ASC"</param>
        /// <returns>The expected PagingParameterOutModel or a null result with the http status code.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status416RequestedRangeNotSatisfiable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagingParameterOutModel<HRCountry>>> Get(
            [FromQuery] PagingParameterInModel pageModel,
            [FromQuery]  HRSortingParamModel orderBy)
        {
            if (_forker != null)
            {
                //1-
                if (pageModel == null)
                {
                    pageModel = GetDefaultPagingInParameter();
                }

                using (Task<(int, PagingParameterOutModel<HRCountry>)> result = _forker.GetFromPagingAsync(pageModel, orderBy, _service, _maxPageSize))
                {
                    await result;
                    if (result.Result.Item2 != null)
                    {
                        return result.Result.Item2;
                    }
                    else
                    {
                        if (_logger != null)
                        {
                            _logger.LogError("Ca marche po");
                        }

                        return StatusCode(result.Result.Item1);
                    }
                }
            }
            else
            {
                if (_logger != null)
                {
                    _logger.LogError("_forker instance is null in HRCountriesController.");
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
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

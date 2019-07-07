using HRBordersAndCountriesWebAPI2.Utils;
using HRCommonModel;
using HRCommonModels;
using HRCommonTools;
using HRCoreCountriesServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QuickType;
using System;
using System.Threading.Tasks;

namespace HRCoreCountriesWebAPI2.Controllers
{
    [Produces("application/json")]
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class HRCountriesController : ControllerBase
    {
        private readonly ICoreCountriesService _service = null;
        private readonly IConfiguration _config;
        private readonly ushort _maxPageSize = 100;
        public HRCountriesController(ICoreCountriesService service, IConfiguration config)
        {
            _service = service;
            _config = config;
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
            Task<(int, HRCountry)> result = HRCountriesControllersForker.GetFromID(id, _service);
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
        /// <param name="pageModel">If pageModel is null return the first page else the querried one.</param>
        /// <returns>The expected PagingParameterOutModel or a null result with the http status code.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status416RequestedRangeNotSatisfiable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagingParameterOutModel<HRCountry>>> Get(
            [FromQuery] PagingParameterInModel pageModel,
            [FromQuery]  HRSortingParamModel orderBy)
        {
            //1-
            if (pageModel == null)
            {
                pageModel = GetDefaultPagingInParameter();
            }

            Task<(int, PagingParameterOutModel<HRCountry>)> result = HRCountriesControllersForker.GetFromPaging(pageModel, orderBy, _service, _maxPageSize);
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

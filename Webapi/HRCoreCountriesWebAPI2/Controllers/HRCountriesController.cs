using HRCommonModel;
using HRCommonModels;
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
        /// Get a Country by ID (ALPHA2_3CODE)
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
                String idToCompare = id.ToUpper();
                Task<HRCountry> countryAction = _service.GetCountryAsync(id);
                await countryAction;
                //3-
                if (countryAction.Result != null)
                {

                    //Last check necessary ??
                    HRCountry candidateCountry = countryAction.Result;
                    if ((!String.IsNullOrEmpty(candidateCountry.Alpha2Code) && candidateCountry.Alpha2Code.ToUpper() == idToCompare)
                        || (!String.IsNullOrEmpty(candidateCountry.Alpha3Code) && (candidateCountry.Alpha3Code == idToCompare)))
                    {
                        return (StatusCodes.Status200OK, countryAction.Result);
                    }
                    else
                    {
                        return (StatusCodes.Status404NotFound, null);
                    }
                }
                return (StatusCodes.Status404NotFound, null);
            }
            catch (Exception ex)
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
        public async Task<ActionResult<PagingParameterOutModel<HRCountry>>> Get(
            [FromQuery] PagingParameterInModel pageModel,
            [FromQuery]  HRSortingParamModel orderBy)
        {
            Task<(int, PagingParameterOutModel<HRCountry>)> result = GetFromPaging(pageModel, orderBy);
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
        public async Task<(int, PagingParameterOutModel<HRCountry>)> GetFromPaging(
            [FromQuery] PagingParameterInModel pageModel,
            [FromQuery]HRSortingParamModel orderBy)
        {
            if (_service != null)
            {
                if (orderBy != null && orderBy.IsInitialised())
                {
                    if (!_service.IsSortable())
                    {
                        return (StatusCodes.Status400BadRequest, null);
                    }
                    else if (!HRSortingParamModelDeserializer.IsValid(orderBy))
                    {
                        return (StatusCodes.Status400BadRequest, null);
                    }
                }
                //1-
                if (pageModel == null)
                {
                    pageModel = GetDefaultPagingInParameter();
                }
                //!Add tu on this
                if (pageModel.PageSize > _maxPageSize)
                {
                    return (StatusCodes.Status413PayloadTooLarge, null);
                }
                try
                {
                    //2-
                    Task<PagingParameterOutModel<HRCountry>> countriesAction = _service.GetCountriesAsync(pageModel, orderBy);
                    await countriesAction;
                    //3-
                    return (StatusCodes.Status200OK, countriesAction.Result);

                }
                catch (IndexOutOfRangeException)
                {
                    //!Add tu on this
                    return (StatusCodes.Status416RequestedRangeNotSatisfiable, null);
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

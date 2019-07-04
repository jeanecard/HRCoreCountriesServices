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
                Task<HRBorder> bordersAction = _borderService.GetBorderAsync(id);
                await bordersAction;
                //3-
                HRBorder resultAction = bordersAction.Result;
                if (resultAction != null)
                {
                    if (!String.IsNullOrEmpty(resultAction.FIPS)
                        && resultAction.FIPS.ToUpper() == id.ToUpper())
                    {
                        return (StatusCodes.Status200OK, resultAction);
                    }
                    else
                    {
                        return (StatusCodes.Status404NotFound, null);
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
        /// <param name="orderBy">The ordering param. Retrun a status 400 bad request is underlying services don't know how to order. Can be null.</param>
        /// <returns>The HRBorders corresponding to pageModel parameter. Can throw MemberAccessException if any service is not consistant.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status416RequestedRangeNotSatisfiable)]
        [ProducesResponseType(StatusCodes.Status413PayloadTooLarge)]
        public async Task<ActionResult<PagingParameterOutModel<HRBorder>>> Get([FromQuery] PagingParameterInModel pageModel,
            [FromQuery]  HRSortingParamModel orderBy)
        {
            Task<(int, PagingParameterOutModel<HRBorder>)> result = GetFromPaging(pageModel, orderBy);
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
        /// 2- Get the HRBorders from service
        /// 3- Paginate previous result
        /// !Strange we have to âss from query even for an "internal" method ... to untderstand.
        /// </summary>
        /// <param name="pageModel">The Paging Model</param>
        /// <returns>(http Status Code, PagingParameterOutModel)</returns>
        public async Task<(int, PagingParameterOutModel<HRBorder>)> GetFromPaging(
            [FromQuery] PagingParameterInModel pageModel,
            [FromQuery]  HRSortingParamModel orderBy)
        {
            if (_borderService != null)
            {
                if (orderBy != null && orderBy.IsInitialised())
                {
                    if (!_borderService.IsSortable())
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
                    Task<PagingParameterOutModel<HRBorder>> bordersAction = _borderService.GetBordersAsync(pageModel, orderBy);
                    await bordersAction;
                    //3-
                    return (StatusCodes.Status200OK, bordersAction.Result);

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
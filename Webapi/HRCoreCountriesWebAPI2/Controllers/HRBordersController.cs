using HRCommonModel;
using HRCommonTools.Interace;
using HRCoreBordersModel;
using HRCoreBordersRepository;
using HRCoreBordersServices;
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
        /// 1- Process PagingInParameter if not supplied
        /// 2- Get the HRBorders from service
        /// 3- Paginate previous result
        /// </summary>
        /// <param name="pageModel">The PagingInParameter. Can be null (will be set to server Default)</param>
        /// <returns>The HRBorders corresponding to pageModel parameter. Can throw MemberAccessException if any service is not consistant.</returns>
        [HttpGet]
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
                Task<IEnumerable<HRBorder>> bordersAction = _borderService.GetBorders();
                await bordersAction;
                //3-
                PagingParameterOutModel<HRBorder> retour = _paginer.GetPagination(pageModel, bordersAction.Result);
                return retour;
            }
            else
            {
                throw new MemberAccessException();
            }
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
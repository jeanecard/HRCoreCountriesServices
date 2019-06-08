using HRCommonModel;
using HRCommonTools.Interace;
using HRCoreCountriesServices;
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
        public HRCountriesController(ICoreCountriesService service, IConfiguration config, IHRPaginer<HRCountry> paginer)
        {
            _service = service;
            _config = config;
            _paginer = paginer;
        }



        [HttpGet]
        public async Task<ActionResult<PagingParameterOutModel<HRCountry>>> GetAllAsync([FromQuery] PagingParameterInModel pageModel = null)
        {
            //1-
            if (pageModel == null)
            {
                pageModel = GetDefaultPagingInParameter();
            }
            if (_service != null && pageModel != null && _paginer != null)
            {
                //2-
                Task<IEnumerable<HRCountry>> serviceAction = _service.GetCountriesAsync();
                await serviceAction;
                //3-
                PagingParameterOutModel<HRCountry> retour = _paginer.GetPagination(pageModel, serviceAction.Result);
                return retour;
            }
            else
            {
                throw new MemberAccessException();
            }
        }

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

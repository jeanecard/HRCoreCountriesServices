using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRCoreBordersModel;
using HRCoreBordersRepository;
using HRCoreBordersServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRCoreCountriesWebAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HRBordersController : ControllerBase
    {
        //!TODO use DI
        private ICoreBordersService _borderService;// = new CoreBordersService(new CoreBordersRepository());
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HRBorder>>> Get([FromQuery] PagingParameterInModel pageModel)
        {

            _borderService = new CoreBordersService(new CoreBordersRepository());
            if (_borderService != null && pageModel != null)
            {
                Task<IEnumerable<HRBorder>> bordersAction = _borderService.GetBorders();
                await bordersAction;
                return bordersAction.Result.ToList();
            }
            else
            {
                throw new MemberAccessException();
            }
        }
    }
}
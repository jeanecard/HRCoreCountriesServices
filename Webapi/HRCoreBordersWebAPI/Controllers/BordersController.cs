using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRCoreBordersModel;
using HRCoreBordersRepository;
using HRCoreBordersServices;
using Microsoft.AspNetCore.Mvc;

namespace HRCoreBordersWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BordersController : ControllerBase
    {
        //!TODO use DI
        private ICoreBordersService _borderService;// = new CoreBordersService(new CoreBordersRepository());
        public BordersController(/*ICoreBordersService service PLEASE use Fucking Microsoft DI*/)
        {
            //_borderService = service;
        }

        /// <summary>
        /// Private to force construction with service injection.
        /// </summary>
        //private BordersController()
        //{

        //}
        // GET api/values
        /// <summary>
        /// Return all Feature from Borders Layer.
        /// throw MemberAccessException if borderService is not available.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HRBorder>>> Get()
        {
            _borderService = new CoreBordersService(new CoreBordersRepository());
            if(_borderService != null)
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

        // GET api/values/5
        /// <summary>
        /// throw MemberAccessException if borderService is not available.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<HRBorder>> Get(int id)
        {
            if (_borderService != null)
            {
                Task<IEnumerable<HRBorder>> bordersAction = _borderService.GetBorders(id);
                await bordersAction;
                return bordersAction.Result.FirstOrDefault();
            }
            else
            {
                throw new MemberAccessException();
            }

        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
            //Dummy.
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            //Dummy
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //Dummy
        }
    }
}

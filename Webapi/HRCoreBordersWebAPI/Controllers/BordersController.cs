using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRCoreBordersModel;
using Microsoft.AspNetCore.Mvc;

namespace HRCoreBordersWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BordersController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<HRBorder>> Get()
        {
            return null;;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<HRBorder> Get(int id)
        {
            return null;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HREFBirdRepository.Models;

namespace HRBordersAndCountriesWebAPI2.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HrbirdsController : ControllerBase
    {
        private readonly gxxawt_obddnfContext _context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public HrbirdsController(gxxawt_obddnfContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: api/Hrbirds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hrbird>>> GetHrbird()
        {
            return await _context.Hrbird.ToListAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Hrbirds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hrbird>> GetHrbird(string id)
        {
            var hrbird = await _context.Hrbird.FindAsync(id);

            if (hrbird == null)
            {
                return NotFound();
            }

            return hrbird;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hrbird"></param>
        /// <returns></returns>
        // PUT: api/Hrbirds/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHrbird(string id, Hrbird hrbird)
        {
            if (id != hrbird.Id)
            {
                return BadRequest();
            }

            _context.Entry(hrbird).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HrbirdExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="hrbird"></param>
        /// <returns></returns>

        // POST: api/Hrbirds
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Hrbird>> PostHrbird(Hrbird hrbird)
        {
            _context.Hrbird.Add(hrbird);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (HrbirdExists(hrbird.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetHrbird", new { id = hrbird.Id }, hrbird);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Hrbirds/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Hrbird>> DeleteHrbird(string id)
        {
            var hrbird = await _context.Hrbird.FindAsync(id);
            if (hrbird == null)
            {
                return NotFound();
            }

            _context.Hrbird.Remove(hrbird);
            await _context.SaveChangesAsync();

            return hrbird;
        }

        private bool HrbirdExists(string id)
        {
            return _context.Hrbird.Any(e => e.Id == id);
        }
    }
}

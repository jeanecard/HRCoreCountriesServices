using AutoMapper;
using HRBirdsDTOModel;
using HREFBirdRepository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRBordersAndCountriesWebAPI2.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HrsourcesController : ControllerBase
    {
        private readonly gxxawt_obddnfContext _context;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public HrsourcesController(gxxawt_obddnfContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: api/Hrsources
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HRSourceDTO>>> GetHrsource()
        {
            using var result = _context.Hrsource.ToListAsync();
            await result;
            return Ok(_mapper.Map<IEnumerable<HRSourceDTO>>(result.Result));
        }

        /// <summary>
        /// GET: api/Hrsources/5 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<HRSourceDTO>> GetHrsource(short id)
        {
            var hrsource = await _context.Hrsource.FindAsync(id);

            if (hrsource == null)
            {
                return NotFound();
            }
            return _mapper.Map<HRSourceDTO>(hrsource);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hrsource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHrsource(short id, Hrsource hrsource)
        {
            if (id != hrsource.IdSource)
            {
                return BadRequest();
            }

            _context.Entry(hrsource).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HrsourceExists(id))
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
        /// <param name="hrsource"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<HRSourceDTO>> PostHrsource(HRSourceDTO hrsource)
        {
            _context.Hrsource.Add(_mapper.Map<Hrsource>(hrsource));
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (HrsourceExists(hrsource.idSource))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetHrsource", new { id = hrsource.idSource }, hrsource);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<HRSourceDTO>> DeleteHrsource(short id)
        {
            var hrsource = await _context.Hrsource.FindAsync(id);
            if (hrsource == null)
            {
                return NotFound();
            }

            _context.Hrsource.Remove(hrsource);
            await _context.SaveChangesAsync();

            return _mapper.Map<HRSourceDTO>(hrsource);
        }

        private bool HrsourceExists(short id)
        {
            return _context.Hrsource.Any(e => e.IdSource == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChristmasPresents.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace ChristmasPresents.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class PresentsController : ControllerBase
    {
        private readonly ChristmasContext _context;

        public PresentsController(ChristmasContext context)
        {
            _context = context;
        }

        // GET: api/Presents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Present>>> GetPresents()
        {
            return await _context.Presents.Where(p => p.Kid.Hidden == 0).Include(p => p.Kid).ToListAsync();
        }

        // GET: api/Presents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Present>> GetPresent(int id)
        {
            var present = await _context.Presents.FindAsync(id);

            if (present == null)
            {
                return NotFound();
            }

            return present;
        }

        // PUT: api/Presents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutPresent(int id, Present present)
        {
            if (id != present.PresentId)
            {
                return BadRequest();
            }

            _context.Entry(present).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PresentExists(id))
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

        // POST: api/Presents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Present>> PostPresent(Present present)
        {
            _context.Presents.Add(present);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPresent", new { id = present.PresentId }, present);
        }

        // DELETE: api/Presents/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePresent(int id)
        {
            var present = await _context.Presents.FindAsync(id);
            if (present == null)
            {
                return NotFound();
            }

            _context.Presents.Remove(present);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PresentExists(int id)
        {
            return _context.Presents.Any(e => e.PresentId == id);
        }
    }
}

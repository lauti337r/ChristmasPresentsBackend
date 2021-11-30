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
    public class PresentGiversController : ControllerBase
    {
        private readonly ChristmasContext _context;

        public PresentGiversController(ChristmasContext context)
        {
            _context = context;
        }

        // GET: api/PresentGivers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PresentGiver>>> GetPresentGivers()
        {
            return await _context.PresentGivers.ToListAsync();
        }

        // GET: api/PresentGivers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PresentGiver>> GetPresentGiver(int id)
        {
            var presentGiver = await _context.PresentGivers.FindAsync(id);

            if (presentGiver == null)
            {
                return NotFound();
            }

            return presentGiver;
        }

        // PUT: api/PresentGivers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutPresentGiver(int id, PresentGiver presentGiver)
        {
            if (id != presentGiver.PresentGiverId)
            {
                return BadRequest();
            }

            _context.Entry(presentGiver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PresentGiverExists(id))
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

        // POST: api/PresentGivers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{kidId}")]
        public async Task<ActionResult<PresentGiver>> PostPresentGiver(int kidId, PresentGiver presentGiver)
        {
            var present = _context.Presents.Where(p => p.KidId == kidId).FirstOrDefault();

            _context.PresentGivers.Add(presentGiver);

            await _context.SaveChangesAsync();

            present.PresentGiverId = presentGiver.PresentGiverId;

            var kid = _context.Kids.Where(k => k.KidId == kidId).FirstOrDefault();

            kid.Hidden = 1;

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPresentGiver", new { id = presentGiver.PresentGiverId }, presentGiver);
        }

        // DELETE: api/PresentGivers/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePresentGiver(int id)
        {
            var presentGiver = await _context.PresentGivers.FindAsync(id);
            if (presentGiver == null)
            {
                return NotFound();
            }

            _context.PresentGivers.Remove(presentGiver);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PresentGiverExists(int id)
        {
            return _context.PresentGivers.Any(e => e.PresentGiverId == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
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
    public class KidsController : ControllerBase
    {
        private readonly ChristmasContext _context;

        public KidsController(ChristmasContext context)
        {
            _context = context;
        }

        // GET: api/Kids
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kid>>> GetKids()
        {
            return await _context.Kids.Where(k => k.Hidden == 0).Include(k => k.Present).ToListAsync();
        }

        // GET: api/Kids/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Kid>> GetKid(int id)
        {
            var kid = _context.Kids.Where(k => k.KidId == id).Include(k => k.Present).FirstOrDefault();

            if (kid == null)
            {
                return NotFound();
            }

            return kid;
        }

        // PUT: api/Kids/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutKid(int id, Kid kid)
        {
            if (id != kid.KidId)
            {
                return BadRequest();
            }

            _context.Entry(kid).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KidExists(id))
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

        // POST: api/Kids
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Kid>> PostKid(Kid kid)
        {
            _context.Kids.Add(kid);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKid", new { id = kid.KidId }, kid);
        }

        // DELETE: api/Kids/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteKid(int id)
        {
            var kid = await _context.Kids.FindAsync(id);
            if (kid == null)
            {
                return NotFound();
            }

            _context.Kids.Remove(kid);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KidExists(int id)
        {
            return _context.Kids.Any(e => e.KidId == id);
        }



        [HttpGet("Populate")]
        public async Task<ActionResult<bool>> Populate()
        {
            List<string> pictures = new List<string>()
            {
                @"https://images.generated.photos/DpkMyXGj-gZW6xB4mp328Jp2grJT55n_JrGRY5CrUlE/rs:fit:512:512/wm:0.95:sowe:18:18:0.33/czM6Ly9pY29uczgu/Z3Bob3Rvcy1wcm9k/LmNvbmQvOTBjMGU1/YzktYmMxYS00YWRh/LWI0NWYtMzk1NjZi/OTY1MTg3LmpwZw.jpg",
                @"https://images.generated.photos/_cY-7_n5ymHs_FuauPzbriMUwsgulIfpSd3BKz9TZLw/rs:fit:512:512/wm:0.95:sowe:18:18:0.33/czM6Ly9pY29uczgu/Z3Bob3Rvcy1wcm9k/LmNvbmQvNDk0YTZj/MzQtYWU0Ny00YzUz/LWJhNGEtYzRhNWMw/Nzc2OWEwLmpwZw.jpg",
                @"https://images.generated.photos/7jYtfobkF1Rxxgm6R1EkJftjGXvHqIl7dEX_m28eXDQ/rs:fit:512:512/wm:0.95:sowe:18:18:0.33/czM6Ly9pY29uczgu/Z3Bob3Rvcy1wcm9k/LmNvbmQvYjI1NTk5/OGUtN2NhMy00YTE5/LTg3ZGEtZTdlM2U0/NWRhNWQ2LmpwZw.jpg",
                @"https://images.generated.photos/32DxtRzmg8hyAvJ9LmubuKyvO5lG1V2xzMPHti9U8yo/rs:fit:512:512/wm:0.95:sowe:18:18:0.33/czM6Ly9pY29uczgu/Z3Bob3Rvcy1wcm9k/LmNvbmQvN2FmZjRh/ZmItZDYyNS00MDI2/LWJjMjctNjU3MTRl/ZDJmOTc4LmpwZw.jpg"
            };

            using (var reader = new StreamReader(@"C:\Users\lau33\Documents\Suma de Voluntades\barriosanmartin.csv"))
            {
                //Apellido y nombre ;Regalo;Negocio 
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    Kid k = new Kid();
                    k.Name = values[0].Trim();
                    k.Area = "San Martín";
                    k.Hidden = 0;
                    k.PictureUrl = pictures[new Random(DateTime.Now.Millisecond).Next(0, 4)];
                    k.Age = "";
                    k.Note = "";
                    _context.Kids.Add(k);
                    _context.SaveChanges();

                    Present p = new Present();
                    p.Name = values[1].Trim();
                    p.ShopName = values[2].Trim();
                    p.Cost = 1000;
                    p.KidId = k.KidId;

                    _context.Presents.Add(p);
                    _context.SaveChanges();
                }

                return true;
            }
        }
    }
}

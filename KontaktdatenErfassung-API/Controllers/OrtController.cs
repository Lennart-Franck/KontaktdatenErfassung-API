using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KontaktdatenErfassung_API.Models;

namespace KontaktdatenErfassung_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrtController : ControllerBase
    {
        private readonly KontaktdatenDBContext _context;

        public OrtController(KontaktdatenDBContext context)
        {
            _context = context;
        }

        // GET: api/Ort
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ort>>> GetOrt()
        {
            return await _context.Ort.ToListAsync();
        }

        // GET: api/Ort/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ort>> GetOrt(int id)
        {
            var ort = await _context.Ort.FindAsync(id);

            if (ort == null)
            {
                return NotFound();
            }

            return ort;
        }

        // PUT: api/Ort/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrt(int id, Ort ort)
        {
            if (id != ort.Id)
            {
                return BadRequest();
            }

            _context.Entry(ort).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrtExists(id))
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

        // POST: api/Ort
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Ort>> PostOrt(Ort ort)
        {
            _context.Ort.Add(ort);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrt", new { id = ort.Id }, ort);
        }

        // DELETE: api/Ort/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Ort>> DeleteOrt(int id)
        {
            var ort = await _context.Ort.FindAsync(id);
            if (ort == null)
            {
                return NotFound();
            }

            _context.Ort.Remove(ort);
            await _context.SaveChangesAsync();

            return ort;
        }

        private bool OrtExists(int id)
        {
            return _context.Ort.Any(e => e.Id == id);
        }
    }
}

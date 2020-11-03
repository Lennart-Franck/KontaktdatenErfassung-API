using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KontaktdatenErfassung_API.Models;
using System;

namespace KontaktdatenErfassung_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AufenthaltController : ControllerBase
    {
        private readonly KontaktdatenDBContext _context;

        public AufenthaltController(KontaktdatenDBContext context)
        {
            _context = context;
        }

        //Soll nicht erreichbar sein
        // GET: api/Aufenthalt
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Aufenthalt>>> GetAufenthalt()
        //{
        //    return await _context.Aufenthalt.ToListAsync();
        //}

        //Aufenthalte für die Person bekommen
        // GET: api/Aufenthalt/5
        [HttpGet("{PersonID}")]
        public async Task<ActionResult<IEnumerable<Aufenthalt>>> GetAufenthalt(Guid PersonID)
        {
            var aufenthalte = await _context.Aufenthalt.Where(x => x.PersonId == PersonID).ToListAsync();

            if (aufenthalte == null)
            {
                return NotFound();
            }

            return aufenthalte;
        }

        // PUT: api/Aufenthalt/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAufenthalt(int id, Aufenthalt aufenthalt)
        {
            if (id != aufenthalt.Id)
            {
                return BadRequest();
            }

            _context.Entry(aufenthalt).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AufenthaltExists(id))
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

        // POST: api/Aufenthalt
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Aufenthalt>> PostAufenthalt(Aufenthalt aufenthalt)
        {
            _context.Aufenthalt.Add(aufenthalt);
            await _context.SaveChangesAsync();

            return aufenthalt;
        }

        // DELETE: api/Aufenthalt/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Aufenthalt>> DeleteAufenthalt(int id)
        {
            var aufenthalt = await _context.Aufenthalt.FindAsync(id);
            if (aufenthalt == null)
            {
                return NotFound();
            }

            _context.Aufenthalt.Remove(aufenthalt);
            await _context.SaveChangesAsync();

            return aufenthalt;
        }

        private bool AufenthaltExists(int id)
        {
            return _context.Aufenthalt.Any(e => e.Id == id);
        }
    }
}

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
    public class AufenthaltController : ControllerBase
    {
        private KontaktdatenDBContext _context;

        public AufenthaltController(KontaktdatenDBContext context)
        {
            _context = context;
        }

        // GET: api/Aufenthalt
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Aufenthalt>>> GetAufenthalt()
        //{
        //    return await _context.Aufenthalt.ToListAsync();
        //}

        // GET: api/Aufenthalt/5
        [HttpGet("{PersonID}")]
        public async Task<ActionResult<List<Aufenthalt>>> GetAufenthalteByPerson(Guid PersonID)
        {
            var aufenthalt = await _context.Aufenthalt.Where(x => x.PersonId == PersonID).ToListAsync();

            if (aufenthalt == null)
            {
                return NotFound();
            }

            return aufenthalt;
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
        public async Task<ActionResult<Aufenthalt>> PostAufenthalt(Aufenthalt Aufenthalt)
        {
            _context.Aufenthalt.Add(Aufenthalt);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (AufenthaltExists(Aufenthalt.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Aufenthalt;
        }

        // DELETE: api/Aufenthalt/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Aufenthalt>> DeleteAufenthalt(int id)
        {
            var Aufenthalt = await _context.Aufenthalt.FindAsync(id);
            if (Aufenthalt == null)
            {
                return NotFound();
            }

            _context.Aufenthalt.Remove(Aufenthalt);
            await _context.SaveChangesAsync();

            return Aufenthalt;
        }

        private bool AufenthaltExists(int id)
        {
            return _context.Aufenthalt.Any(e => e.Id == id);
        }
    }
}

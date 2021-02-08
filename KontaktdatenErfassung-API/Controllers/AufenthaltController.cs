using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KontaktdatenErfassung_API.Models;
using Microsoft.AspNetCore.Authorization;
using KontaktdatenErfassung_API.Attributes;

namespace KontaktdatenErfassung_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    [ApiKey]
    public class AufenthaltController : ControllerBase
    {
        private KontaktdatenDBContext _context;

        public AufenthaltController(KontaktdatenDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gibt die alle Aufenthalte für die Person wieder
        /// </summary>
        /// <param name="PersonID">Die <see cref="Guid"/>Instanz der PersonenID.</param>
        /// <returns>Eine Instanz der <see cref="List{T}"/> mit Instanzen der <see cref="Aufenthalt"/> Klasse</returns>
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

        /// <summary>
        /// Aktualisiert einen Aufenthalt
        /// </summary>
        /// <param name="id">Die <see cref="int"/> Instanz der AufenthaltID</param>
        /// <param name="aufenthalt">Die aktualisierte <see cref="Aufenthalt"/></param>
        /// <returns>Eine Instanz der <see cref="IActionResult"/> Klasse mit dem HTTP Code</returns>
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

        /// <summary>
        /// Erstellt einen Aufenthalt 
        /// </summary>
        /// <param name="Aufenthalt">Der zu erstellende <see cref="Aufenthalt"/></param>
        /// <returns>Eine Instanz der erstellten <see cref="Aufenthalt"/> Klasse</returns>
        [HttpPost]
        public async Task<ActionResult<Aufenthalt>> PostAufenthalt(Aufenthalt Aufenthalt)
        {
            Aufenthalt.DatumBis = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            _context.Aufenthalt.Add(Aufenthalt);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
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

        /// <summary>
        /// Löscht einen Aufenthalt
        /// </summary>
        /// <param name="id">Die AufenthaltID des zu löschenden Aufenthaltes</param>
        /// <returns>Die gelöschte Instanz der <see cref="Aufenthalt"/> Klasse</returns>
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

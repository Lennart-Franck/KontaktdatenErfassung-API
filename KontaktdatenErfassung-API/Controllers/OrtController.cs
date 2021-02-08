using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public class OrtController : ControllerBase
    {
        private readonly KontaktdatenDBContext _context;

        public OrtController(KontaktdatenDBContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Gibt einen Ort zurück
        /// </summary>
        /// <param name="id">Die Guid des Ortes</param>
        /// <returns>Eine Instanz der <see cref="Ort"/> Klasse</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Ort>> GetOrt(Guid id)
        {
            var ort = await _context.Ort.FindAsync(id);

            if (ort == null)
            {
                return NotFound();
            }

            return ort;
        }

        /// <summary>
        /// Aktualisiert einen Ort
        /// </summary>
        /// <param name="id">Die Guid des Ortes</param>
        /// <param name="Ort">Die aktuelle <see cref="Ort"/> Klasse</param>
        /// <returns>Eine Instanz der <see cref="IActionResult"/> Klasse</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrt(Guid id, Ort Ort)
        {
            if (id != Ort.OrtId)
            {
                return BadRequest();
            }

            _context.Entry(Ort).State = EntityState.Modified;

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

        /// <summary>
        /// Erstellt einen Ort
        /// </summary>
        /// <param name="Ort">Der zu erstellende Ort</param>
        /// <returns>Eine Instanz der <see cref="Ort"/> Klasse</returns>
        [HttpPost]
        public async Task<ActionResult<Ort>> PostOrt(Ort Ort)
        {
            _context.Ort.Add(Ort);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrtExists(Ort.OrtId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrt", new { id = Ort.OrtId }, Ort);
        }

        /// <summary>
        /// Löscht einen Ort
        /// </summary>
        /// <param name="id">Die <see cref="Guid"/> der OrtID</param>
        /// <returns>Eine Instanz der <see cref="Ort"/> Klasse</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Ort>> DeleteOrt(Guid id)
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

        private bool OrtExists(Guid id)
        {
            return _context.Ort.Any(e => e.OrtId == id);
        }
    }
}

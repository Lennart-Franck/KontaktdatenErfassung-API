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
    public class PersonController : ControllerBase
    {
        private KontaktdatenDBContext _context;

        public PersonController(KontaktdatenDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Sucht eine Person anhand der Guid und gibt diese zurück
        /// </summary>
        /// <param name="id">Die <see cref="Guid"/> der Person</param>
        /// <returns>Eine Instanz der <see cref="Person"/> Klasse</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(Guid id)
        {
            var person = await _context.Person.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        /// <summary>
        /// Aktualisiert eine Person
        /// </summary>
        /// <param name="id">Die <see cref="Guid"/> der Person</param>
        /// <param name="Person">Die Instanz der zu aktualisierenden <see cref="Person"/> Klasse</param>
        /// <returns>Eine Instanz der <see cref="IActionResult"/></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(Guid id, Person Person)
        {
            if (id != Person.PersonId)
            {
                return BadRequest();
            }

            _context.Entry(Person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
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
        /// Erstellt eine Person
        /// </summary>
        /// <param name="Person">Eine Instanz der <see cref="Person"/> Klasse</param>
        /// <returns>Eine Instanz der erstellten <see cref="Person"/> Klasse</returns>
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person Person)
        {
            _context.Person.Add(Person);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PersonExists(Person.PersonId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPerson", new { id = Person.PersonId }, Person);
        }

        /// <summary>
        /// Löscht eine Person
        /// </summary>
        /// <param name="id">Die <see cref="Guid"/> der Person</param>
        /// <returns>Eine Instanz der <see cref="IActionResult"/> Klasse<returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Person>> DeletePerson(Guid id)
        {
            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Person.Remove(person);
            await _context.SaveChangesAsync();

            return person;
        }

        private bool PersonExists(Guid id)
        {
            return _context.Person.Any(e => e.PersonId == id);
        }
    }
}

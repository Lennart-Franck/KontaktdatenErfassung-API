using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KontaktdatenErfassung_API.Models;
using KontaktdatenErfassung_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KontaktdatenErfassung_API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly KontaktdatenDBContext _context;

        public AuthController(KontaktdatenDBContext context)
        {
            _context = context;
        }


        [HttpPost("api/sign-up")]
        public async Task<ActionResult<Unternehmen>> SignUp([FromBody] Unternehmen unternehmen)
        {
            unternehmen.Passwort = EncryptionService.EncodePassword(unternehmen.Passwort);

            _context.Unternehmen.Add(unternehmen);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest();
            }

            return unternehmen;
        }

        [HttpPost("api/login")]
        public async Task<ActionResult<Unternehmen>> Login([FromBody] string email, string password)
        {
            var list =  await _context.Unternehmen.Where(x => x.Email == email).ToListAsync();

            foreach (var item in list)
            {
                if (EncryptionService.CheckPassword(item.Passwort, password))
                {
                    return item;
                }
            }

            return BadRequest();
        }

        [HttpGet("api/places/{id}")]
        public async Task<ActionResult<List<Ort>>> GetPlaces(int id)
        {
            var places = await _context.Ort.Where(x => x.UnternehmenId == id).ToListAsync();

            if (places == null)
            {
                return NotFound();
            }

            return places;
        }

        [HttpGet("api/visits/{id}")]
        public async Task<ActionResult<List<Aufenthalt>>> GetVisits(Guid id, DateTime? from = null, DateTime? till = null)
        {
            var visits = new List<Aufenthalt>();

            if (from == null)
            {
                visits = await _context.Aufenthalt.Where(x => x.OrtId == id).ToListAsync();
            }
            else
            {
                visits = await _context.Aufenthalt.Where(x => x.OrtId == id && x.DatumVon == from && x.DatumBis == till).ToListAsync();

            }



            if (visits == null)
            {
                return NotFound();
            }

            return visits;
        }
    }
}

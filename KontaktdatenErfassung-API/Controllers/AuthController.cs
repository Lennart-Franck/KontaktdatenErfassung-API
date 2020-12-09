using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using KontaktdatenErfassung_API.Models;
using KontaktdatenErfassung_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KontaktdatenErfassung_API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly KontaktdatenDBContext _context;
        private readonly IConfiguration _config;

        public AuthController(KontaktdatenDBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;

        }
        

        [HttpPost("api/sign-up")]
        [AllowAnonymous]
        public IActionResult SignUp([FromBody] Unternehmen unternehmen)
        {
            IActionResult response = BadRequest();

            if (_context.Unternehmen.Where(x => x.Email == unternehmen.Email).ToList().Count >= 1)
            {
                response = Conflict("Diese Email ist bereits registriert");
                return response;
            }


            unternehmen.Passwort = EncryptionService.EncodePassword(unternehmen.Passwort);

            _context.Unternehmen.Add(unternehmen);
            try
            {
                _context.SaveChanges();
                User user = new User() { Email = unternehmen.Email, Name = unternehmen.Name, UnternehmenID = unternehmen.UnternehmenId};
                var tokenString = GenerateJWTToken(user);

                response = Ok(new
                {
                    token = tokenString,
                    userDetails = user
                });
            }
            catch (DbUpdateException)
            {

            }

            return response;
        }

        [HttpPost("api/login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] User login)
        {
            IActionResult response = BadRequest("Email oder Passwort sind nicht korrekt");

            User user = AuthenticateUser(login);

            if(user != null)
            {
                var tokenString = GenerateJWTToken(user);

                response = Ok(new
                {
                    token = tokenString,
                    userDetails = user
                });
            }

            return response;
        }

        
        private string GenerateJWTToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                new Claim("role", "User"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(issuer: _config["Jwt:Issuer"], audience: _config["Jwt:Audience"], claims: claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private User AuthenticateUser(User loginCredentials)
        {
            var list =  _context.Unternehmen.Where(x => x.Email == loginCredentials.Email).ToList();

            foreach (var item in list)
            {
                if (EncryptionService.CheckPassword(item.Passwort, loginCredentials.Passwort))
                {
                    return new User() { Email = item.Email, Name = item.Name, UnternehmenID = item.UnternehmenId};
                }
            }
            return null;
        }




        [HttpGet("api/places/{id}")]
        [Authorize(Policy = Policies.User)]
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
        [Authorize(Policy = Policies.User)]
        public IActionResult GetVisits(string id)
        {
            int week = 0;
            int month = 0;
            Guid guid;

            try
            {
                guid = new Guid(id);
            }
            catch
            {
                return BadRequest("Guid hat das falsche Format");
            }
            


            if(_context.Ort.Find(guid) == null)
            {
                return BadRequest("Ort ist nicht vorhanden");
            }

            DateTime dateWeekAgo = DateTime.Now.Date.AddDays(-7);
            DateTime dateMonthAgo = DateTime.Now.Date.AddDays(-30);

            week = _context.Aufenthalt.Where(x => DateTime.Compare(x.DatumVon, dateWeekAgo) > 0 && x.OrtId == guid).ToList().Count;

            month = _context.Aufenthalt.Where(x => DateTime.Compare(x.DatumVon, dateMonthAgo) > 0 && x.OrtId == guid).ToList().Count;

            var result = new
            {
                week = week,
                month = month
            };

            return Ok(result);

        }

        [HttpPost("api/places")]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult> PostPlace([FromBody] Ort ort)
        {
            try
            {
                await _context.Ort.AddAsync(ort);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest();
            }
            return Ok(ort);
        }

    }
}

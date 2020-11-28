using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KontaktdatenErfassung_API.Models
{
    public class User 
    { 
        public string Email { get; set; } 
        public string Passwort { get; set; }
        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace KontaktdatenErfassung_API.Models
{
    public partial class Unternehmen
    {
        public Unternehmen()
        {
            Ort = new HashSet<Ort>();
        }

        public int UnternehmenId { get; set; }
        public string Email { get; set; }
        public string Passwort { get; set; }
        public string Telefon { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Ort> Ort { get; set; }
    }
}

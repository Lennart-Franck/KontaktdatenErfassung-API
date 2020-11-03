using System;
using System.Collections.Generic;

namespace KontaktdatenErfassung_API.Models
{
    public partial class Ort
    {
        public Ort()
        {
            Aufenthalt = new HashSet<Aufenthalt>();
        }

        public Guid Id { get; set; }
        public string Bezeichnung { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Straße { get; set; }
        public string Hausnummer { get; set; }
        public int Plz { get; set; }
        public string Stadt { get; set; }

        public virtual ICollection<Aufenthalt> Aufenthalt { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace KontaktdatenErfassung_API.Models
{
    public partial class Person
    {
        public Person()
        {
            Aufenthalt = new HashSet<Aufenthalt>();
        }

        public Guid PersonId { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Straße { get; set; }
        public string Hausnummer { get; set; }
        public string Plz { get; set; }
        public string Stadt { get; set; }

        public virtual ICollection<Aufenthalt> Aufenthalt { get; set; }
    }
}

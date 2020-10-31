using System;
using System.Collections.Generic;

namespace KontaktdatenErfassung_API.Models
{
    public partial class Aufenthalt
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int OrtId { get; set; }
        public DateTime DatumVon { get; set; }
        public DateTime? DatumBis { get; set; }

        public virtual Ort Ort { get; set; }
        public virtual Person Person { get; set; }
    }
}

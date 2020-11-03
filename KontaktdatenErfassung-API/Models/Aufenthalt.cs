using System;
using System.Collections.Generic;

namespace KontaktdatenErfassung_API.Models
{
    public partial class Aufenthalt
    {
        public int Id { get; set; }
        public Guid PersonId { get; set; }
        public DateTime DatumVon { get; set; }
        public DateTime? DatumBis { get; set; }
        public Guid OrtId { get; set; }

        public virtual Ort Ort { get; set; }
        public virtual Person Person { get; set; }
    }
}

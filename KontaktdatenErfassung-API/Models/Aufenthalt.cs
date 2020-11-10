using System;
using System.Collections.Generic;

namespace KontaktdatenErfassung_API.Models
{
    public partial class Aufenthalt
    {
        public int Id { get; set; }
        public Guid PersonId { get; set; }
        public Guid OrtId { get; set; }
        public DateTime VonDatum { get; set; }
        public DateTime? BisDatum { get; set; }

        public virtual Ort Ort { get; set; }
        public virtual Person Person { get; set; }
    }
}

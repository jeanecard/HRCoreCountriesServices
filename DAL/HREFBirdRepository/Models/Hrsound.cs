using System;
using System.Collections.Generic;

namespace HREFBirdRepository.Models
{
    public partial class Hrsound
    {
        public string IdBird { get; set; }
        public int IdTypeSound { get; set; }
        public short IdSource { get; set; }
        public string Value { get; set; }
        public int IdSound { get; set; }
    }
}

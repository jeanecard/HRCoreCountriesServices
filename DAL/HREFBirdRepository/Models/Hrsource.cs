using System;
using System.Collections.Generic;

namespace HREFBirdRepository.Models
{
    public partial class Hrsource
    {
        public Hrsource()
        {
            Hrbird = new HashSet<Hrbird>();
        }

        public short IdSource { get; set; }
        public string Caption { get; set; }

        public virtual ICollection<Hrbird> Hrbird { get; set; }
    }
}

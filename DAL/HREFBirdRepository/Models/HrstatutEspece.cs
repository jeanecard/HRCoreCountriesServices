using System;
using System.Collections.Generic;

namespace HREFBirdRepository.Models
{
    public partial class HrstatutEspece
    {
        public HrstatutEspece()
        {
            Hrbird = new HashSet<Hrbird>();
        }

        public short IdStatut { get; set; }
        public string Caption { get; set; }

        public virtual ICollection<Hrbird> Hrbird { get; set; }
    }
}

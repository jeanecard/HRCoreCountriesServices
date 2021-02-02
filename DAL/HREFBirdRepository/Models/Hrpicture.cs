using System;
using System.Collections.Generic;

namespace HREFBirdRepository.Models
{
    public partial class Hrpicture
    {
        public Hrpicture()
        {
            HrmainPicture = new HashSet<HrmainPicture>();
        }

        public string IdBird { get; set; }
        public string Link { get; set; }
        public short? IdTypeAge { get; set; }
        public bool? IdMale { get; set; }
        public short? Source { get; set; }
        public int IdPicture { get; set; }
        public string Credit { get; set; }

        public virtual ICollection<HrmainPicture> HrmainPicture { get; set; }
    }
}

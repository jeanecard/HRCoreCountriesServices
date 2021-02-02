using System;
using System.Collections.Generic;

namespace HREFBirdRepository.Models
{
    public partial class HrmainSound
    {
        public string IdBird { get; set; }
        public int? IdSound { get; set; }

        public virtual Hrbird IdBirdNavigation { get; set; }
        public virtual Hrsound IdSoundNavigation { get; set; }
    }
}

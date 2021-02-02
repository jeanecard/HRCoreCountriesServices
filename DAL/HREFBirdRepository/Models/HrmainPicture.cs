using System;
using System.Collections.Generic;

namespace HREFBirdRepository.Models
{
    public partial class HrmainPicture
    {
        public int? IdPicture { get; set; }
        public string IdBird { get; set; }

        public virtual Hrbird IdBirdNavigation { get; set; }
        public virtual Hrpicture IdPictureNavigation { get; set; }
    }
}

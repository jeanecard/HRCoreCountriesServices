using System;
using System.Collections.Generic;

namespace HREFBirdRepository.Models
{
    public partial class Hrnames
    {
        public string IdBird { get; set; }
        public string Name { get; set; }
        public string LangIso639 { get; set; }

        public virtual Hrbird IdBirdNavigation { get; set; }
    }
}

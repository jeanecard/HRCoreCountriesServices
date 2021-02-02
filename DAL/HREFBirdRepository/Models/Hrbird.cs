using System;
using System.Collections.Generic;

namespace HREFBirdRepository.Models
{
    public partial class Hrbird
    {
        public Hrbird()
        {
            Hrnames = new HashSet<Hrnames>();
        }

        public string Caption { get; set; }
        public short? EnvergureMax { get; set; }
        public string Id { get; set; }
        public short? OeufsParPonteMax { get; set; }
        public int? PeriodeRepro { get; set; }
        public short? PoidsMax { get; set; }
        public short? Source { get; set; }
        public short? Statut { get; set; }
        public short? LongueurMax { get; set; }
        public short? NombrePonteMax { get; set; }
        public short? NombrePonteMin { get; set; }
        public short? LongueurMin { get; set; }
        public short? PoidsMin { get; set; }
        public short? OeufsParPonteMin { get; set; }
        public short? EnvergureMin { get; set; }

        public virtual Hrsource SourceNavigation { get; set; }
        public virtual HrstatutEspece StatutNavigation { get; set; }
        public virtual HrmainPicture HrmainPicture { get; set; }
        public virtual ICollection<Hrnames> Hrnames { get; set; }
    }
}

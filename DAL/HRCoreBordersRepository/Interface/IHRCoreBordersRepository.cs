using HRCoreBordersModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRCoreBordersRepository.Interface
{
    public interface IHRCoreBordersRepository
    {
        Task<IEnumerable<HRBorder>> GetBorders(int? borderID = null);
    }
}

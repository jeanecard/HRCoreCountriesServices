using HRCoreBordersModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRCoreBordersServices
{
    public interface ICoreBordersService
    {
        Task<IEnumerable<HRBorder>> GetBorders(String borderID = null);
    }
}

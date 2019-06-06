using HRCoreBordersModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRCoreBordersServices
{
    public interface ICoreBordersService
    {
        Task<IEnumerable<HRBorder>> GetBordersAsync(String borderID = null);
    }
}

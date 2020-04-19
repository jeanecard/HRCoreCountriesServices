using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HRCoreBordersModel;
using HRCoreBordersServices;

namespace ControllersForkerTools.Utils.Interface
{
    public interface IHRBordersByContinentControllerForker
    {
        Task<(int, IEnumerable<HRBorder>)> GetHRBordersByContinentAsync(ICoreBordersService service, string id);
    }
}

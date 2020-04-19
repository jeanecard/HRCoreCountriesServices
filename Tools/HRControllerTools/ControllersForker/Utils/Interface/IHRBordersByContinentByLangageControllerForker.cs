using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HRCoreBordersModel;
using HRCoreBordersServices;

namespace ControllersForkerTools.Utils.Interface
{
    public interface IHRBordersByContinentByLangageControllerForker
    {
        Task<(int, IEnumerable<HRBorder>)> GetHRBordersByContinentByLanguageAsync(ICoreBordersService service, string continentId, string langageId);
    }
}

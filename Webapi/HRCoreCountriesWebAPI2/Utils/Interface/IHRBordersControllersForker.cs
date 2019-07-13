using HRCommonModel;
using HRCommonModels;
using HRCoreBordersModel;
using HRCoreBordersServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRBordersAndCountriesWebAPI2.Utils
{
    /// <summary>
    /// Util interface
    /// </summary>
    public interface IHRBordersControllersForker
    {
        /// <summary>
        /// GetFromIDAsync.
        /// </summary>
        /// <param name="id">The border ID.</param>
        /// <param name="borderService">The Border service.</param>
        /// <returns></returns>
        Task<(int, HRBorder)> GetFromIDAsync(String id, ICoreBordersService borderService);

        /// <summary>
        /// GetFromPagingAsync.
        /// </summary>
        /// <param name="pageModel">The PageModel.</param>
        /// <param name="orderBy">The OrderBy clause.</param>
        /// <param name="borderService">The Border service.</param>
        /// <param name="maxPageSize">The MaxPage size allowed.</param>
        /// <returns></returns>
        Task<(int, PagingParameterOutModel<HRBorder>)> GetFromPagingAsync(
            PagingParameterInModel pageModel,
            HRSortingParamModel orderBy,
            ICoreBordersService borderService,
            ushort maxPageSize);
    }
}

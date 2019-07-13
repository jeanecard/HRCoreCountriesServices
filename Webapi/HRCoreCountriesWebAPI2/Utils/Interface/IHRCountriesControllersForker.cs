using HRCommonModel;
using HRCommonModels;
using HRCoreCountriesServices;
using QuickType;
using System.Threading.Tasks;

namespace HRBordersAndCountriesWebAPI2.Utils
{
    /// <summary>
    /// HRCountriesControllersForker interface.
    /// </summary>
    public interface IHRCountriesControllersForker
    {
        /// <summary>
        /// GetFromIDAsync.
        /// </summary>
        /// <param name="id">a Country ID.</param>
        /// <param name="service">a Country service.</param>
        /// <returns></returns>
        Task<(int, HRCountry)> GetFromIDAsync(string id, ICoreCountriesService service);
        /// <summary>
        /// GetFromPagingAsync.
        /// </summary>
        /// <param name="pageModel">The PageModel.</param>
        /// <param name="orderBy">The orderBy clause.</param>
        /// <param name="service">The orderable Service.</param>
        /// <param name="maxPageSize">The Max page size allowed.</param>
        /// <returns></returns>
        Task<(int, PagingParameterOutModel<HRCountry>)> GetFromPagingAsync(
        PagingParameterInModel pageModel,
        HRSortingParamModel orderBy,
        ICoreCountriesService service,
        ushort maxPageSize);
    }
}

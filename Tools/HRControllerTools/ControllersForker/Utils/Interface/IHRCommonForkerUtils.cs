using HRCommon.Interface;
using HRCommonModels;

namespace HRControllersForker.Interface
{
    /// <summary>
    /// Interface on HRCommonForkerUtils use for UT
    /// </summary>
    public interface IHRCommonForkerUtils
    {
        /// <summary>
        /// Method use to determine if the orderBy clause with the Sortable Service can order properly.
        /// </summary>
        /// <param name="orderBy">a OrderBy Clause. Can be null.</param>
        /// <param name="service">a Service orderable. Can be null.</param>
        /// <returns></returns>
        bool CanOrder(HRSortingParamModel orderBy, ISortable service);
    }
}

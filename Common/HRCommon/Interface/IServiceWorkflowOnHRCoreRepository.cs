using HRCommonModel;
using HRCommonModels;
using System.Threading.Tasks;

namespace HRCommon.Interface
{
    /// <summary>
    /// Interface on Workflow to get a list of items considering that repository is sortable or not, paginable or not.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IServiceWorkflowOnHRCoreRepository<T>
    {
        Task<PagingParameterOutModel<T>> GetQueryResultsAsync(
             PagingParameterInModel pageModel,
             HRSortingParamModel orderBy);

        ushort MaxPageSize { get; set; }
    }
}

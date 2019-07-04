using HRCommonModel;
using HRCommonModels;
using System.Threading.Tasks;

namespace HRCommon.Interface
{
    public interface IServiceWorkflowOnHRCoreRepository<T>
    {
        Task<PagingParameterOutModel<T>> GetQueryResultsAsync(
             PagingParameterInModel pageModel,
             HRSortingParamModel orderBy);

        ushort MaxPageSize { get; set; }
    }
}

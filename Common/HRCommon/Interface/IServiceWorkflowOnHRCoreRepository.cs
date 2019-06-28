using HRCommonModel;
using HRCommonModels;
using System;
using System.Collections.Generic;
using System.Text;
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

using HRCommon.Interface;
using HRCommonModel;
using HRCommonModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCoreRepository.Interface
{
    public interface IHRCoreRepository<T> : ISortable, IPaginable
    {
        Task<T> GetAsync(String id);
        Task<IEnumerable<T>> GetOrderedsAsync(HRSortingParamModel orderBy);
        Task<IEnumerable<T>> GetFullsAsync();

        Task<PagingParameterOutModel<T>> GetOrderedAndPaginatedsAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy);
        Task<PagingParameterOutModel<T>> GetPaginatedsAsync(PagingParameterInModel pageModel);
    }
}

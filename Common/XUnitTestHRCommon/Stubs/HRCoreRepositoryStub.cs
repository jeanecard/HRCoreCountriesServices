using HRCommonModel;
using HRCommonModels;
using HRCoreRepository.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XUnitTestHRCommon.Stubs
{
    /// <summary>
    /// Stub, so comments are very lights :-)
    /// </summary>
    class HRCoreRepositoryStub : IHRCoreRepository<int>
    {
        public bool _isSortable = true;
        public bool _isPaginable = true;
        public Task<int> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<int>> GetFullsAsync()
        {
            await Task.Delay(1);
            return new List<int>() { 45 };
        }

        public async Task<PagingParameterOutModel<int>> GetOrderedAndPaginatedsAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy)
        {
            await Task.Delay(1);
            return new PagingParameterOutModel<int>() { CurrentPage = 42 };
        }

        public async Task<IEnumerable<int>> GetOrderedsAsync(HRSortingParamModel orderBy)
        {
            await Task.Delay(1);
            return new List<int>() { 43 };
        }

        public async Task<PagingParameterOutModel<int>> GetPaginatedsAsync(PagingParameterInModel pageModel)
        {
            await Task.Delay(1);
            return new PagingParameterOutModel<int>() { CurrentPage = 44 };
        }

        public bool IsPaginable()
        {
            return _isPaginable;
        }

        public bool IsSortable()
        {
            return _isSortable;
        }
    }
}

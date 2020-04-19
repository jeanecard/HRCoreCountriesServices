using HRCommon;
using HRCommonModel;
using HRCommonModels;
using HRCommonTools;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using XUnitTestHRCommon.Stubs;

namespace XUnitTestHRCommon
{
    /// <summary>
    /// TODO
    /// </summary>
    public class HRServiceWorkflowPaginationOnlyTest
    {
        private HRCoreRepositoryStub _repo = null;
        private HRServiceWorkflowPaginationOnly<int> _stubbedPagination = null;
        public HRServiceWorkflowPaginationOnlyTest()
        {
            _repo = new HRCoreRepositoryStub();
            _stubbedPagination = new HRServiceWorkflowPaginationOnly<int>(_repo, new HRPaginer<int>());
        }
        /// <summary>
        /// Test that GetQueryResultsAsync throw MemberAccessException if repo or paginer is null;
        /// </summary>
        [Fact]
        public async void HRServiceWorkflowPaginationOnly_GetQueryResultsAsync_Throw_MemberAccessException_On_Members_Null()
        {
            HRServiceWorkflowPaginationOnly<int> noRepo = new HRServiceWorkflowPaginationOnly<int>(null, new HRPaginer<int>());
            HRServiceWorkflowPaginationOnly<int> noPaginer = new HRServiceWorkflowPaginationOnly<int>(new HRCoreRepositoryStub(), null);

            await Assert.ThrowsAsync<MemberAccessException>(async () => await noRepo.GetQueryResultsAsync(new PagingParameterInModel(), new HRSortingParamModel())); ;
            await Assert.ThrowsAsync<MemberAccessException>(async () => await noPaginer.GetQueryResultsAsync(new PagingParameterInModel(), new HRSortingParamModel()));
        }
        [Fact]
        /// <summary>
        /// Test that GetQueryResultsAsync throw ArgumentNullException with a null pageModel 
        /// </summary>
        public async void HRServiceWorkflowPaginationOnly_GetQueryResultsAsync_Throw_ArgumentNullException_With_Null_PageModel()
        {
            HRServiceWorkflowPaginationOnly<int> classic = new HRServiceWorkflowPaginationOnly<int>(new HRCoreRepositoryStub(), new HRPaginer<int>());
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await classic.GetQueryResultsAsync(null, new HRSortingParamModel())); ;
        }

        [Fact]
        /// <summary>
        /// Test that GetQueryResultsAsync throw NotSupportedException with a unsortable repository and a valid orderBy  
        /// </summary>
        public async void HRServiceWorkflowPaginationOnly_GetQueryResultsAsync_Throw_NotSupportedException_On_Unsortable_Repository()
        {
            _repo._isSortable = false;
            await Assert.ThrowsAsync<NotSupportedException>(async () => await _stubbedPagination.GetQueryResultsAsync(new PagingParameterInModel(), new HRSortingParamModel() { OrderBy = "name;asc" })); ;
        }
        [Fact]
        /// <summary>
        /// Test that GetQueryResultsAsync Retrun SortableAndPaginable from repo with a valid orderBy  
        /// </summary>
        public async void HRServiceWorkflowPaginationOnly_GetQueryResultsAsync_Return_Repository_Get_Ordered_And_Paginated_Async()
        {
            _repo._isSortable = true;
            Task<PagingParameterOutModel<int>> task = _stubbedPagination.GetQueryResultsAsync(
                new PagingParameterInModel(),
                new HRSortingParamModel() { OrderBy = "name;asc" });
            await task;
            Assert.NotNull(task);
            Assert.NotNull(task.Result);
            Assert.Equal(42, task.Result.CurrentPage);
        }
        [Fact]
        /// <summary>
        /// Test that GetQueryResultsAsync Retrun GetOrderedsAsync from repo unable to paginate and with a valid orderBy  
        /// </summary>
        public async void HRServiceWorkflowPaginationOnly_GetQueryResultsAsyncRetrunRepositoryGetOrderedsAsync()
        {
            _repo._isSortable = true;
            _repo._isPaginable = false;
            Task<PagingParameterOutModel<int>> task = _stubbedPagination.GetQueryResultsAsync(
                new PagingParameterInModel() { PageNumber = 0, PageSize = 10 },
                new HRSortingParamModel() { OrderBy = "name;asc" });
            await task;
            Assert.NotNull(task);
            Assert.NotNull(task.Result);
            Assert.NotNull(task.Result.PageItems);
            Assert.True(task.Result.PageItems.ToList()[0] == 43);
        }
        [Fact]
        /// <summary>
        /// Test that GetQueryResultsAsync with invalid PageModel throw InvalidProgramException  
        /// </summary>
        public async void HRServiceWorkflowPaginationOnly_GetQueryResultsAsyncWithInvalidPageModelThrowInvalidProgramException()
        {
            _repo._isSortable = true;
            _repo._isPaginable = false;
            await Assert.ThrowsAsync<InvalidProgramException>(async () => await _stubbedPagination.GetQueryResultsAsync(
                new PagingParameterInModel() { PageNumber = 500, PageSize = 10 },
                new HRSortingParamModel() { OrderBy = "name;asc" }));
        }
        [Fact]
        /// <summary>
        /// Test that GetQueryResultsAsync Retrun SortableAndPaginable from paginable repo. without orderBy  
        /// </summary>
        public async void HRServiceWorkflowPaginationOnly_GetQueryResultsAsyncRetrunRepositoryGetPaginatedsAsync()
        {
            _repo._isSortable = true;
            _repo._isPaginable = true;
            Task<PagingParameterOutModel<int>> task = _stubbedPagination.GetQueryResultsAsync(
                new PagingParameterInModel() { PageNumber = 0, PageSize = 20 },
                new HRSortingParamModel() { OrderBy = "" });
            await task;
            Assert.NotNull(task);
            Assert.NotNull(task.Result);
            Assert.Equal(44, task.Result.CurrentPage);
        }

        [Fact]
        /// <summary>
        /// Test that GetQueryResultsAsync Retrun GetFullsAsync from unpaginable repo without ordering.
        /// /// </summary>
        public async void HRServiceWorkflowPaginationOnly_GetQueryResultsAsyncRetrunRepositoryGetFullsAsync()
        {
            _repo._isSortable = true;
            _repo._isPaginable = false;
            Task<PagingParameterOutModel<int>> task = _stubbedPagination.GetQueryResultsAsync(
                new PagingParameterInModel() { PageNumber = 0, PageSize = 10 },
                null);
            await task;
            Assert.NotNull(task);
            Assert.NotNull(task.Result);
            Assert.NotNull(task.Result.PageItems);
            Assert.True(task.Result.PageItems.ToList()[0] == 45);
        }
    }
}

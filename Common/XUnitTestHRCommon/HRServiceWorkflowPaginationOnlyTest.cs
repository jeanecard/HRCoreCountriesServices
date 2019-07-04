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
    public class HRServiceWorkflowPaginationOnlyTest
    {
        /// <summary>
        /// Test that GetQueryResultsAsync throw MemberAccessException if repo or paginer is null;
        /// </summary>
        [Fact]
        public async void GetQueryResultsAsyncThrowMemberAccessExceptionOnMembersNull()
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
        public async void GetQueryResultsAsyncThrowArgumentNullExceptionWithNullPageModel()
        {
            HRServiceWorkflowPaginationOnly<int> classic = new HRServiceWorkflowPaginationOnly<int>(new HRCoreRepositoryStub(), new HRPaginer<int>());
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await classic.GetQueryResultsAsync(null, new HRSortingParamModel())); ;
        }

        [Fact]
        /// <summary>
        /// Test that GetQueryResultsAsync throw NotSupportedException with a unsuortable repository and a valid orderBy  
        /// </summary>
        public async void GetQueryResultsAsyncThrowNotSupportedExceptionOnUnsortableRepository()
        {
            HRCoreRepositoryStub repo = new HRCoreRepositoryStub();
            repo._isSortable = false;
            HRServiceWorkflowPaginationOnly<int> classic = new HRServiceWorkflowPaginationOnly<int>(repo, new HRPaginer<int>());

            await Assert.ThrowsAsync<NotSupportedException>(async () => await classic.GetQueryResultsAsync(new PagingParameterInModel(), new HRSortingParamModel() { SortingParamsQuery = "name;asc" })); ;
        }
        [Fact]
        /// <summary>
        /// Test that GetQueryResultsAsync Retrun SortableAndPAginable from repo with a valid orderBy  
        /// </summary>
        public async void GetQueryResultsAsyncRetrunRepositoryGetOrderedAndPaginatedsAsync()
        {
            HRCoreRepositoryStub repo = new HRCoreRepositoryStub();
            repo._isSortable = true;
            HRServiceWorkflowPaginationOnly<int> classic = new HRServiceWorkflowPaginationOnly<int>(repo, new HRPaginer<int>());
            Task<PagingParameterOutModel<int>> task = classic.GetQueryResultsAsync(
                new PagingParameterInModel(),
                new HRSortingParamModel() { SortingParamsQuery = "name;asc" });
            await task;
            Assert.NotNull(task);
            Assert.NotNull(task.Result);
            Assert.Equal(42, task.Result.CurrentPage);
        }
        [Fact]
        /// <summary>
        /// Test that GetQueryResultsAsync Retrun GetOrderedsAsync from repo unable to paginate and with a valid orderBy  
        /// </summary>
        public async void GetQueryResultsAsyncRetrunRepositoryGetOrderedsAsync()
        {
            HRCoreRepositoryStub repo = new HRCoreRepositoryStub();
            repo._isSortable = true;
            repo._isPaginable = false;
            HRServiceWorkflowPaginationOnly<int> classic = new HRServiceWorkflowPaginationOnly<int>(repo, new HRPaginer<int>());
            Task<PagingParameterOutModel<int>> task = classic.GetQueryResultsAsync(
                new PagingParameterInModel() { PageNumber = 0, PageSize = 10 },
                new HRSortingParamModel() { SortingParamsQuery = "name;asc" });
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
        public async void GetQueryResultsAsyncWithInvalidPageModelThrowInvalidProgramException()
        {
            HRCoreRepositoryStub repo = new HRCoreRepositoryStub();
            repo._isSortable = true;
            repo._isPaginable = false;
            HRServiceWorkflowPaginationOnly<int> classic = new HRServiceWorkflowPaginationOnly<int>(repo, new HRPaginer<int>());
            await Assert.ThrowsAsync<InvalidProgramException>(async () => await classic.GetQueryResultsAsync(
                new PagingParameterInModel() { PageNumber = 500, PageSize = 10 },
                new HRSortingParamModel() { SortingParamsQuery = "name;asc" }));
        }
        [Fact]
        /// <summary>
        /// Test that GetQueryResultsAsync Retrun SortableAndPaginable from paginable repo. without orderBy  
        /// </summary>
        public async void GetQueryResultsAsyncRetrunRepositoryGetPaginatedsAsync()
        {
            HRCoreRepositoryStub repo = new HRCoreRepositoryStub();
            repo._isSortable = true;
            repo._isPaginable = true;
            HRServiceWorkflowPaginationOnly<int> classic = new HRServiceWorkflowPaginationOnly<int>(repo, new HRPaginer<int>());
            Task<PagingParameterOutModel<int>> task = classic.GetQueryResultsAsync(
                new PagingParameterInModel() { PageNumber = 0, PageSize = 20 },
                new HRSortingParamModel() { SortingParamsQuery = "" });
            await task;
            Assert.NotNull(task);
            Assert.NotNull(task.Result);
            Assert.Equal(44, task.Result.CurrentPage);
        }

        [Fact]
        /// <summary>
        /// Test that GetQueryResultsAsync Retrun GetFullsAsync from unpaginable repo without ordering.
        /// /// </summary>
        public async void GetQueryResultsAsyncRetrunRepositoryGetFullsAsync()
        {
            HRCoreRepositoryStub repo = new HRCoreRepositoryStub();
            repo._isSortable = true;
            repo._isPaginable = false;
            HRServiceWorkflowPaginationOnly<int> classic = new HRServiceWorkflowPaginationOnly<int>(repo, new HRPaginer<int>());
            Task<PagingParameterOutModel<int>> task = classic.GetQueryResultsAsync(
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

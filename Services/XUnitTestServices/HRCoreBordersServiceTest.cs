using HRCommonModel;
using HRCommonModels;
using HRCommonTools;
using HRCoreBordersModel;
using HRCoreBordersServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using XUnitTestServices.MocksAndStubs;
using System.Linq;

namespace XUnitTestServices
{
    public class HRCoreBordersServiceTest
    {
        /// <summary>
        /// Test that MemberAccessException is thrown on GetBordersAsync with a null repository.
        /// </summary>
        [Fact]
        public async void BorderServiceOnGetBordersAsyncThrowMemberAccessExceptionIfRepositoryIsNull()
        {
            HRCoreBordersService service = new HRCoreBordersService(null, new HRPaginer<HRBorder>());
            await Assert.ThrowsAsync<MemberAccessException>(async () => await service.GetBordersAsync(new PagingParameterInModel(), null));

        }
        /// <summary>
        /// Test that MemberAccessException is thrown on GetBorderAsync with a null repository.
        /// </summary>
        [Fact]
        public async void BorderServiceOnGetBorderAsyncThrowMemberAccessExceptionIfRepositoryIsNull()
        {
            HRCoreBordersService service = new HRCoreBordersService(null, new HRPaginer<HRBorder>());
            await Assert.ThrowsAsync<MemberAccessException>(async () => await service.GetBorderAsync("xx"));

        }
        /// <summary>
        /// Test that MemberAccessException is thrown on GetBordersAsync with a null Paginer.
        /// </summary>
        [Fact]
        public async void BorderServiceOnGetBordersAsyncThrowMemberAccessExceptionIfPaginerIsNull()
        {
            HRCoreBordersService service = new HRCoreBordersService(new HRCoreBordersRepositoryStub(null, ""), null);
            await Assert.ThrowsAsync<MemberAccessException>(async () => await service.GetBordersAsync(new PagingParameterInModel(), null));
        }
        /// <summary>
        /// Test that ArgumentNullException is thrown if pageModel is null.
        /// </summary>
        [Fact]
        public async void BorderServiceOnGetBordersAsyncThrowArgumentNullExceptionIfPageModelIsNull()
        {
            HRCoreBordersService service = new HRCoreBordersService(new HRCoreBordersRepositoryStub(null, ""), new HRPaginer<HRBorder>());
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.GetBordersAsync(null, null));
        }

        /// <summary>
        /// Test that NotSupportedException is thrown On GetBorders with not sortable Repository
        /// </summary>
        [Fact]
        public async void BorderServiceOnGetBordersAsyncThrowNotSupportedExceptionOnOrderWithoutOrderingCapacityOnRepository()
        {
            HRCoreBordersService service = new HRCoreBordersService(new HRCoreBordersRepositoryStub(null, "", false), new HRPaginer<HRBorder>());
            await Assert.ThrowsAsync<NotSupportedException>(async () => await service.GetBordersAsync(new PagingParameterInModel(), new HRSortingParamModel()));
        }

        /// <summary>
        /// Verify that Service do not do any extra processing on GetBorders Paginable and sortable by repository
        /// /// </summary>
        [Fact]
        public async void BorderServiceOnGetBordersAsyncWithFullRepositoryReturnResultWithoutExtraProcessing()
        {
            HRCoreBordersRepositoryStub repository = new HRCoreBordersRepositoryStub(
                new List<String>() { "AA", "BB", "CC"}, 
                "BB", 
                true);
            HRCoreBordersService service = new HRCoreBordersService(repository, new HRPaginer<HRBorder>());
            Task<PagingParameterOutModel<HRBorder>> task = service.GetBordersAsync(new PagingParameterInModel() { PageNumber = 0, PageSize = 20 }, new HRSortingParamModel() { OrderBy = "POP2005;asc" });
            await task;
            Assert.NotNull(task.Result);
            Assert.True(task.Result.TotalItemsCount == 3);
            Assert.False(task.Result.HasNextPage);
            Assert.False(task.Result.HasPreviousPage);
            Assert.True(task.Result.PageSize == 20);
            Assert.True(task.Result.PageItems.ToList<HRBorder>()[0].FIPS == "AA");
            Assert.True(task.Result.PageItems.ToList<HRBorder>()[1].FIPS == "BB");
            Assert.True(task.Result.PageItems.ToList<HRBorder>()[2].FIPS == "CC");
        }

        /// <summary>
        /// Verify that Service do not do any extra processing on repository results but Pagination.
        /// </summary>
        [Fact]
        public async void BorderServiceOnGetBordersAsyncWithOrderOnlyRepositoryReturnResultPaginated()
        {
            List<String> borders = new List<string>();
            for(int i = 0; i < 60; i++)
            {
                borders.Add(i.ToString());
            }
            HRCoreBordersRepositoryStub repository = new HRCoreBordersRepositoryStub(
                borders,  "BB", true, false);
            HRCoreBordersService service = new HRCoreBordersService(repository, new HRPaginer<HRBorder>());
            Task<PagingParameterOutModel<HRBorder>> task = service.GetBordersAsync(new PagingParameterInModel() { PageNumber = 1, PageSize = 20 }, new HRSortingParamModel() { OrderBy = "POP2005;asc" });
            await task;
            Assert.NotNull(task.Result);
            Assert.True(task.Result.TotalItemsCount == 60);
            Assert.True(task.Result.HasNextPage);
            Assert.True(task.Result.HasPreviousPage);
            Assert.True(task.Result.PageSize == 20);
            Assert.True(task.Result.PageItems.ToList<HRBorder>()[0].FIPS == "20");
            Assert.True(task.Result.PageItems.ToList<HRBorder>()[1].FIPS == "21");
            Assert.True(task.Result.PageItems.ToList<HRBorder>()[2].FIPS == "22");
        }

        /// <summary>
        /// Verify that Service do not do any extra processing on GetBorders Paginable and sortable by repository
        /// /// </summary>
        [Fact]
        public async void BorderServiceOnGetBordersAsyncWithoutORderingWithFullRepositoryReturnResultWithoutExtraProcessing()
        {
            HRCoreBordersRepositoryStub repository = new HRCoreBordersRepositoryStub(
                new List<String>() { "AA", "BB", "CC" },
                "BB",
                true,
                true);
            HRCoreBordersService service = new HRCoreBordersService(repository, new HRPaginer<HRBorder>());
            Task<PagingParameterOutModel<HRBorder>> task = service.GetBordersAsync(new PagingParameterInModel() { PageNumber = 0, PageSize = 20 }, null);
            await task;
            Assert.NotNull(task.Result);
            Assert.True(task.Result.TotalItemsCount == 3);
            Assert.False(task.Result.HasNextPage);
            Assert.False(task.Result.HasPreviousPage);
            Assert.True(task.Result.PageSize == 20);
            Assert.True(task.Result.PageItems.ToList<HRBorder>()[0].FIPS == "AA");
            Assert.True(task.Result.PageItems.ToList<HRBorder>()[1].FIPS == "BB");
            Assert.True(task.Result.PageItems.ToList<HRBorder>()[2].FIPS == "CC");
        }
        /// <summary>
        /// Verify that Service do not do any extra processing on repository results but Pagination.
        /// </summary>
        [Fact]
        public async void BorderServiceOnGetBordersUnOrderedAsyncWithOrderOnlyRepositoryReturnResultPaginated()
        {
            List<String> borders = new List<string>();
            for (int i = 0; i < 60; i++)
            {
                borders.Add(i.ToString());
            }
            HRCoreBordersRepositoryStub repository = new HRCoreBordersRepositoryStub(
                borders, "BB", true, false);
            HRCoreBordersService service = new HRCoreBordersService(repository, new HRPaginer<HRBorder>());
            Task<PagingParameterOutModel<HRBorder>> task = service.GetBordersAsync(new PagingParameterInModel() { PageNumber = 1, PageSize = 20 }, null);
            await task;
            Assert.NotNull(task.Result);
            Assert.True(task.Result.TotalItemsCount == 60);
            Assert.True(task.Result.HasNextPage);
            Assert.True(task.Result.HasPreviousPage);
            Assert.True(task.Result.PageSize == 20);
            Assert.True(task.Result.PageItems.ToList<HRBorder>()[0].FIPS == "20");
            Assert.True(task.Result.PageItems.ToList<HRBorder>()[1].FIPS == "21");
            Assert.True(task.Result.PageItems.ToList<HRBorder>()[2].FIPS == "22");
        }

    }
}

using HRCommon;
using HRCommonModel;
using HRCommonModels;
using HRCommonTools;
using HRCoreBordersModel;
using HRCoreBordersServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using XUnitTestServices.MocksAndStubs;

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
            HRCoreBordersService service = new HRCoreBordersService(
                null,
                new HRServiceWorkflowPaginationOnly<HRBorder>(null, null));
            await Assert.ThrowsAsync<MemberAccessException>(async () => await service.GetBordersAsync(new PagingParameterInModel(), null));

        }
        /// <summary>
        /// Test that MemberAccessException is thrown on GetBorderAsync with a null repository.
        /// </summary>
        [Fact]
        public async void BorderServiceOnGetBorderAsyncThrowMemberAccessExceptionIfRepositoryIsNull()
        {
            HRCoreBordersService service = new HRCoreBordersService(
                null,
                new HRServiceWorkflowPaginationOnly<HRBorder>(null, null));
            await Assert.ThrowsAsync<MemberAccessException>(async () => await service.GetBorderAsync("xx"));

        }
        /// <summary>
        /// Test that MemberAccessException is thrown on GetBordersAsync with a null Paginer.
        /// </summary>
        [Fact]
        public async void BorderServiceOnGetBordersAsyncThrowMemberAccessExceptionIfPaginerIsNull()
        {
            HRCoreBordersService service = new HRCoreBordersService(
                new HRCoreBordersRepositoryStub(null, ""),
                new HRServiceWorkflowPaginationOnly<HRBorder>(null, null));
            await Assert.ThrowsAsync<MemberAccessException>(async () => await service.GetBordersAsync(new PagingParameterInModel(), null));
        }
        /// <summary>
        /// Test that ArgumentNullException is thrown if pageModel is null.
        /// </summary>
        [Fact]
        public async void BorderServiceOnGetBordersAsyncThrowArgumentNullExceptionIfPageModelIsNull()
        {
            HRCoreBordersService service = new HRCoreBordersService(
                new HRCoreBordersRepositoryStub(null, ""),
                new HRServiceWorkflowPaginationOnly<HRBorder>(null, null));
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.GetBordersAsync(null, null));
        }
    }
}

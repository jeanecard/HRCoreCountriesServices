using HRCommon;
using HRCommonModel;
using HRCoreBordersModel;
using HRCoreBordersServices;
using QuickType;
using System;
using System.Collections.Generic;
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
        public async void BorderService_On_GetBordersAsync_Throw_MemberAccessException_If_Repository_Is_Null()
        {
            HRCoreBordersService service = new HRCoreBordersService(
                null,
                new HRServiceWorkflowPaginationOnly<HRBorder>(null, null),
                null,
                null);
            await Assert.ThrowsAsync<MemberAccessException>(async () => await service.GetBordersAsync(new PagingParameterInModel(), null));

        }
        /// <summary>
        /// Test that MemberAccessException is thrown on GetBorderAsync with a null repository.
        /// </summary>
        [Fact]
        public async void BorderService_On_GetBorderAsync_Throw_MemberAccessException_If_Repository_Is_Null()
        {
            HRCoreBordersService service = new HRCoreBordersService(
                null,
                new HRServiceWorkflowPaginationOnly<HRBorder>(null, null),
                null,
                null);
            await Assert.ThrowsAsync<MemberAccessException>(async () => await service.GetBorderAsync("xx"));

        }
        /// <summary>
        /// Test that MemberAccessException is thrown on GetBordersAsync with a null Paginer.
        /// </summary>
        [Fact]
        public async void BorderService_On_GetBordersAsync_Throw_MemberAccessException_If_Paginer_Is_Null()
        {
            HRCoreBordersService service = new HRCoreBordersService(
                new HRCoreBordersRepositoryStub(null, ""),
                new HRServiceWorkflowPaginationOnly<HRBorder>(null, null),
                null,
                null);
            await Assert.ThrowsAsync<MemberAccessException>(async () => await service.GetBordersAsync(new PagingParameterInModel(), null));
        }
        /// <summary>
        /// Test that ArgumentNullException is thrown if pageModel is null.
        /// </summary>
        [Fact]
        public async void BorderService_On_GetBordersAsync_Throw_ArgumentNullException_If_PageModel_Is_Null()
        {
            HRCoreBordersService service = new HRCoreBordersService(
                new HRCoreBordersRepositoryStub(null, ""),
                new HRServiceWorkflowPaginationOnly<HRBorder>(null, null),
                null,
                null);
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.GetBordersAsync(null, null));
        }
    }
}

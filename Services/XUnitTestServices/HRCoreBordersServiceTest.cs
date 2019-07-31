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
        public async void BorderServiceOnGetBordersAsyncThrowMemberAccessExceptionIfRepositoryIsNull()
        {
            HRCoreBordersService service = new HRCoreBordersService(
                null,
                new HRServiceWorkflowPaginationOnly<HRBorder>(null, null),
                null);
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
                new HRServiceWorkflowPaginationOnly<HRBorder>(null, null),
                null);
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
                new HRServiceWorkflowPaginationOnly<HRBorder>(null, null),
                null);
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
                new HRServiceWorkflowPaginationOnly<HRBorder>(null, null),
                null);
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.GetBordersAsync(null, null));
        }
        /// <summary>
        /// Test that Service return all region but Empty and add "All".
        /// Implemented in "Gros bill mode"
        /// </summary>
        [Fact]
        public async void BorderServiceOnGetRegionReturnAllButEmptyPlusAll()
        {
            HRCoreBordersService service = new HRCoreBordersService(
            new HRCoreBordersRepositoryStub(null, ""),
            new HRServiceWorkflowPaginationOnly<HRBorder>(null, null),
            null);
            using(Task<IEnumerable<String>> serviceTask = service.GetContinentsAsync())
            {
                await serviceTask;
                int continentCount = 0;
                foreach(String iter in serviceTask.Result)
                {
                    Assert.NotEqual("Empty", iter);
                    continentCount++;
                }
                Assert.Equal(7, continentCount);

            }
            
        }
        /// <summary>
        /// Test that Service return String Empty with unexisting continentID
        /// </summary>
        [Fact]
        public async void BorderServiceOnGetRegionByIDReturnStringEmptyWithUnexistingID()
        {
            HRCoreBordersService service = new HRCoreBordersService(
                new HRCoreBordersRepositoryStub(null, ""),
                new HRServiceWorkflowPaginationOnly<HRBorder>(null, null),
                null);
            using (Task<String> serviceTask = service.GetContinentByIDAsync("Test"))
            {
                await serviceTask;
                Assert.Equal(String.Empty, serviceTask.Result);
            }
        }
        /// <summary>
        /// Test that Service return String Empty with ID = Empty
        /// </summary>
        [Fact]
        public async void BorderServiceOnGetRegionByIDReturnStringEmptyWithIDValueEmpty()
        {
            HRCoreBordersService service = new HRCoreBordersService(
                new HRCoreBordersRepositoryStub(null, ""),
                new HRServiceWorkflowPaginationOnly<HRBorder>(null, null),
                null);
            using (Task<String> serviceTask = service.GetContinentByIDAsync("Empty"))
            {
                await serviceTask;
                Assert.Equal(String.Empty, serviceTask.Result);
            }
        }
        /// <summary>
        /// Test that Service return String Empty with ID = All
        /// </summary>
        [Fact]
        public async void BorderServiceOnGetRegionByIDReturnAllWithIDValueAll()
        {
            HRCoreBordersService service = new HRCoreBordersService(
                new HRCoreBordersRepositoryStub(null, ""),
                new HRServiceWorkflowPaginationOnly<HRBorder>(null, null),
                null);
            using (Task<String> serviceTask = service.GetContinentByIDAsync("All"))
            {
                await serviceTask;
                Assert.Equal(HRCoreBordersService.ALL_CONTINENT_ID, serviceTask.Result);
            }
        }
        /// <summary>
        /// Test that Service return String Polar
        /// </summary>
        [Fact]
        public async void BorderServiceOnGetRegionByIDReturnPolarWithIDValuePolar()
        {
            HRCoreBordersService service = new HRCoreBordersService(
                new HRCoreBordersRepositoryStub(null, ""),
                new HRServiceWorkflowPaginationOnly<HRBorder>(null, null),
                null);
            using (Task<String> serviceTask = service.GetContinentByIDAsync("Polar"))
            {
                await serviceTask;
                Assert.Equal(Region.Polar.ToString(), serviceTask.Result);
            }
        }
        /// <summary>
        /// Test that Service return String String.Empty with pOlAr
        /// </summary>
        [Fact]
        public async void BorderServiceOnGetRegionByIDReturnStringEmptyWithIDValuepOlAr()
        {
            HRCoreBordersService service = new HRCoreBordersService(
               new HRCoreBordersRepositoryStub(null, ""),
                new HRServiceWorkflowPaginationOnly<HRBorder>(null, null),
                null);
            using (Task<String> serviceTask = service.GetContinentByIDAsync("pOlAr"))
            {
                await serviceTask;
                Assert.Equal(String.Empty, serviceTask.Result);
            }

        }

    }
}

using HRCommonModel;
using HRCommonModels;
using HRControllersForker;
using HRCoreBordersModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using TemporaryStubsToMoveInXUnitStubs;
using Xunit;

namespace XUnitTestControllers
{
    /// <summary>
    /// Test Class For HRBordersForkerTest
    /// </summary>
    public class HRBordersForkerTest
    {
        /// <summary>
        /// Test que GetFromPaging retourne un erreur interne 500 si le service fourni est null
        /// </summary>
        [Fact]
        public async void HRBorderForkerOnGetFromPagingWithNullServiceExpectStatus500InternalServerError()
        {
            PagingParameterInModel model = new PagingParameterInModel();
            CoreBordersServiceStub service = null;
            HRCommonForkerUtilsStub utilStub = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRBordersControllersForker forker = new HRBordersControllersForker(utilStub);

            using (Task<(int, PagingParameterOutModel<HRBorder>)> forkerTask = forker.GetFromPagingAsync(
                model,
                null,
                service,
                50
                ))
            {
                await forkerTask;
                Assert.True(forkerTask.Result.Item1 == StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// Test que GetFromPaging retourne un erreur interne 500 si le service retourne une exception autre que IndexOutOfRangeException
        /// </summary>
        [Fact]
        public async void HRBorderForkerOnGetFromPagingWithServiceThrowingExceptionExpectStatus500InternalServerError()
        {
            PagingParameterInModel model = new PagingParameterInModel();
            CoreBordersServiceStub service = new CoreBordersServiceStub(null) { ThrowException = true };
            service.ExceptionToThrow = new Exception();
            HRCommonForkerUtilsStub utilStub = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRBordersControllersForker forker = new HRBordersControllersForker(utilStub);

            using (Task<(int, PagingParameterOutModel<HRBorder>)> forkerTask = forker.GetFromPagingAsync(
                model,
                null,
                service,
                50
                ))
            {
                await forkerTask;
                Assert.True(forkerTask.Result.Item1 == StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// Test que GetFromPaging retourne un erreur Status416RequestedRangeNotSatisfiable si le service retourne une exception IndexOutOfRangeException
        /// </summary>
        [Fact]
        public async void HRBorderForkerOnGetFromPagingWithServiceThrowingIndexOutOfRangeExceptionExpectStatus416RequestedRangeNotSatisfiable()
        {
            PagingParameterInModel model = new PagingParameterInModel();
            CoreBordersServiceStub service = new CoreBordersServiceStub(null) { ThrowException = true };
            service.ExceptionToThrow = new IndexOutOfRangeException();
            HRCommonForkerUtilsStub utilStub = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRBordersControllersForker forker = new HRBordersControllersForker(utilStub);

            using (Task<(int, PagingParameterOutModel<HRBorder>)> forkerTask = forker.GetFromPagingAsync(
                model,
                null,
                service,
                50
                ))
            {
                await forkerTask;
                Assert.True(forkerTask.Result.Item1 == StatusCodes.Status416RequestedRangeNotSatisfiable);
            }

        }
        /// <summary>
        /// Test que GetFromPaging retourne un code 200 si le service retourne des données
        /// </summary>
        [Fact]
        public async void HRBorderForkerOnGetFromPagingWithServiceNormalResultExpectCode200()
        {
            PagingParameterInModel model = new PagingParameterInModel()
            {
                PageNumber = 0,
                PageSize = 10
            };
            CoreBordersServiceStub service = new CoreBordersServiceStub(new System.Collections.Generic.List<string>() { "XX" })
            {
                ThrowException = false
            };
            HRCommonForkerUtilsStub utilStub = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRBordersControllersForker forker = new HRBordersControllersForker(utilStub);
            using (Task<(int, PagingParameterOutModel<HRBorder>)> forkerTask = forker.GetFromPagingAsync(
                model,
                null,
                service,
                50
                ))
            {
                await forkerTask;
                Assert.True(forkerTask.Result.Item1 == StatusCodes.Status200OK);
            }
        }
        /// <summary>
        /// Test que GetFromPaging retourne un code Status413PayloadTooLarge si le pageSize du Model fourni est plus grand que maxPageSize
        /// </summary>
        [Fact]
        public async void HRBorderForkerOnGetFromPagingWithModelPageSizeGreaterThanMaxSizeExpectCodeStatus413PayloadTooLarge()
        {
            PagingParameterInModel model = new PagingParameterInModel()
            {
                PageNumber = 0,
                PageSize = 51
            };
            CoreBordersServiceStub service = new CoreBordersServiceStub(null)
            {
                ThrowException = true
            };
            service.ExceptionToThrow = new IndexOutOfRangeException();
            HRCommonForkerUtilsStub utilStub = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRBordersControllersForker forker = new HRBordersControllersForker(utilStub);
            using (Task<(int, PagingParameterOutModel<HRBorder>)> forkerTask = forker.GetFromPagingAsync(
                model,
                null,
                service,
                50
                ))
            { 
            await forkerTask;
            Assert.True(forkerTask.Result.Item1 == StatusCodes.Status413PayloadTooLarge);
            }
        }
        /// <summary>
        /// Test que GetFromPaging retourne un code 400 si le HRCommonForkerUtil.CanOrder est faux.
        /// </summary>
        [Fact]
        public async void HRBorderForkerOnGetFromPagingWithCanOrderReturnoingFalseExpectStatus400BadRequest()
        {
            PagingParameterInModel model = new PagingParameterInModel()
            {
                PageNumber = 0,
                PageSize = 50
            };
            HRSortingParamModel sort = new HRSortingParamModel()
            {
                OrderBy = "FIELD1;ASC"
            };
            CoreBordersServiceStub service = new CoreBordersServiceStub(null)
            {
                ThrowException = true,
                ExceptionToThrow = new IndexOutOfRangeException()
            };
            HRCommonForkerUtilsStub utilStub = new HRCommonForkerUtilsStub() { CanOrderReturn = false };
            HRBordersControllersForker forker = new HRBordersControllersForker(utilStub);
            using (Task<(int, PagingParameterOutModel<HRBorder>)> forkerTask = forker.GetFromPagingAsync(
                model,
                sort,
                service,
                50
                ))
            {
                await forkerTask;
                Assert.True(forkerTask.Result.Item1 == StatusCodes.Status400BadRequest);
            }
        }
    }
}

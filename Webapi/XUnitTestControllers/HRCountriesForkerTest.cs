using HRBordersAndCountriesWebAPI2.Utils;
using HRCommonModel;
using HRCommonModels;
using Microsoft.AspNetCore.Http;
using QuickType;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using XUnitTestControllers.MockAndStubs;

namespace XUnitTestControllers
{
    public class HRCountriesForkerTest
    {
        /// <summary>
        /// Test que GetFromPaging retourne un erreur interne 500 si le service fourni est null
        /// </summary>
        [Fact]
        public async void HRCountriesForkerOnGetFromPagingWithNullServiceExpectStatus500InternalServerError()
        {
            PagingParameterInModel model = new PagingParameterInModel();
            CoreCountriesServiceStub service = null;
            HRCommonForkerUtilsStub utilStub = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRCountriesControllersForker forker = new HRCountriesControllersForker(utilStub);

            using (Task<(int, PagingParameterOutModel<HRCountry>)> forkerTask = forker.GetFromPagingAsync(
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
        public async void HRCountriesOnGetFromPagingWithServiceThrowingExceptionExpectStatus500InternalServerError()
        {
            PagingParameterInModel model = new PagingParameterInModel();
            CoreCountriesServiceStub service = new CoreCountriesServiceStub(null) {
                ThrowException = true,
                ExceptionToThrow = new Exception()
            };
            HRCommonForkerUtilsStub utilStub = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRCountriesControllersForker forker = new HRCountriesControllersForker(utilStub);

            using (Task<(int, PagingParameterOutModel<HRCountry>)> forkerTask = forker.GetFromPagingAsync(
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
        public async void HRCountriesOnGetFromPagingWithServiceThrowingIndexOutOfRangeExceptionExpectStatus500InternalServerError()
        {
            PagingParameterInModel model = new PagingParameterInModel();
            CoreCountriesServiceStub service = new CoreCountriesServiceStub(null)
            {
                ThrowException = true,
                ExceptionToThrow = new IndexOutOfRangeException()
            };

            HRCommonForkerUtilsStub utilStub = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRCountriesControllersForker forker = new HRCountriesControllersForker(utilStub);

            using (Task<(int, PagingParameterOutModel<HRCountry>)> forkerTask = forker.GetFromPagingAsync(
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
        public async void HRCountriesOnGetFromPagingWithServiceNormalResultExpectCode200()
        {
            PagingParameterInModel model = new PagingParameterInModel()
            {
                PageNumber = 0,
                PageSize = 10
            };
            CoreCountriesServiceStub service = new CoreCountriesServiceStub(new System.Collections.Generic.List<string>() { "XX" })
            {
                ThrowException = false
            };
            HRCommonForkerUtilsStub utilStub = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRCountriesControllersForker forker = new HRCountriesControllersForker(utilStub);
            using (Task<(int, PagingParameterOutModel<HRCountry>)> forkerTask = forker.GetFromPagingAsync(
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
        public async void HRCountriesOnGetFromPagingWithModelPageSizeGreaterThanMaxSizeExpectCodeStatus413PayloadTooLarge()
        {
            PagingParameterInModel model = new PagingParameterInModel()
            {
                PageNumber = 0,
                PageSize = 51
            };
            CoreCountriesServiceStub service = new CoreCountriesServiceStub(null)
            {
                ThrowException = true
            };
            service.ExceptionToThrow = new IndexOutOfRangeException();
            HRCommonForkerUtilsStub utilStub = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRCountriesControllersForker forker = new HRCountriesControllersForker(utilStub);
            using (Task<(int, PagingParameterOutModel<HRCountry>)> forkerTask = forker.GetFromPagingAsync(
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
        public async void HRCountriesOnGetFromPagingWithCanOrderReturnoingFalseExpectStatus400BadRequest()
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
            CoreCountriesServiceStub service = new CoreCountriesServiceStub(null)
            {
                ThrowException = true,
                ExceptionToThrow = new IndexOutOfRangeException()
            };
            HRCommonForkerUtilsStub utilStub = new HRCommonForkerUtilsStub() { CanOrderReturn = false };
            HRCountriesControllersForker forker = new HRCountriesControllersForker(utilStub);
            using (Task<(int, PagingParameterOutModel<HRCountry>)> forkerTask = forker.GetFromPagingAsync(
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

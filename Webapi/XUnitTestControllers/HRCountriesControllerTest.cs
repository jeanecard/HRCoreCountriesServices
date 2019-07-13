using HRBordersAndCountriesWebAPI2.Utils;
using HRCommonModel;
using HRCoreCountriesWebAPI2.Controllers;
using Microsoft.AspNetCore.Http;
using QuickType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using XUnitTestControllers.MockAndStubs;

namespace XUnitTestControllers
{
    public class HRCountriesControllerTest
    {
        #region GetByID
        /// <summary>
        /// Test that GetByID with an unknow ID return status code 404 and a HRBorder null.
        /// </summary>
        [Fact]
        public async void HRCountriesControllerOnGetByIDUnknownExpectStatusCode404()
        {
            List<String> list = new List<String>() {
                "FR",
                "US" };
            IHRCommonForkerUtils util = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRCountriesControllersForker forker = new HRCountriesControllersForker(util);
            using (Task<(int, HRCountry)> resultService = forker.GetFromIDAsync("AA", new CoreCountriesServiceStub(list)))
            {
                await resultService;
                Assert.True(resultService.Result.Item1 == StatusCodes.Status404NotFound);
                Assert.True(resultService.Result.Item2 == null);
            }

        }
        /// <summary>
        /// Test that GetByID with a null ID return status code 400 and a HRBorder null.
        /// This case can not happen with the WebAPI microsoft as this query is processes by Get(PagingParam)
        /// </summary>
        [Fact]
        public async void HRCountriesControllerOnGetByIDNullExpectStatus400BadRequest()
        {
            List<String> list = new List<String>() {
                "FR",
                "US" };
            CoreCountriesServiceStub service = new CoreCountriesServiceStub(list);
            IHRCommonForkerUtils util = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRCountriesControllersForker forker = new HRCountriesControllersForker(util);

            using (Task<(int, HRCountry)> resultService = forker.GetFromIDAsync(null, service))
            {
                await resultService;
                Assert.True(resultService.Result.Item1 == StatusCodes.Status400BadRequest);
                Assert.True(resultService.Result.Item2 == null);
            }
        }
        /// <summary>
        /// Test that GetID return status code 500 if his _service is null (problem with DI)
        /// </summary>
        [Fact]
        public async void HRCountriesControllerOnGetByIDWithNullServiceExpectStatus500InternalServerError()
        {
            IHRCommonForkerUtils util = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRCountriesControllersForker forker = new HRCountriesControllersForker(util);

            using (Task<(int, HRCountry)> resultService = forker.GetFromIDAsync("xx", null))
            {
                await resultService;
                Assert.True(resultService.Result.Item1 == StatusCodes.Status500InternalServerError);
                Assert.True(resultService.Result.Item2 == null);
            }


        }
        /// <summary>
        /// Test that GetID return status code 500 and HRBorder Null in case of any exception raised by service
        /// </summary>
        [Fact]
        public async void HRCountriesControllerOnGetByIDWithExceptionThrownByServiceExpectStatus500InternalServerError()
        {
            List<String> list = new List<String>() {
                "FR",
                "US" };
            IHRCommonForkerUtils util = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRCountriesControllersForker forker = new HRCountriesControllersForker(util);

            using (Task<(int, HRCountry)> resultService = forker.GetFromIDAsync("xx", new CoreCountriesServiceStub(list) { ThrowException = true }))
            {
                await resultService;
                Assert.True(resultService.Result.Item1 == StatusCodes.Status500InternalServerError);
                Assert.True(resultService.Result.Item2 == null);
            }

        }

        /// <summary>
        /// Test that consistant call of GetByID return the Expected result HRBorder and a http status code of 200
        /// </summary>
        [Fact]
        public async void HRCountriesControllerOnGetByIDWithExistingItemExpectItemAndCodeStatus200()
        {
            List<String> list = new List<String>() {"FR",  "US" };
            IHRCommonForkerUtils util = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRCountriesControllersForker forker = new HRCountriesControllersForker(util);

            using (Task<(int, HRCountry)> resultService = forker.GetFromIDAsync("FR", new CoreCountriesServiceStub(list)))
            {
                await resultService;
                Assert.True(resultService.Result.Item1 == StatusCodes.Status200OK);
                Assert.True(resultService.Result.Item2 != null && resultService.Result.Item2.Alpha2Code.Equals("FR"));
            }
        }

        #endregion

        #region GetAll
        /// <summary>
        /// Test that an invalid PagingIn on GetAll return a http status 416 and a null result.
        /// </summary>
        [Fact]
        public async void HRCountriesControllerOnGetAllWithInvalidPagingInExpectStatus416RequestedRangeNotSatisfiable()
        {
            List<String> list = new List<String>() {
                "FR",
                "US" };

            PagingParameterInModel invalidModel = new PagingParameterInModel() { PageNumber = 2, PageSize = 50 };
            IHRCommonForkerUtils util = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRCountriesControllersForker forker = new HRCountriesControllersForker(util);

            using (Task<(int, PagingParameterOutModel<HRCountry>)> resultService = forker.GetFromPagingAsync(
                invalidModel,
                null,
                new CoreCountriesServiceStub(list) { ThrowException = true, ExceptionToThrow = new IndexOutOfRangeException() },
                50))
            {
                await resultService;
                Assert.True(resultService.Result.Item1 == StatusCodes.Status416RequestedRangeNotSatisfiable);
                Assert.True(resultService.Result.Item2 == null);
            }
        }

        /// <summary>
        /// Test that a call to GetAll with any exception raised by his service return a 500 http status code and a null result.
        /// </summary>
        [Fact]
        public async void HRCountriesControllerOnGetAllWithExceptionThrownByServiceExpectStatus500InternalServerError()
        {
            List<String> list = new List<String>() {
                "FR",
                "US" };

            PagingParameterInModel invalidModel = new PagingParameterInModel() { PageNumber = 2, PageSize = 50 };
            IHRCommonForkerUtils util = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRCountriesControllersForker forker = new HRCountriesControllersForker(util);

            using (Task<(int, PagingParameterOutModel<HRCountry>)> resultService = forker.GetFromPagingAsync(
                invalidModel,
                null,
                new CoreCountriesServiceStub(list) { ThrowException = true },
                50
                ))
            {
                await resultService;
                Assert.True(resultService.Result.Item1 == StatusCodes.Status500InternalServerError);
                Assert.True(resultService.Result.Item2 == null);
            }
        }
        /// <summary>
        /// Test normal condition Success and partial result returned.
        /// </summary>
        [Fact]
        public async void HRCountriesControllerOnGetAllWithValidPagingInExpectItemsAndCodeStatus200()
        {
            List<String> list = new List<String>() {
                "FR",
                "US" };
            PagingParameterInModel invalidModel = new PagingParameterInModel() { PageNumber = 1, PageSize = 50 };
            IHRCommonForkerUtils util = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRCountriesControllersForker forker = new HRCountriesControllersForker(util);

            using (Task<(int, PagingParameterOutModel<HRCountry>)> resultService = forker.GetFromPagingAsync(
                invalidModel,
                null,
                new CoreCountriesServiceStub(list),
                50
                ))
            {
                await resultService;
                Assert.True(resultService.Result.Item1 == StatusCodes.Status200OK);
            }
        }
        #endregion
    }
}

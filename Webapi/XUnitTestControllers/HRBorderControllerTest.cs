using HRBordersAndCountriesWebAPI2.Utils;
using HRCommonModel;
using HRCoreBordersModel;
using HRCoreCountriesWebAPI2.Controllers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using XUnitTestControllers.MockAndStubs;

namespace XUnitTestControllers
{
    public class HRBorderControllerTest
    {
        #region GetByID
        /// <summary>
        /// Test that GetByID with an unknow ID return status code 404 and a HRBorder null.
        /// </summary>
        [Fact]
        public async void HRBorderControllerOnGetByIDUnknownExpectStatusCode404()
        {
            List<String> list = new List<String>();
            HRCommonForkerUtilsStub forkerUtil = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRBordersControllersForker forker = new HRBordersControllersForker(forkerUtil);
            using (Task<(int, HRBorder)> resultService = forker.GetFromIDAsync("XX", new CoreBordersServiceStub(list)))
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
        public async void HRBorderControllerOnGetByIDNullExpectStatus400BadRequest()
        {
            List<String> list = new List<String>();
            HRCommonForkerUtilsStub forkerUtil = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRBordersControllersForker forker = new HRBordersControllersForker(forkerUtil);
            using (Task<(int, HRBorder)> resultService = forker.GetFromIDAsync(null, new CoreBordersServiceStub(list)))
            {
                await resultService;
                Assert.True(resultService.Result.Item1 == StatusCodes.Status400BadRequest);
                Assert.True(resultService.Result.Item2 == null);
            }
        }
        /// <summary>
        /// Test that GetID return status code 500 and his _service is null (problem with DI)
        /// </summary>
        [Fact]
        public async void HRBorderControllerOnGetByIDWithNullServiceExpectStatus500InternalServerError()
        {
            HRCommonForkerUtilsStub forkerUtil = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRBordersControllersForker forker = new HRBordersControllersForker(forkerUtil);
            using (Task<(int, HRBorder)> resultService = forker.GetFromIDAsync("XX", null))
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
        public async void HRBorderControllerOnGetByIDWithExceptionThrownByServiceExpectStatus500InternalServerError()
        {
            List<String> list = new List<String>() { ("XX") };
            HRCommonForkerUtilsStub forkerUtil = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRBordersControllersForker forker = new HRBordersControllersForker(forkerUtil);
            using (Task<(int, HRBorder)> resultService = forker.GetFromIDAsync("XX", new CoreBordersServiceStub(list) { ThrowException = true }))
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
        public async void HRBorderControllerOnGetByIDWithExistingItemExpectItemAndCodeStatus200()
        {
            List<String> list = new List<String>() { ("XX"), ("YY") };
            HRCommonForkerUtilsStub forkerUtil = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRBordersControllersForker forker = new HRBordersControllersForker(forkerUtil);
            using (Task<(int, HRBorder)> resultService = forker.GetFromIDAsync("XX", new CoreBordersServiceStub(list)))
            {
                await resultService;
                Assert.True(resultService.Result.Item1 == StatusCodes.Status200OK);
                Assert.True(resultService.Result.Item2 != null && resultService.Result.Item2.FIPS == "XX");
            }
        }
        #endregion

        #region GetAll

        /// <summary>
        /// Test that a call to GetAll with any exception raised by his service return a 500 http status code and a null result.
        /// </summary>
        [Fact]
        public async void HRBorderControllerOnGetAllWithExceptionThrownByServiceExpectStatus500InternalServerError()
        {
            List<String> list = new List<String>() { ("XX"), ("YY") };
            HRCommonForkerUtilsStub forkerUtil = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRBordersControllersForker forker = new HRBordersControllersForker(forkerUtil);
            using (Task<(int, PagingParameterOutModel<HRBorder>)> resultService = forker.GetFromPagingAsync(
                new PagingParameterInModel() { PageNumber = 0, PageSize = 50 },
                null,
                new CoreBordersServiceStub(list) { ThrowException = true },
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
        public async void HRBorderControllerOnGetAllWithValidPagingInExpectItemsAndCodeStatus200()
        {
            List<String> list = new List<String>();
            for (int i = 0; i < 300; i++)
            {
                list.Add(i.ToString());
            }
            CoreBordersServiceStub service = new CoreBordersServiceStub(list);
            PagingParameterInModel validModel = new PagingParameterInModel() { PageNumber = 1, PageSize = 50 };
            HRCommonForkerUtilsStub forkerUtil = new HRCommonForkerUtilsStub() { CanOrderReturn = true };
            HRBordersControllersForker forker = new HRBordersControllersForker(forkerUtil);
            using (Task<(int, PagingParameterOutModel<HRBorder>)> resultService = forker.GetFromPagingAsync(
                validModel,
                null,
                new CoreBordersServiceStub(list),
                50
                ))
            {
                await resultService;
                Assert.True(resultService.Result.Item1 == StatusCodes.Status200OK);
                Assert.True(resultService.Result.Item2 != null);
            }
        }
        #endregion
    }
}

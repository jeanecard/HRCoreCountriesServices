using HRCommonModel;
using HRCommonTools;
using HRCoreBordersModel;
using HRCoreCountriesWebAPI2.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;


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
            HRBordersController ctrl = new HRBordersController(new HRPaginer<HRBorder>(), null, new CoreBordersServiceStub(list));
            Task<(int, HRBorder)> resultService = ctrl.GetFromID("XX");
            await resultService;
            Assert.True(resultService.Result.Item1 == StatusCodes.Status404NotFound);
            Assert.True(resultService.Result.Item2 == null);

        }
        /// <summary>
        /// Test that GetByID with a null ID return status code 400 and a HRBorder null.
        /// This case can not happen with the WebAPI microsoft as this query is processes by Get(PagingParam)
        /// </summary>
        [Fact]
        public async void HRBorderControllerOnGetByIDNullExpectStatus400BadRequest()
        {
            List<String> list = new List<String>();
            HRBordersController ctrl = new HRBordersController(new HRPaginer<HRBorder>(), null, new CoreBordersServiceStub(list));
            Task<(int, HRBorder)> resultService = ctrl.GetFromID(null);
            await resultService;
            Assert.True(resultService.Result.Item1 == StatusCodes.Status400BadRequest);
            Assert.True(resultService.Result.Item2 == null);


        }
        /// <summary>
        /// Test that GetID return status code 500 and his _service is null (problem with DI)
        /// </summary>
        [Fact]
        public async void HRBorderControllerOnGetByIDWithNullServiceExpectStatus500InternalServerError()
        {
            HRBordersController ctrl = new HRBordersController(new HRPaginer<HRBorder>(), null, null);
            Task<(int, HRBorder)> resultService = ctrl.GetFromID("XX");
            await resultService;
            Assert.True(resultService.Result.Item1 == StatusCodes.Status500InternalServerError);
            Assert.True(resultService.Result.Item2 == null);


        }
        /// <summary>
        /// Test that GetID return status code 500 and HRBorder Null in case of any exception raised by service
        /// </summary>
        [Fact]
        public async void HRBorderControllerOnGetByIDWithExceptionThrownByServiceExpectStatus500InternalServerError()
        {
            List<String> list = new List<String>() { ("XX") };
            CoreBordersServiceStub service = new CoreBordersServiceStub(list) { ThrowException = true }; 
            HRBordersController ctrl = new HRBordersController(new HRPaginer<HRBorder>(), null, service);
            Task<(int, HRBorder)> resultService = ctrl.GetFromID("XX");
            await resultService;
            Assert.True(resultService.Result.Item1 == StatusCodes.Status500InternalServerError);
            Assert.True(resultService.Result.Item2 == null);

        }

        /// <summary>
        /// Test that consistant call of GetByID return the Expected result HRBorder and a http status code of 200
        /// </summary>
        [Fact]
        public async void HRBorderControllerOnGetByIDWithExistingItemExpectItemAndCodeStatus200()
        {
            List<String> list = new List<String>() { ("XX"), ("YY") };
            CoreBordersServiceStub service = new CoreBordersServiceStub(list);
            HRBordersController ctrl = new HRBordersController(new HRPaginer<HRBorder>(), null, service);
            Task<(int, HRBorder)> resultService = ctrl.GetFromID("XX");
            await resultService;
            Assert.True(resultService.Result.Item1 == StatusCodes.Status200OK);
            Assert.True(resultService.Result.Item2 != null && resultService.Result.Item2.FIPS == "XX");
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Test that an invalid PagingIn on GetAll return a http status 416 and a null result.
        /// </summary>
        [Fact]
        public async void HRBorderControllerOnGetAllWithInvalidPagingInExpectStatus416RequestedRangeNotSatisfiable()
        {
            List<String> list = new List<String>() { ("XX"), ("YY") };
            CoreBordersServiceStub service = new CoreBordersServiceStub(list);
            HRBordersController ctrl = new HRBordersController(new HRPaginer<HRBorder>(), null, service);
            PagingParameterInModel invalidModel = new PagingParameterInModel() { PageNumber = 2, PageSize = 100 };
            Task<(int, PagingParameterOutModel<HRBorder>)> resultService = ctrl.GetFromPaging(invalidModel, null);
            await resultService;
            Assert.True(resultService.Result.Item1 == StatusCodes.Status416RequestedRangeNotSatisfiable);
            Assert.True(resultService.Result.Item2 == null);

        }

        /// <summary>
        /// Test that a call to GetAll with any exception raised by his service return a 500 http status code and a null result.
        /// </summary>
        [Fact]
        public async void HRBorderControllerOnGetAllWithExceptionThrownByServiceExpectStatus500InternalServerError()
        {
            List<String> list = new List<String>() { ("XX"), ("YY") };
            CoreBordersServiceStub service = new CoreBordersServiceStub(list) { ThrowException = true };          
            HRBordersController ctrl = new HRBordersController(new HRPaginer<HRBorder>(), null, service);
            PagingParameterInModel validModel = new PagingParameterInModel() { PageNumber = 0, PageSize = 50 };
            Task<(int, PagingParameterOutModel<HRBorder>)> resultService = ctrl.GetFromPaging(validModel, null);
            await resultService;
            Assert.True(resultService.Result.Item1 == StatusCodes.Status500InternalServerError);
            Assert.True(resultService.Result.Item2 == null);

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
            HRBordersController ctrl = new HRBordersController(new HRPaginer<HRBorder>(), null, service);
            PagingParameterInModel validModel = new PagingParameterInModel() { PageNumber = 1, PageSize = 100 };
            Task<(int, PagingParameterOutModel<HRBorder>)> resultService = ctrl.GetFromPaging(validModel, null);
            await resultService;
            Assert.True(resultService.Result.Item1 == StatusCodes.Status200OK);
            Assert.True(resultService.Result.Item2 != null);
            Assert.True(resultService.Result.Item2.HasNextPage);
            Assert.True(resultService.Result.Item2.HasPreviousPage);
            Assert.True(resultService.Result.Item2.TotalItemsCount == 300);
            int j = 100;
            foreach (HRBorder iterator in resultService.Result.Item2.PageItems)
            {
                Assert.True(iterator.FIPS == j.ToString());
                j++;
            }
            Assert.True(j == 150);
        }
        /// <summary>
        /// Test normal condition Success and partial result returned in the maxlimit pageSize.
        /// </summary>
        [Fact]
        public async void HRBorderControllerOnGetAllWithValidPagingInExpectItemsChunkedTo50MaxPageSize()
        {
            List<String> list = new List<String>();
            for (int i = 0; i < 300; i++)
            {
                list.Add(i.ToString());
            }
            CoreBordersServiceStub service = new CoreBordersServiceStub(list);
            HRBordersController ctrl = new HRBordersController(new HRPaginer<HRBorder>(), null, service);
            PagingParameterInModel validModel = new PagingParameterInModel() { PageNumber = 1, PageSize = 100 };
            Task<ActionResult<PagingParameterOutModel<HRBorder>>> resultService = ctrl.Get(validModel, null);
            await resultService;
            Assert.True(resultService.Result.Value != null);
            Assert.True(resultService.Result.Value.HasNextPage);
            Assert.True(resultService.Result.Value.HasPreviousPage);//in first version reset to 0
            Assert.True(resultService.Result.Value.TotalItemsCount == 300);
            int j = 100;
            foreach (HRBorder iterator in resultService.Result.Value.PageItems)
            {
                Assert.True(iterator.FIPS == j.ToString());
                j++;
            }
            Assert.True(j == 150);
        }

        #endregion
    }
}

using HRCommonModel;
using HRCoreBordersModel;
using HRCoreCountriesWebAPI2.Controllers;
using Microsoft.AspNetCore.Http;
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
            HRBordersController ctrl = new HRBordersController(null, new CoreBordersServiceStub(list));
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
            HRBordersController ctrl = new HRBordersController(null, new CoreBordersServiceStub(list));
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
            HRBordersController ctrl = new HRBordersController(null, null);
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
            HRBordersController ctrl = new HRBordersController(null, service);
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
            HRBordersController ctrl = new HRBordersController(null, service);
            Task<(int, HRBorder)> resultService = ctrl.GetFromID("XX");
            await resultService;
            Assert.True(resultService.Result.Item1 == StatusCodes.Status200OK);
            Assert.True(resultService.Result.Item2 != null && resultService.Result.Item2.FIPS == "XX");
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
            CoreBordersServiceStub service = new CoreBordersServiceStub(list) { ThrowException = true };
            HRBordersController ctrl = new HRBordersController(null, service);
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
            HRBordersController ctrl = new HRBordersController(null, service);
            PagingParameterInModel validModel = new PagingParameterInModel() { PageNumber = 1, PageSize = 50 };
            Task<(int, PagingParameterOutModel<HRBorder>)> resultService = ctrl.GetFromPaging(validModel, null);
            await resultService;
            Assert.True(resultService.Result.Item1 == StatusCodes.Status200OK);
            Assert.True(resultService.Result.Item2 != null);

        }
        #endregion
    }
}

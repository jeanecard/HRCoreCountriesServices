using HRCommonModel;
using HRCoreCountriesWebAPI2.Controllers;
using Microsoft.AspNetCore.Http;
using QuickType;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;


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
            List<MongoDB.Bson.ObjectId> list = new List<MongoDB.Bson.ObjectId>() {
                new MongoDB.Bson.ObjectId("507f191e810c19729de860ea"),
                new MongoDB.Bson.ObjectId("507f191e810c19729de860eb") };
            CoreCountriesServiceStub service = new CoreCountriesServiceStub(list);
            HRCountriesController ctrl = new HRCountriesController(service, null);

            Task<(int, HRCountry)> resultService = ctrl.GetFromID("507f191e810c19729de860ec");
            await resultService;
            Assert.True(resultService.Result.Item1 == StatusCodes.Status404NotFound);
            Assert.True(resultService.Result.Item2 == null);

        }
        /// <summary>
        /// Test that GetByID with a null ID return status code 400 and a HRBorder null.
        /// This case can not happen with the WebAPI microsoft as this query is processes by Get(PagingParam)
        /// </summary>
        [Fact]
        public async void HRCountriesControllerOnGetByIDNullExpectStatus400BadRequest()
        {
            List<MongoDB.Bson.ObjectId> list = new List<MongoDB.Bson.ObjectId>() {
                new MongoDB.Bson.ObjectId("507f191e810c19729de860ea"),
                new MongoDB.Bson.ObjectId("507f191e810c19729de860eb") };
            CoreCountriesServiceStub service = new CoreCountriesServiceStub(list);
            HRCountriesController ctrl = new HRCountriesController(service, null);
            Task<(int, HRCountry)> resultService = ctrl.GetFromID(null);
            await resultService;
            Assert.True(resultService.Result.Item1 == StatusCodes.Status400BadRequest);
            Assert.True(resultService.Result.Item2 == null);


        }
        /// <summary>
        /// Test that GetID return status code 500 if his _service is null (problem with DI)
        /// </summary>
        [Fact]
        public async void HRCountriesControllerOnGetByIDWithNullServiceExpectStatus500InternalServerError()
        {
            HRCountriesController ctrl = new HRCountriesController(null, null);
            Task<(int, HRCountry)> resultService = ctrl.GetFromID("507f191e810c19729de860ea");
            await resultService;
            Assert.True(resultService.Result.Item1 == StatusCodes.Status500InternalServerError);
            Assert.True(resultService.Result.Item2 == null);


        }
        /// <summary>
        /// Test that GetID return status code 500 if his _service is null (problem with DI)
        /// </summary>
        [Fact]
        public async void HRCountriesControllerOnGetByIDWithIDNotBsonIDExpectStatus500InternalServerError()
        {
            List<MongoDB.Bson.ObjectId> list = new List<MongoDB.Bson.ObjectId>() {
                new MongoDB.Bson.ObjectId("507f191e810c19729de860ea"),
                new MongoDB.Bson.ObjectId("507f191e810c19729de860eb") };
            CoreCountriesServiceStub service = new CoreCountriesServiceStub(list);
            HRCountriesController ctrl = new HRCountriesController(service, null);
            Task<(int, HRCountry)> resultService = ctrl.GetFromID("507a");
            await resultService;
            Assert.True(resultService.Result.Item1 == StatusCodes.Status500InternalServerError);
            Assert.True(resultService.Result.Item2 == null);
        }
        /// <summary>
        /// Test that GetID return status code 500 and HRBorder Null in case of any exception raised by service
        /// </summary>
        [Fact]
        public async void HRCountriesControllerOnGetByIDWithExceptionThrownByServiceExpectStatus500InternalServerError()
        {
            List<MongoDB.Bson.ObjectId> list = new List<MongoDB.Bson.ObjectId>() {
                new MongoDB.Bson.ObjectId("507f191e810c19729de860ea"),
                new MongoDB.Bson.ObjectId("507f191e810c19729de860eb") };
            CoreCountriesServiceStub service = new CoreCountriesServiceStub(list);
            HRCountriesController ctrl = new HRCountriesController(service, null);
            service.ThrowException = true;
            Task<(int, HRCountry)> resultService = ctrl.GetFromID("507f191e810c19729de860ea");
            await resultService;
            Assert.True(resultService.Result.Item1 == StatusCodes.Status500InternalServerError);
            Assert.True(resultService.Result.Item2 == null);

        }

        /// <summary>
        /// Test that consistant call of GetByID return the Expected result HRBorder and a http status code of 200
        /// </summary>
        [Fact]
        public async void HRCountriesControllerOnGetByIDWithExistingItemExpectItemAndCodeStatus200()
        {
            List<MongoDB.Bson.ObjectId> list = new List<MongoDB.Bson.ObjectId>() {
                new MongoDB.Bson.ObjectId("507f191e810c19729de860ea"),
                new MongoDB.Bson.ObjectId("507f191e810c19729de860eb") };
            CoreCountriesServiceStub service = new CoreCountriesServiceStub(list);
            HRCountriesController ctrl = new HRCountriesController(service, null);
            Task<(int, HRCountry)> resultService = ctrl.GetFromID("507f191e810c19729de860ea");
            await resultService;
            Assert.True(resultService.Result.Item1 == StatusCodes.Status200OK);
            Assert.True(resultService.Result.Item2 != null && resultService.Result.Item2._id.Equals(new MongoDB.Bson.ObjectId("507f191e810c19729de860ea")));
        }

        #endregion

        #region GetAll
        /// <summary>
        /// Test that an invalid PagingIn on GetAll return a http status 416 and a null result.
        /// </summary>
        [Fact]
        public async void HRCountriesControllerOnGetAllWithInvalidPagingInExpectStatus416RequestedRangeNotSatisfiable()
        {
            List<MongoDB.Bson.ObjectId> list = new List<MongoDB.Bson.ObjectId>() {
                new MongoDB.Bson.ObjectId("507f191e810c19729de860ea"),
                new MongoDB.Bson.ObjectId("507f191e810c19729de860eb") };
            CoreCountriesServiceStub service = new CoreCountriesServiceStub(list);
            HRCountriesController ctrl = new HRCountriesController(service, null);
            PagingParameterInModel invalidModel = new PagingParameterInModel() { PageNumber = 2, PageSize = 100 };
            Task<(int, PagingParameterOutModel<HRCountry>)> resultService = ctrl.GetFromPaging(invalidModel, null);
            await resultService;
            Assert.True(resultService.Result.Item1 == StatusCodes.Status416RequestedRangeNotSatisfiable);
            Assert.True(resultService.Result.Item2 == null);

        }

        /// <summary>
        /// Test that a call to GetAll with any exception raised by his service return a 500 http status code and a null result.
        /// </summary>
        [Fact]
        public async void HRCountriesControllerOnGetAllWithExceptionThrownByServiceExpectStatus500InternalServerError()
        {
            List<MongoDB.Bson.ObjectId> list = new List<MongoDB.Bson.ObjectId>() {
                new MongoDB.Bson.ObjectId("507f191e810c19729de860ea"),
                new MongoDB.Bson.ObjectId("507f191e810c19729de860eb") };
            CoreCountriesServiceStub service = new CoreCountriesServiceStub(list) { ThrowException = true };
            HRCountriesController ctrl = new HRCountriesController(service, null);
            PagingParameterInModel invalidModel = new PagingParameterInModel() { PageNumber = 2, PageSize = 100 };
            Task<(int, PagingParameterOutModel<HRCountry>)> resultService = ctrl.GetFromPaging(invalidModel, null);
            await resultService;
            Assert.True(resultService.Result.Item1 == StatusCodes.Status500InternalServerError);
            Assert.True(resultService.Result.Item2 == null);

        }
        /// <summary>
        /// Test normal condition Success and partial result returned.
        /// </summary>
        [Fact]
        public async void HRCountriesControllerOnGetAllWithValidPagingInExpectItemsAndCodeStatus200()
        {
            List<MongoDB.Bson.ObjectId> list = new List<MongoDB.Bson.ObjectId>();
            for (int i = 0; i < 300; i++)
            {
                list.Add(new MongoDB.Bson.ObjectId("507f191e810c19729de860ea"));
            }
            CoreCountriesServiceStub service = new CoreCountriesServiceStub(list);
            HRCountriesController ctrl = new HRCountriesController(service, null);
            PagingParameterInModel invalidModel = new PagingParameterInModel() { PageNumber = 1, PageSize = 100 };
            Task<(int, PagingParameterOutModel<HRCountry>)> resultService = ctrl.GetFromPaging(invalidModel, null);
            await resultService;
            Assert.True(resultService.Result.Item1 == StatusCodes.Status200OK);
            Assert.True(resultService.Result.Item2 != null);
            Assert.True(resultService.Result.Item2.HasNextPage);
            Assert.True(resultService.Result.Item2.HasPreviousPage);
            Assert.True(resultService.Result.Item2.TotalItemsCount == 300);
            int j = 100;
            foreach (HRCountry iterator in resultService.Result.Item2.PageItems)
            {
                j++;
            }
            Assert.True(j == 200);
        }
        #endregion
    }
}

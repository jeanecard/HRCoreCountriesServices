using HRControllersForker;
using Microsoft.AspNetCore.Http;
using QuickType;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemporaryStubsToMoveInXUnitStubs;
using Xunit;

namespace XUnitTestControllers
{
    public class HRCountriesByContinentControllerForkerTest
    {
        /// <summary>
        /// Test that GetHRCountriesByContinentAsync with null service Return 500 error
        /// </summary>
        [Fact]
        public async void GetHRCountriesByContinentAsync_With_Null_Service_Return_Error500()
        {
            HRCountriesByContinentControllerForker forker = new HRCountriesByContinentControllerForker();
            using (Task<(int, IEnumerable<HRCountry>)> task = forker.GetHRCountriesByContinentAsync(null, "Africa"))
            {
                await task;
                Assert.True(task.Result.Item1 == StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// Test that GetHRCountriesByContinentAsync with non convertible Region return Error 400.
        /// </summary>
        [Fact]
        public async void GetHRCountriesByContinentAsync_With_Null_Non_Convertible_Region_Return_Error400()
        {
            HRCountriesByContinentControllerForker forker = new HRCountriesByContinentControllerForker();
            CoreCountriesServiceStub serviceStub = new CoreCountriesServiceStub(null);
            using (Task<(int, IEnumerable<HRCountry>)> task = forker.GetHRCountriesByContinentAsync(serviceStub, "NonConvertibleRegion"))
            {
                await task;
                Assert.True(task.Result.Item1 == StatusCodes.Status400BadRequest);
            }
        }
        /// <summary>
        /// Test that GetHRCountriesByContinentAsync with non convertible Region return Error 400.
        /// </summary>
        [Fact]
        public async void GetHRCountriesByContinentAsync_With_Convertible_Region_Return_Status_200()
        {
            HRCountriesByContinentControllerForker forker = new HRCountriesByContinentControllerForker();
            CoreCountriesServiceStub serviceStub = new CoreCountriesServiceStub(new List<string>() { "1"});
            using (Task<(int, IEnumerable<HRCountry>)> task = forker.GetHRCountriesByContinentAsync(serviceStub, "Africa"))
            {
                await task;
                Assert.True(task.Result.Item1 == StatusCodes.Status200OK);
                Assert.NotNull(task.Result.Item2);
                Assert.NotEmpty(task.Result.Item2.ToList());
                Assert.True(task.Result.Item2.ToList().Count == 1);
                Assert.True(task.Result.Item2.ToList()[0].Alpha3Code == "1");


            }
        }

    }
}

using HRControllersForker;
using Microsoft.AspNetCore.Http;
using QuickType;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemporaryStubsToMoveInXUnitStubs;
using Xunit;

namespace XUnitTestControllers
{
    /// <summary>
    /// TODO
    /// </summary>
    public class HRLangagesByContinentControllerForkerTest
    {
        /// <summary>
        /// Test that GetLangagesByContinentAsync with null service Return 500 error
        /// </summary>
        [Fact]
        public async void GetLangagesByContinentAsync_With_Null_Service_Return_Error500()
        {
            HRLangagesByContinentControllerForker forker = new HRLangagesByContinentControllerForker();
            using (Task<(int, IEnumerable<Language>)> task = forker.GetLangagesByContinentAsync(null, "Africa"))
            {
                await task;
                Assert.True(task.Result.Item1 == StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// Test that GetLangagesByContinentAsync with non convertible Region return Error 400.
        /// </summary>
        [Fact]
        public async void GetLangagesByContinentAsync_With_Null_Non_Convertible_Region_Return_Error400()
        {
            HRLangagesByContinentControllerForker forker = new HRLangagesByContinentControllerForker();
            CoreCountriesServiceStub serviceStub = new CoreCountriesServiceStub(null);
            using (Task<(int, IEnumerable<Language>)> task = forker.GetLangagesByContinentAsync(serviceStub, "NonConvertibleRegion"))
            {
                await task;
                Assert.True(task.Result.Item1 == StatusCodes.Status400BadRequest);
            }
        }
        /// <summary>
        /// Test that GetLangagesByContinentAsync with convertible Region return Status 200
        /// </summary>
        [Fact]
        public async void GetLangagesByContinentAsync_With_Convertible_Region_Return_Status_200()
        {
            HRLangagesByContinentControllerForker forker = new HRLangagesByContinentControllerForker();
            CoreCountriesServiceStub serviceStub = new CoreCountriesServiceStub(null);
            using (Task<(int, IEnumerable<Language>)> task = forker.GetLangagesByContinentAsync(serviceStub, "Africa"))
            {
                await task;
                Assert.True(task.Result.Item1 == StatusCodes.Status200OK);
            }
        }

    }
}

using HRCoreCountriesRepository;
using QuickType;
using System;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestDAL
{
    public class HRCoreCountriesRepositoryTest
    {
        /// <summary>
        /// Test that GetAsync return null when id is null or Empty
        /// </summary>
        [Fact]
        public async void TestGetAsyncReturnNullWithIDNullOrEmpty()
        {
            MongoDBCountriesRepository repo = new MongoDBCountriesRepository(null);
            Task<HRCountry> task = repo.GetAsync(null);
            await task;
            Assert.Null(task.Result);
            task = repo.GetAsync(String.Empty);
            await task;
            Assert.Null(task.Result);
        }
    }
}

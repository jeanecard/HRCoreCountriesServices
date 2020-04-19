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
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async void MongoDBCountriesRepository_GetAsync_With_ID_Null_Or_Empty_Expect_Return_Null(String idCountry)
        {
            MongoDBCountriesRepository repo = new MongoDBCountriesRepository(null, null);
            Task<HRCountry> task = repo.GetAsync(idCountry);
            await task;
            Assert.Null(task.Result);
        }
    }
}

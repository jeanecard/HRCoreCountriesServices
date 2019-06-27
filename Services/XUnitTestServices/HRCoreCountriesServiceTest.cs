using HRCoreBordersModel;
using HRCoreBordersServices;
using HRCoreCountriesServices;
using QuickType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using XUnitTestServices.MocksAndStubs;

namespace XUnitTestServices
{
    public class HRCoreCountriesServiceTest
    {
        /// <summary>
        /// Test that MemberAccessException is thrown on GetCountriessAsync with a null repoistory.
        /// </summary>
        [Fact]
        public async void CountriesServiceOnGetCountriesAsyncThrowMemberAccessExceptionIfRepositoryIsNullExpectTrue()
        {
            //CoreCountriesService service = new CoreCountriesService(null);
            //bool exceptionThrown = false;
            //try
            //{
            //    Task<IEnumerable<HRCountry>>  retour = service.GetCountriesAsync("xx");
            //    await retour;
            //}
            //catch(MemberAccessException)
            //{
            //    exceptionThrown = true;
            //}
            //catch(Exception)
            //{
            //    //Dummy.
            //}
            //Assert.True(exceptionThrown);
        }
        /// <summary>
        /// Verify that Service do not do any extra processing on repository results.
        /// </summary>
        [Fact]
        public async void CountriesServiceOnGetCountriesAsyncWithCountryIDReturnRepositoryResultWithoutExtraProcessingwExpectTrue()
        {
            //HRCoreCountriesRepositoryStub repository = new HRCoreCountriesRepositoryStub(null, "507f191e810c19729de860ea");
            //CoreCountriesService service = new CoreCountriesService(repository);
            //Task<IEnumerable<HRCountry>> repoResult = service.GetCountriesAsync("507f191e810c19729de860ea");
            //await repoResult;
            //Assert.NotNull(repoResult.Result);
            //IEnumerator<HRCountry> enumerator = repoResult.Result.GetEnumerator();
            //enumerator.MoveNext();
            //Assert.NotNull(enumerator.Current);
            //Assert.True(enumerator.Current._id.Equals(new MongoDB.Bson.ObjectId("507f191e810c19729de860ea")));
        }

        /// <summary>
        /// Verify that Service do not do any extra processing on repository results.
        /// </summary>
        [Fact]
        public async void CountriesServiceOnGetBCountriesAsyncWithoutCountryIDReturnRepositoryResultWithoutExtraProcessingwExpectTrue()
        {
            //List<String> countriesID = new List<string>() { "507f191e810c19729de860ea", "507f191e810c19729de860eb", "507f191e810c19729de860ec" };
            //HRCoreCountriesRepositoryStub repository = new HRCoreCountriesRepositoryStub(countriesID, null);
            //CoreCountriesService service = new CoreCountriesService(repository);
            //Task<IEnumerable<HRCountry>> repoResult = service.GetCountriesAsync(null);
            //await repoResult;
            //Assert.NotNull(repoResult.Result);
            //IEnumerator<HRCountry> enumerator = repoResult.Result.GetEnumerator();
            //int i = 0;
            //while (enumerator.MoveNext())
            //{
            //    Assert.NotNull(enumerator.Current);
            //    MongoDB.Bson.ObjectId idi = new MongoDB.Bson.ObjectId();
            //    if (i == 0)
            //    {
            //        idi = new MongoDB.Bson.ObjectId("507f191e810c19729de860ea");
            //    }
            //    else if (i == 1)
            //    {
            //        idi = new MongoDB.Bson.ObjectId("507f191e810c19729de860eb");
            //    }
            //    else if( i == 2)
            //    {
            //        idi = new MongoDB.Bson.ObjectId("507f191e810c19729de860ec");
            //    }
            //    Assert.True(enumerator.Current._id.Equals(idi ));
            //    i++;
            //}
            //Assert.True(i == 3);
        }
    }
}

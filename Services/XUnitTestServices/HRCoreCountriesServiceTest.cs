using HRCommon;
using HRCommon.Interface;
using HRCommonModel;
using HRCommonModels;
using HRCommonTools;
using HRCoreCountriesServices;
using QuickType;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using XUnitTestServices.MocksAndStubs;

namespace XUnitTestServices
{
    public class HRCoreCountriesServiceTest
    {
        /// <summary>
        /// Test that MemberAccessException is thrown on GetCountriessAsync with a null repoistory. Not really a UT for HRCoreCountriesService but for Workflow.
        /// Keep it there while no rework is needed.
        /// </summary>
        [Fact]
        public async void CountriesService_On_GetCountriesAsync_Throw_MemberAccessException_If_CountryRepository_Is_Null_ExpectTrue()
        {
            IServiceWorkflowOnHRCoreRepository<HRCountry> workflow = new HRServiceWorkflowPaginationOnly<HRCountry>(null, null);
            CoreCountriesService service = new CoreCountriesService(null, new LanguageRepositoryStub(),  workflow, null);
            bool exceptionThrown = false;
            try
            {
                PagingParameterInModel pageModel = new PagingParameterInModel();
                HRSortingParamModel orderBy = new HRSortingParamModel();
                using (Task<PagingParameterOutModel<HRCountry>> retour = service.GetCountriesAsync(pageModel, orderBy))
                {
                    await retour;
                }
            }
            catch (MemberAccessException)
            {
                exceptionThrown = true;
            }
            catch (Exception)
            {
                //Dummy.
            }
            Assert.True(exceptionThrown);
        }

        /// <summary>
        /// Test that MemberAccessException is thrown on GetCountriessAsync with a null workflow. 
        /// </summary>
        [Fact]
        public async void CountriesServiceOnGetCountriesAsyncThrowMemberAccessExceptionIfWorkflowIsNullExpectTrue()
        {
            CoreCountriesService service = new CoreCountriesService(
                new HRCoreCountriesRepositoryStub(null, null), 
                new LanguageRepositoryStub(), 
                null, 
                null);
            bool exceptionThrown = false;
            try
            {
                PagingParameterInModel pageModel = new PagingParameterInModel();
                HRSortingParamModel orderBy = new HRSortingParamModel();
                using (Task<PagingParameterOutModel<HRCountry>> retour = service.GetCountriesAsync(pageModel, orderBy))
                {
                    await retour;
                }
            }
            catch (MemberAccessException)
            {
                exceptionThrown = true;
            }
            catch (Exception)
            {
                //Dummy.
            }
            Assert.True(exceptionThrown);
        }
        /// <summary>
        /// Verify that Service do not do any extra processing on repository results.
        /// </summary>
        [Fact]
        public async void CountriesServiceOnGetCountriesAsyncWithCountryIDReturnRepositoryResultWithoutExtraProcessingwExpectTrue()
        {
            HRCoreCountriesRepositoryStub repository = new HRCoreCountriesRepositoryStub(new List<string>() { "aa"}, "aa");
            CoreCountriesService service = new CoreCountriesService(
                repository,
                new LanguageRepositoryStub(),
                new HRServiceWorkflowPaginationOnly<HRCountry>(
                    repository,
                    new HRPaginer<HRCountry>()),
                null
                );
            using (Task<HRCountry> repoResult = service.GetCountryAsync("aa"))
            { 
                await repoResult;
                Assert.NotNull(repoResult.Result);
                Assert.Equal("aa", repoResult.Result.Alpha2Code);
            }
        }

        /// <summary>
        /// Verify that Service do not do any extra processing on repository results.
        /// </summary>
        [Fact]
        public async void CountriesServiceOnGetBCountriesAsyncWithoutCountryIDReturnRepositoryResultWithoutExtraProcessingwExpectTrue()
        {
            HRCoreCountriesRepositoryStub repository = new HRCoreCountriesRepositoryStub(new List<string>() { "aa", "bb", "cc" }, "aa");
            CoreCountriesService service = new CoreCountriesService(
                repository,
                new LanguageRepositoryStub(),
                new HRServiceWorkflowPaginationOnly<HRCountry>(
                    repository,
                    new HRPaginer<HRCountry>()),
                null
                );
            PagingParameterInModel pageModel = new PagingParameterInModel() { PageNumber = 0, PageSize = 10 };

            using (Task<PagingParameterOutModel<HRCountry>> repoResult = service.GetCountriesAsync(pageModel, null))
            {
                await repoResult;
                Assert.NotNull(repoResult.Result);
                IEnumerator<HRCountry> enumerator = repoResult.Result.PageItems.GetEnumerator();
                int i = 0;
                String id = String.Empty;
                while (enumerator.MoveNext())
                {
                    Assert.NotNull(enumerator.Current);
                    if (i == 0)
                    {
                        id = "aa";
                    }
                    else if (i == 1)
                    {
                        id = "bb";
                    }
                    else if (i == 2)
                    {
                        id = "cc";
                    }
                    Assert.True(enumerator.Current.Alpha2Code == id);
                    i++;
                }
                Assert.True(i == 3);
            }
        }

        /// <summary>
        /// Test that GetContinentByID return null with an unknown ID
        /// </summary>
        [Fact]
        public void CountriesService_On_Get_Continent_By_ID_With_Unknown_ID_Return_Null()
        {
            IServiceWorkflowOnHRCoreRepository<HRCountry> workflow = new HRServiceWorkflowPaginationOnly<HRCountry>(null, null);
            CoreCountriesService service = new CoreCountriesService(null, new LanguageRepositoryStub(), workflow, null);
            Assert.True(String.IsNullOrEmpty(service.GetContinentByID("HR")));
        }
        /// <summary>
        /// Test that GetContinentByID return Africa with Africa ID (case insensitive)
        /// </summary>
        [Fact]
        public void CountriesService_On_GetContinentByID_With_AfricaID_No_Matching_Case_Return_Africa()
        {
            IServiceWorkflowOnHRCoreRepository<HRCountry> workflow = new HRServiceWorkflowPaginationOnly<HRCountry>(null, null);
            CoreCountriesService service = new CoreCountriesService(null, new LanguageRepositoryStub(), workflow, null);
            Assert.Equal(Region.Africa.ToString(), service.GetContinentByID("AFrICa"));
        }

        /// <summary>
        /// Test that GetContinents return All Continents (Checksum)
        /// </summary>
        [Fact]
        public void CountriesService_On_GetContinents_Return_7_Items()
        {
            IServiceWorkflowOnHRCoreRepository<HRCountry> workflow = new HRServiceWorkflowPaginationOnly<HRCountry>(null, null);
            CoreCountriesService service = new CoreCountriesService(null, new LanguageRepositoryStub(), workflow, null);
            Assert.Equal(7, service.GetContinents().ToList().Count);
        }
        /// <summary>
        /// TODO
        /// </summary>
        [Fact]
        public async void GetHRLangagesByContinentAsync_With_Null_Langage_Repository_Throw_MemberAccessException()
        {
            IServiceWorkflowOnHRCoreRepository<HRCountry> workflow = new HRServiceWorkflowPaginationOnly<HRCountry>(null, null);
            HRCoreCountriesRepositoryStub repository = new HRCoreCountriesRepositoryStub(new List<string>() { "aa" }, "aa");
            CoreCountriesService coreService = new CoreCountriesService(repository, null, null, null);
            Region region = Region.Africa;
            await Assert.ThrowsAsync<MemberAccessException>(async () => await coreService.GetHRLangagesByContinentAsync(region));
        }

        /// <summary>
        /// TODO
        /// </summary>
        [Fact]
        public async void GetHRLangagesByContinentAsync_With_Stub_Langage_Repository_Return_Langage_Without_Alteration()
        {
            IServiceWorkflowOnHRCoreRepository<HRCountry> workflow = new HRServiceWorkflowPaginationOnly<HRCountry>(null, null);
            LanguageRepositoryStub langRepo = new LanguageRepositoryStub();
            langRepo.Langs.Add(new Language() { Name = "1"});
            langRepo.Langs.Add(new Language() { Name = "2" });
            langRepo.Langs.Add(new Language() { Name = "1" });
            CoreCountriesService coreService = new CoreCountriesService(null, langRepo, null, null);
            Region region = Region.Africa;
            using(Task<IEnumerable<Language>> task = coreService.GetHRLangagesByContinentAsync(region))
            {
                await task;
                Assert.NotNull(task.Result);
                Assert.Equal(3, task.Result.ToList().Count);
                Assert.Equal("1", task.Result.ToList()[0].Name);
                Assert.Equal("2", task.Result.ToList()[1].Name);
                Assert.Equal("1", task.Result.ToList()[2].Name);
            }
        }

    }
}

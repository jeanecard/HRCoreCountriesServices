using HRCoreCountriesRepository;
using HRCoreCountriesRepository.Interface;
using NSubstitute;
using QuickType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestDAL
{
    public class MongoDBCountryByContinentByLanguageRepositoryTest
    {
        readonly List<HRCountry> _countriesWithFr = null;
        readonly List<HRCountry> _countriesWithoutFr = null;
        public MongoDBCountryByContinentByLanguageRepositoryTest()
        {
            _countriesWithFr = new List<HRCountry>() {
                new HRCountry()
                { Alpha2Code = "HRCountry",
                    Languages = new Language[] 
                    {
                        new Language() { Iso6391 = "Fr" },
                        new Language() { Iso6391 = "en" }
                    }
                },
                new HRCountry()
                { Alpha2Code = "HR",
                    Languages = new Language[] 
                    {
                        new Language() { Iso6391 = "Es" },
                        new Language() { Iso6391 = "en" }
                    }
                }
            };
            _countriesWithoutFr = new List<HRCountry>() {
                new HRCountry()
                { Alpha2Code = "ZZ",
                    Languages = new Language[] 
                    {
                        new Language() { Iso6391 = "De" },
                        new Language() { Iso6391 = "en" }
                    }
                },
                new HRCountry() { Alpha2Code = "AA",
                    Languages = new Language[] 
                    {
                        new Language() { Iso6391 = "Es" },
                        new Language() { Iso6391 = "Po" }
                    }
                }
            };
        }
        /// <summary>
        /// Check that GetHRCountriesByContinentByLanguageAsync throw  ArgumentNullException if no repository is supplied.
        /// </summary>
        [Fact]
        public async void MongoDBCountryByContinentByLanguageRepository_GetHRCountriesByContinentByLanguageAsync_With_Null_Repository_Throw_ArgumentNullException()
        {
            MongoDBCountryByContinentByLanguageRepository _repoWithNullParams = new MongoDBCountryByContinentByLanguageRepository(null, null);
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => 
                {
                    using(Task<IEnumerable<HRCountry>> testTask = _repoWithNullParams.GetHRCountriesByContinentByLanguageAsync(Region.Africa, "Fr"))
                    {
                        await testTask;
                    }
                }
            );
        }
        /// <summary>
        /// Check that GetHRCountriesByContinentByLanguageAsync throw  ArgumentNullException if no iso6391 is supplied (null or String Empty).
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async void MongoDBCountryByContinentByLanguageRepository_GetHRCountriesByContinentByLanguageAsync_With_Iso6391_Param_Null_Or_Empty_Throw_ArgumentNullException(String isoCode)
        {
            IHRCountryByContinentRepository _repo = Substitute.For<IHRCountryByContinentRepository>();
            MongoDBCountryByContinentByLanguageRepository _repoWithNullParams = new MongoDBCountryByContinentByLanguageRepository(null, _repo);
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () =>
                {
                    using (Task<IEnumerable<HRCountry>> testTask = _repoWithNullParams.GetHRCountriesByContinentByLanguageAsync(Region.Africa, isoCode))
                    {
                        await testTask;
                    }
                }
            );
        }
        /// <summary>
        /// Check that GetHRCountriesByContinentByLanguageAsync Get Fr Countries with Fr Language on _countriesWithFr return HRCountry
        /// </summary>
        [Fact]
        public async void MongoDBCountryByContinentByLanguageRepository_GetHRCountriesByContinentByLanguageAsync_With_countriesWithFr_Expect_HRCountry()
        {
            IHRCountryByContinentRepository _repo = Substitute.For<IHRCountryByContinentRepository>();
            _repo.GetHRCountriesByContinentAsync(Region.Europe).Returns<IEnumerable<HRCountry>>(_countriesWithFr);
            MongoDBCountryByContinentByLanguageRepository _mongoRepo = new MongoDBCountryByContinentByLanguageRepository(null, _repo);
            using(Task<IEnumerable<HRCountry>> taskResult = _mongoRepo.GetHRCountriesByContinentByLanguageAsync(Region.Europe, "Fr"))
            {
                await taskResult;
                Assert.NotNull(taskResult.Result);
                List<HRCountry> resultList = taskResult.Result.ToList();
                Assert.NotEmpty(resultList);
                Assert.True(resultList.Count == 1);
                Assert.True(resultList[0].Alpha2Code == "HRCountry");
            }
        }
        /// <summary>
        /// Check that GetHRCountriesByContinentByLanguageAsync Get Fr Countries with Fr Language on _countriesWithoutFr return empty list.
        /// </summary>
        [Fact]
        public async void MongoDBCountryByContinentByLanguageRepository_GetHRCountriesByContinentByLanguageAsync_With_countriesWithoutFr_Expect_Empty_Result()
        {
            IHRCountryByContinentRepository _repo = Substitute.For<IHRCountryByContinentRepository>();
            _repo.GetHRCountriesByContinentAsync(Region.Europe).Returns<IEnumerable<HRCountry>>(_countriesWithoutFr);
            MongoDBCountryByContinentByLanguageRepository _mongoRepo = new MongoDBCountryByContinentByLanguageRepository(null, _repo);
            using (Task<IEnumerable<HRCountry>> taskResult = _mongoRepo.GetHRCountriesByContinentByLanguageAsync(Region.Europe, "Fr"))
            {
                await taskResult;
                Assert.NotNull(taskResult.Result);
                List<HRCountry> resultList = taskResult.Result.ToList();
                Assert.Empty(resultList);
            }
        }
    }
}

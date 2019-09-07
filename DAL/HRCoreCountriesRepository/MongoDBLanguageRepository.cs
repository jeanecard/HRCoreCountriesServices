using HRCoreCountriesRepository.Interface;
using HRCoreCountriesRepository.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using QuickType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCoreCountriesRepository
{
    public class MongoDBLanguageRepository : ILanguageRepository
    {
        private readonly ILogger<MongoDBCountriesRepository> _looger = null;
        private readonly IConfiguration _config = null;
        public MongoDBLanguageRepository(IConfiguration injectedMongoConfig, ILogger<MongoDBCountriesRepository> logger)
        {
            _config = injectedMongoConfig;
            _looger = logger;
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Language>> GetHRLangagesByContinentAsync(Region region)
        {
            Dictionary<String, Language> partialResult = new Dictionary<string, Language>();
            try
            {
                MondoDBConnexionParam conParam = MondoDBConnexionParamFactory.CreateMondoDBConnexionParam(_config);
                IMongoCollection<HRCountry> collection = MongoDBCollectionGetter<HRCountry>.GetCollection(conParam);
                if (collection != null)
                {
                    FilterDefinitionBuilder<HRCountry> bld = new FilterDefinitionBuilder<HRCountry>();
                    Task<IAsyncCursor<HRCountry>> retourTask = null;
                    try
                    {
                        FieldDefinition<HRCountry, int> field = "Region";
                        //var distinctItems = collection.Distinct(new StringFieldDefinition<HRCountry, string>("Attributes.Name"), FilterDefinition<HRCountry>.Empty).ToList();

                        retourTask = collection.FindAsync(bld.Where(country => country.Region == region));
                        //1.1.3-
                        await retourTask;
                        //Message IDE0067 Disposable object created by 'await retourTask' is never disposed whereas finally dispose exists ?
                        if (retourTask.Result != null)
                        {
                            Language langi = null;
                            while(await retourTask.Result.MoveNextAsync())
                            {
                                foreach (HRCountry iter in retourTask.Result.Current)
                                {
                                    int languagesCount = iter.Languages.Length;
                                    for (int i = 0; i < languagesCount; i++)
                                    {
                                        langi = iter.Languages[i];
                                        if (langi != null && !String.IsNullOrEmpty(langi.Iso6391))
                                        {
                                            if (!partialResult.ContainsKey(langi.Iso6391))
                                            {
                                                partialResult.Add(langi.Iso6391, langi);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    finally
                    {
                        if (retourTask != null)
                        {
                            if (retourTask.Result != null)
                            {
                                retourTask.Result.Dispose();
                            }
                            retourTask.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (_looger != null)
                {
                    _looger.LogError(ex.Message);
                }
                throw;
            }
            return partialResult.Values;
        }
    }
}

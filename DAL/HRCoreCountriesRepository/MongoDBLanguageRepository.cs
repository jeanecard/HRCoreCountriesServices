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
    /// <summary>
    /// Class to access HRCountries in MongoDB.
    /// All methods used there are mainly async as they rely on I/O and network request. 
    /// </summary>
    public class MongoDBLanguageRepository : ILanguageRepository
    {
        private readonly ILogger<MongoDBCountriesRepository> _logger = null;
        private readonly IConfiguration _config = null;
        public MongoDBLanguageRepository(IConfiguration injectedMongoConfig, ILogger<MongoDBCountriesRepository> logger)
        {
            _config = injectedMongoConfig;
            _logger = logger;
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Language>> GetHRLangagesByContinentAsync(Region region)
        {
            FilterDefinitionBuilder<HRCountry> bld = new FilterDefinitionBuilder<HRCountry>();
            using (Task<IEnumerable<Language>> task = GetHRLangages(bld.Where(country => country.Region == region)))
            {
                await task;
                return task.Result;
            }
        }
        /// <summary>
        /// Dispose all Disposable Objects used by MongoDB connection.
        /// </summary>
        /// <param name="retourTask"></param>
        private static void DisposeMongoDBTask(Task<IAsyncCursor<HRCountry>> retourTask)
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
        /// <summary>
        /// Update Dictionnary with all unexisting Language in cursor.
        /// 1- Iterate through enumerable
        /// 1.1- For all Languages in iterator 
        /// 1.2- Search iso6391 code (case sensitive) in Dictionnary and Add it if not found
        /// </summary>
        /// <param name="dictionnary">a dictionnary</param>
        /// <param name="cursor">a HRCountry enumerable</param>
        public void FillUpDictionnary(IDictionary<String, Language> dictionnary, IEnumerable<HRCountry> cursor)
        {
            int languagesCounti = 0;
            Language langi = null;
            //1-
            foreach (HRCountry iter in cursor)
            {
                if (iter != null && iter.Languages != null)
                {
                    //1.1-
                    languagesCounti = iter.Languages.Length;
                    for (int i = 0; i < languagesCounti; i++)
                    {
                        langi = iter.Languages[i];
                        if (langi != null && !String.IsNullOrEmpty(langi.Iso6391))
                        {
                            //1.2-
                            if (!dictionnary.ContainsKey(langi.Iso6391))
                            {
                                dictionnary.Add(langi.Iso6391, langi);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Language>> GetHRAllLangagesAsync()
        {
            FilterDefinitionBuilder<HRCountry> bld = new FilterDefinitionBuilder<HRCountry>();
            //retourTask = collection.FindAsync(filterfilter.Where(country => country.Region == region));
            using (Task<IEnumerable<Language>> task = GetHRLangages(bld.Empty))
            {
                await task;
                return task.Result;
            }
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="filterDefinition"></param>
        /// <returns></returns>
        private async Task<IEnumerable<Language>> GetHRLangages(FilterDefinition<HRCountry> filterDefinition)
        {
            Dictionary<String, Language> partialResult = new Dictionary<string, Language>();
            try
            {
                MondoDBConnexionParam conParam = MondoDBConnexionParamFactory.CreateMondoDBConnexionParam(_config);
                IMongoCollection<HRCountry> collection = MongoDBCollectionGetter<HRCountry>.GetCollection(conParam);
                if (collection != null)
                {
                    Task<IAsyncCursor<HRCountry>> retourTask = null;
                    try
                    {
                        retourTask = collection.FindAsync(filterDefinition);
                        await retourTask;
                        //Message IDE0067 Disposable object created by 'await retourTask' is never disposed whereas finally dispose exists ?
                        if (retourTask.Result != null)
                        {
                            Task<bool> cursorMovingTask = null;
                            try
                            {
                                cursorMovingTask = retourTask.Result.MoveNextAsync();
                                await cursorMovingTask;
                                while (cursorMovingTask.Result)
                                {
                                    FillUpDictionnary(partialResult, retourTask.Result.Current);
                                    cursorMovingTask.Dispose();
                                    cursorMovingTask = retourTask.Result.MoveNextAsync();
                                    await cursorMovingTask;
                                }
                            }
                            finally
                            {
                                if (cursorMovingTask != null)
                                {
                                    cursorMovingTask.Dispose();
                                }
                            }
                        }
                    }
                    finally
                    {
                        DisposeMongoDBTask(retourTask);
                    }
                }
            }
            catch (Exception ex)
            {
                if (_logger != null)
                {
                    _logger.LogError(ex.Message);
                }
                throw;
            }
            return partialResult.Values;
        }
    }
}
//FieldDefinition<HRCountry, int> field = "Region";
//var distinctItems = collection.Distinct(new StringFieldDefinition<HRCountry, string>("Attributes.Name"), FilterDefinition<HRCountry>.Empty).ToList();

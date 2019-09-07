using HRCommon.Interface;
using HRCommonModel;
using HRCommonModels;
using HRCoreCountriesRepository.Interface;
using HRCoreCountriesRepository.Util;
using HRCoreRepository.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using QuickType;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRCoreCountriesRepository
{
    public class MongoDBLanguageRepository : ILanguageRepository
    {
        private readonly ILogger<MongoDBCountriesRepository> _looger = null;
        private readonly IConfiguration _config = null;
        private static readonly String _MONGO_CX_STRING_KEY = "CountriesConnection";
        private static readonly String _MONGO_CLUSTER = "MongoDBDataBaseName:ClusterName";
        private static readonly String _MONGO_COUNTRIES_COLLECTION_KEY = "MongoDBDataBaseName:CountriesCollection";
        private static readonly String _MONGO_USERNAME = "MongoDBDataBaseName:Username";
        private static readonly String _MONGO_PASSWORD = "MongoDBDataBaseName:Password";

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
            List<Language> retourStub = new List<Language>();
            try
            {
                MondoDBConnexionParam conParam = CreateMondoDBConnexionParam();
                IMongoCollection<HRCountry> collection = MongoDBCollectionGetter<HRCountry>.GetCollection(conParam);
                if (collection != null)
                {
                    FilterDefinitionBuilder<HRCountry> bld = new FilterDefinitionBuilder<HRCountry>();
                    Task<IAsyncCursor<HRCountry>> retourTask = null;
                    try
                    {
                        retourTask = collection.FindAsync(bld.Where(country => country.Region == region));
                        //1.1.3-
                        await retourTask;
                        //Message IDE0067 Disposable object created by 'await retourTask' is never disposed whereas finally dispose exists ?
                        if (retourTask.Result != null)
                        {
                            List<HRCountry> fullresult = retourTask.Result.ToList();
                            foreach(HRCountry iter in fullresult)
                            {
                                foreach(Language iterLang in iter.Languages)
                                {
                                    retourStub.Add(iterLang);
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
            return retourStub;
        }
        /// <summary>
        /// 1- Context consistence check
        /// 2- Get value from IConfig.
        /// 3- Create and retrun MondoDBConnexionParam
        /// </summary>
        /// <returns></returns>
        private MondoDBConnexionParam CreateMondoDBConnexionParam()
        {
            //1-
            if (_config == null)
            {
                String ErrorMessage = "No config available in MongoDBCountriesRepository GetCountriesCollection";
                if (_looger != null)
                {
                    _looger.LogError(ErrorMessage);
                }

                throw new MemberAccessException(ErrorMessage);
            }
            //2-
            String connectionString = _config.GetConnectionString(_MONGO_CX_STRING_KEY);
            String mongoUSerName = _config[_MONGO_USERNAME];
            String mongoPassword = _config[_MONGO_PASSWORD];
            String clusterName = _config[_MONGO_CLUSTER];
            String collectionName = _config[_MONGO_COUNTRIES_COLLECTION_KEY];
            //3-
            return new MondoDBConnexionParam(
                connectionString,
                mongoUSerName,
                mongoPassword,
                clusterName,
                collectionName);
        }
    }
}

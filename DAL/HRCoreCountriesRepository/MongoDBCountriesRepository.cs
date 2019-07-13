using HRCommon.Interface;
using HRCommonModel;
using HRCommonModels;
using HRCoreRepository.Interface;
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
    /// It's not possible so far to use Dapper on this MongoDB Driver.
    /// </summary>
    public class MongoDBCountriesRepository : IHRCoreRepository<HRCountry>, IPaginable, ISortable
    {
        private readonly ILogger<MongoDBCountriesRepository> _looger = null;
        private readonly IConfiguration _config = null;
        private static readonly String _MONGO_CX_STRING_KEY = "CountriesConnection";
        private static readonly String _MONGO_CLUSTER = "MongoDBDataBaseName:ClusterName";
        private static readonly String _MONGO_COUNTRIES_COLLECTION_KEY = "MongoDBDataBaseName:CountriesCollection";
        private static readonly String _MONGO_USERNAME = "MongoDBDataBaseName:Username";
        private static readonly String _MONGO_PASSWORD = "MongoDBDataBaseName:Password";


        //Default Constructor
        private MongoDBCountriesRepository()
        {
        }
        //Constructor for DI with Configuration
        public MongoDBCountriesRepository(IConfiguration injectedMongoConfig, ILogger<MongoDBCountriesRepository> logger)
        {
            _config = injectedMongoConfig;
            _looger = logger;
        }
        /// <summary>
        /// 1- Get collection of Countries from Mongo
        ///     1.1- If collection is valid
        ///         1.1.2- Create an Empty Filter to get All Countries and create the corresponding task
        ///         1.1.3- await task result and if Result is not null return result
        ///     1.2- Else
        ///         Throw Code InvalidOperationException
        /// 2- Return null result if previous algorithm does not throw any exception and does not return any result.
        /// </summary>
        /// <returns>return all countries (a collection with single element if if exists and is supplied)
        /// or throw Exceptions :
        ///     InvalidOperationException if collection cannot be retrieved
        ///     ArgumentOutOfRangeException if objectID supplied can not be converted in MongoDB ID
        ///     System Exception as is if any other exception is thrown.
        /// </returns>
        public async Task<IEnumerable<HRCountry>> GetCountriesAsync()
        {
            IEnumerable<HRCountry> retour = null;
            Task<IAsyncCursor<HRCountry>> retourTask = null;
            //1-
            try
            {
                IMongoCollection<HRCountry> collection = GetCountriesCollection();
                //1.1-
                if (collection != null)
                {
                    //1.1.2-
                    FilterDefinitionBuilder<HRCountry> bld = new FilterDefinitionBuilder<HRCountry>();
                    retourTask = collection.FindAsync(bld.Empty);
                    //1.1.3-
                    await retourTask;
                    //Message IDE0067 Disposable object created by 'await retourTask' is never disposed whereas finally dispose exists ?

                    if (retourTask.Result != null)
                    {
                        //Force to list to avoid return asyn enum that can be enumerate only once.
                        retour = retourTask.Result.ToList();
                    }
                }
                //1.2-
                else
                {
                    String ErrorMessage = "Collection not found in MongoDBCountriesRepository GetCountriesAsync";
                    if (_looger != null)
                    {
                        _looger.LogError(ErrorMessage);
                    }
                    throw new InvalidOperationException(ErrorMessage);
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
            finally
            {
                retourTask.Dispose();
            }
            //2-
            return retour;
        }
        /// <summary>
        /// 1- Test Context validity
        /// 2- Get DatabaseName and CollectionName from IConfiguration
        /// 3- Instanciate MongoDB Database and client
        /// 4- Return the collection (synch method)
        /// </summary>
        /// <returns>The MongoDB Collection for Countries. Can throw MemberAccessException.
        /// Does not catch any Exception.
        /// </returns>
        private IMongoCollection<HRCountry> GetCountriesCollection()
        {
            if (_config == null)
            {
                String ErrorMessage = "No config available in MongoDBCountriesRepository GetCountriesCollection";
                if (_looger != null)
                {
                    _looger.LogError(ErrorMessage);
                }

                throw new MemberAccessException(ErrorMessage);
            }
            IMongoCollection<HRCountry> retour = null;
            //1-
            if (_config != null)
            {
                //2-
                String connectionString = _config.GetConnectionString(_MONGO_CX_STRING_KEY);
                connectionString = String.Format(connectionString, _config[_MONGO_USERNAME], _config[_MONGO_PASSWORD]);
                String clusterName = _config[_MONGO_CLUSTER];
                if (!String.IsNullOrEmpty(connectionString) && !String.IsNullOrEmpty(clusterName))
                {
                    //3-
                    MongoClient client = new MongoClient(connectionString);
                    IMongoDatabase database = client.GetDatabase(clusterName);
                    if (database != null)
                    {
                        //4-
                        String collectionName = _config[_MONGO_COUNTRIES_COLLECTION_KEY];
                        retour = database.GetCollection<HRCountry>(collectionName);
                    }
                }
            }
            return retour;
        }
        /// <summary>
        /// Pick a country in collection by his ID (ALPHA 2 or 3 Code).
        /// </summary>
        /// <param name="id">The searched ID (Alpha2 or Alpha3)</param>
        /// <returns>The corrresponding HRCountry or null if not found. Can throw the following exception :
        /// </returns>
        public async Task<HRCountry> GetAsync(string id)
        {
            HRCountry retour = null;
            if (String.IsNullOrEmpty(id))
            {
                return null;
            }
            try
            {
                String idToSearch = id.ToUpper();
                IMongoCollection<HRCountry> collection = GetCountriesCollection();
                if (collection != null)
                {
                    FilterDefinitionBuilder<HRCountry> bld = new FilterDefinitionBuilder<HRCountry>();
                    Task<IAsyncCursor<HRCountry>> retourTask = null;
                    try
                    {
                        retourTask = collection.FindAsync(bld.Where(country =>
                        ((!String.IsNullOrEmpty(country.Alpha2Code)) && (country.Alpha2Code == idToSearch))
                        ||
                       ((!String.IsNullOrEmpty(country.Alpha3Code)) && (country.Alpha3Code == idToSearch))));
                        //1.1.3-
                        await retourTask;
                        //Message IDE0067 Disposable object created by 'await retourTask' is never disposed whereas finally dispose exists ?
                        if (retourTask.Result != null)
                        {
                            retour = retourTask.Result.FirstOrDefault();
                        }
                    }
                    finally
                    {
                        retourTask.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                if(_looger != null)
                {
                    _looger.LogError(ex.Message);
                }
                throw;
            }
            //2-
            return retour;
        }
        /// <summary>
        /// Not implemented in Version 1.
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public Task<IEnumerable<HRCountry>> GetOrderedsAsync(HRSortingParamModel orderBy)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get All Countries.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<HRCountry>> GetFullsAsync()
        {
            IEnumerable<HRCountry> retour = null;
            //1-
            try
            {
                IMongoCollection<HRCountry> collection = GetCountriesCollection();
                //1.1-
                if (collection != null)
                {
                    //1.1.2-
                    FilterDefinitionBuilder<HRCountry> bld = new FilterDefinitionBuilder<HRCountry>();
                    using (Task<IAsyncCursor<HRCountry>> retourTask = collection.FindAsync(bld.Empty))
                    {
                        //1.1.3-
                        await retourTask;
                        //Message IDE0067 Disposable object created by 'await retourTask' is never disposed whereas using dispose exists ?
                        if (retourTask.Result != null)
                        {
                            //Force to list to avoid return asyn enum that can be enumerate only once.
                            retour = retourTask.Result.ToList();
                        }
                    }
                }
                else
                {
                    String ErrorMessage = "No Collection found in MongoDBCountriesRepository GetFullsAsync";
                    if (_looger != null)
                    {
                        _looger.LogError(ErrorMessage);
                    }
                    throw new InvalidOperationException(ErrorMessage);
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
            //2-
            return retour;
        }

        /// <summary>
        /// Not implemented in version 1.
        /// </summary>
        /// <param name="pageModel"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public Task<PagingParameterOutModel<HRCountry>> GetOrderedAndPaginatedsAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented in version 1.
        /// </summary>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        public Task<PagingParameterOutModel<HRCountry>> GetPaginatedsAsync(PagingParameterInModel pageModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// False in version 1 even if MongoDB can Order.
        /// </summary>
        /// <returns></returns>
        public bool IsSortable()
        {
            return false;
        }

        /// <summary>
        /// False in version 1 even if MongoDB can paginate.
        /// </summary>
        /// <returns></returns>
        public bool IsPaginable()
        {
            return false;
        }
    }
}
using HRCommon.Interface;
using HRCommonModel;
using HRCommonModels;
using HRCoreCountriesRepository.Util;
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
        private readonly ILogger<MongoDBCountriesRepository> _logger = null;
        private readonly IConfiguration _config = null;

        //Default Constructor
        private MongoDBCountriesRepository()
        {
        }
        //Constructor for DI with Configuration
        public MongoDBCountriesRepository(IConfiguration injectedMongoConfig, ILogger<MongoDBCountriesRepository> logger)
        {
            _config = injectedMongoConfig;
            _logger = logger;
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
                MondoDBConnexionParam conParam = MondoDBConnexionParamFactory.CreateMondoDBConnexionParam(_config);
                IMongoCollection<HRCountry> collection = MongoDBCollectionGetter<HRCountry>.GetCollection(conParam);
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
                    if (_logger != null)
                    {
                        _logger.LogError(ErrorMessage);
                    }
                    throw new InvalidOperationException(ErrorMessage);
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
            finally
            {
                if (retourTask != null)
                {
                    if(retourTask.Result != null)
                    {
                        retourTask.Result.Dispose();
                    }
                    retourTask.Dispose();
                }
            }
            //2-
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
                MondoDBConnexionParam conParam = MondoDBConnexionParamFactory.CreateMondoDBConnexionParam(_config);
                IMongoCollection<HRCountry> collection = MongoDBCollectionGetter<HRCountry>.GetCollection(conParam);
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
                if(_logger != null)
                {
                    _logger.LogError(ex.Message);
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
                MondoDBConnexionParam conParam = MondoDBConnexionParamFactory.CreateMondoDBConnexionParam(_config);
                IMongoCollection<HRCountry> collection = MongoDBCollectionGetter<HRCountry>.GetCollection(conParam);
                //1.1-
                if (collection != null)
                {
                    //1.1.2-
                    FilterDefinitionBuilder<HRCountry> bld = new FilterDefinitionBuilder<HRCountry>();
                    Task<IAsyncCursor<HRCountry>> retourTask = null;
                    try
                    {
                        retourTask = collection.FindAsync(bld.Empty);
                    

                        FilterDefinitionBuilder<HRCountry> builder = new FilterDefinitionBuilder<HRCountry>();
                        builder.Eq<Region>((HRCountry x) => x.Region, Region.Africa);
                        //1.1.3-
                        await retourTask;
                        if (retourTask.Result != null)
                        {
                            //Force to list to avoid return asyn enum that can be enumerate only once.
                            retour = retourTask.Result.ToList();
                        }
                    }
                    finally
                    {
                        if(retourTask != null)
                        {
                            if(retourTask.Result != null)
                            {
                                retourTask.Result.Dispose();
                            }
                            retourTask.Dispose();
                        }
                    }
                }
                else
                {
                    String ErrorMessage = "No Collection found in MongoDBCountriesRepository GetFullsAsync";
                    if (_logger != null)
                    {
                        _logger.LogError(ErrorMessage);
                    }
                    throw new InvalidOperationException(ErrorMessage);
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
        /// <summary>
        /// Dummy.
        /// </summary>
        /// <param name="iDs"></param>
        /// <returns></returns>
        public Task<IEnumerable<HRCountry>> GetAsync(IEnumerable<string> iDs)
        {
            throw new NotImplementedException();
        }
    }
}
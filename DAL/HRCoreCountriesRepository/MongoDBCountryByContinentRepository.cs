using HRCoreCountriesRepository.Interface;
using HRCoreCountriesRepository.Util;
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
    public class MongoDBCountryByContinentRepository : IHRCountryByContinentRepository
    {
        private readonly ILogger<MongoDBCountriesRepository> _logger = null;
        private readonly IConfiguration _config = null;

        public MongoDBCountryByContinentRepository(IConfiguration injectedMongoConfig, 
            ILogger<MongoDBCountriesRepository> logger)
        {
            _config = injectedMongoConfig;
            _logger = logger;
        }
        /// <summary>
        /// 1- Connect and Get MongoDB Collection
        /// 2- Create Filter (Empty if Region = All else Filter on Region)
        /// 3- Apply Filter on Collection and convert result to Enumerable (give up async in this pre-version) TODO
        /// 4- Release Disposable elements.
        /// </summary>
        /// <param name="region"></param>
        /// <returns>Corresponding Countries in Region or re throw any Exception raised buy MongoDB.</returns>
        public async Task<IEnumerable<HRCountry>> GetHRCountriesByContinentAsync(Region region)
        {
            IEnumerable<HRCountry> retour = null;
            try
            {
                //1-
                MondoDBConnexionParam conParam = MondoDBConnexionParamFactory.CreateMondoDBConnexionParam(_config);
                IMongoCollection<HRCountry> collection = MongoDBCollectionGetter<HRCountry>.GetCollection(conParam);
                if (collection != null)
                {
                    //2-
                    FilterDefinitionBuilder<HRCountry>  filterDefinitionBuilder = new FilterDefinitionBuilder<HRCountry>();
                    Task<IAsyncCursor<HRCountry>> retourTask = null;
                    try
                    {
                        FilterDefinition<HRCountry> filter = filterDefinitionBuilder.Empty;
                        if(region != Region.All)
                        {
                            filter = filterDefinitionBuilder.Where(country => country.Region == region);
                        }
                        //3-
                        retourTask = collection.FindAsync(filter);
                        await retourTask;
                        if (retourTask.Result != null)
                        {
                            retour = retourTask.Result.ToEnumerable<HRCountry>();
                        }
                    }
                    //4-
                    finally
                    {
                        if (retourTask != null)
                        {
                            retourTask.Dispose();
                        }
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
            return retour;
        }
    }
}

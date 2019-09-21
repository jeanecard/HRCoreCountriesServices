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

        public MongoDBCountryByContinentRepository(IConfiguration injectedMongoConfig, ILogger<MongoDBCountriesRepository> logger)
        {
            _config = injectedMongoConfig;
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public async Task<IEnumerable<HRCountry>> GetHRCountriesByContinentAsync(Region region)
        {
            IEnumerable<HRCountry> retour = null;
            try
            {
                MondoDBConnexionParam conParam = MondoDBConnexionParamFactory.CreateMondoDBConnexionParam(_config);
                IMongoCollection<HRCountry> collection = MongoDBCollectionGetter<HRCountry>.GetCollection(conParam);
                if (collection != null)
                {
                    FilterDefinitionBuilder<HRCountry>  filterDefinitionBuilder = new FilterDefinitionBuilder<HRCountry>();
                    Task<IAsyncCursor<HRCountry>> retourTask = null;
                    try
                    {
                        FilterDefinition<HRCountry> filter = filterDefinitionBuilder.Empty;
                        if(region != Region.All)
                        {
                            //Indeed all does not exist in DB
                            filter = filterDefinitionBuilder.Where(country => country.Region == region);
                        }
                        retourTask = collection.FindAsync(filter);
                        await retourTask;
                        if (retourTask.Result != null)
                        {
                            retour = retourTask.Result.ToEnumerable<HRCountry>();
                        }
                    }
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

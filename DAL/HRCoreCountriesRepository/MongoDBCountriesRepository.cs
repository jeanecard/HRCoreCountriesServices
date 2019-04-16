using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using QuickType;
using Microsoft.Extensions.Configuration;
using HRDALExceptionLib;

namespace HRCoreCountriesRepository
{
    public class MongoDBCountriesRepository : ICountriesRepository
    {
        private IConfiguration _config = null;//
        private static String _MONGO_DATABASE_KEY = "MongoDBDataBaseName";
        private static String _MONGO_COUNTRIES_COLLECTION_KEY = "Countries";

        //Private Constructor for DI
        public MongoDBCountriesRepository()
        {
        }
        //Constructor for DI with Configuration
        private MongoDBCountriesRepository(IConfiguration injectedMongoConfig)
        {
            _config = injectedMongoConfig;
        }
        private IMongoCollection<HRCountry> GetCountriesCollection()
        {
            IMongoCollection<HRCountry> retour = null;
            if (_config != null)
            {
                String dataBaseName = _config[_MONGO_DATABASE_KEY];
                String connectionString = _config.GetConnectionString(dataBaseName);
                if (!String.IsNullOrEmpty(connectionString))
                {
                    MongoClient client = new MongoClient(connectionString);
                    IMongoDatabase database = client.GetDatabase(dataBaseName);
                    if (database != null)
                    {
                        retour = database.GetCollection<HRCountry>(_config[_MONGO_COUNTRIES_COLLECTION_KEY]);
                    }
                    else
                    {
                        HRDALException.ThrowException("", "");
                    }
                }
                else
                {
                    HRDALException.ThrowException("", "");
                }
            }
            return retour;
        }
        public async Task<IEnumerable<HRCountry>> GetCountriesAsync()
        {
            try
            {
                IMongoCollection<HRCountry> collection = GetCountriesCollection();
                FilterDefinitionBuilder<HRCountry> bld = new FilterDefinitionBuilder<HRCountry>();
                using (Task<IAsyncCursor<HRCountry>> retourTask = collection.FindAsync(bld.Empty))
                {
                    await retourTask;
                    return retourTask.Result.ToEnumerable();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

    }
}
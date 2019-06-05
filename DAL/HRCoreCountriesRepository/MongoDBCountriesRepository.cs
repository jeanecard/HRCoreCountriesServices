using HRDALExceptionLib;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using QuickType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCoreCountriesRepository
{
    public class MongoDBCountriesRepository : ICountriesRepository
    {
        private IConfiguration _config = null;//
        private static String _MONGO_CX_STRING_KEY = "CountriesConnection";
        private static String _MONGO_CLUSTER = "MongoDBDataBaseName:ClusterName";
        private static String _MONGO_COUNTRIES_COLLECTION_KEY = "MongoDBDataBaseName:CountriesCollection";
        private static String _MONGO_USERNAME = "MongoDBDataBaseName:Username";
        private static String _MONGO_PASSWORD = "MongoDBDataBaseName:Password";


        //Default Constructor
        private MongoDBCountriesRepository()
        {
        }
        //Constructor for DI with Configuration
        public MongoDBCountriesRepository(IConfiguration injectedMongoConfig)
        {
            _config = injectedMongoConfig;
        }
        /// <summary>
        /// 1- Get collection of Countries from Mongo
        /// If collection is valid
        ///     1.2- Create an Empty Filter to get All Countries
        ///     1.3- Create Task of Find
        ///         1.3.1- Await task end
        ///         1.3.2- Return Enumerable
        /// Else
        ///     Throw Code 101 Exception
        ///  Rethrow all others exceptions.
        /// </summary>
        /// <returns>return all countries or throw Exceptions :
        ///     Code 101 if collection cannot be retrieved
        ///     System Exception as is if any other exception is thrown.
        /// </returns>
        public async Task<IEnumerable<HRCountry>> GetCountriesAsync()
        {
            try
            {
                //1-
                IMongoCollection<HRCountry> collection = GetCountriesCollection();
                if(collection !=null)
                {
                    //1.2-
                    FilterDefinitionBuilder<HRCountry> bld = new FilterDefinitionBuilder<HRCountry>();
                    //1.3-
                    using (Task<IAsyncCursor<HRCountry>> retourTask = collection.FindAsync(bld.Empty))
                    {
                        //1.3.1-
                        await retourTask;
                        //1.3.2-
                        return retourTask.Result.ToEnumerable();
                    }
                }
                //2-
                else
                {
                    HRDALException.ThrowException("Can not retrieve the following collection : " + _MONGO_COUNTRIES_COLLECTION_KEY, "101");
                }

            }
            catch (Exception ex)
            {
                //!TODO Log pattern to set
                Console.WriteLine(ex.Message);
                throw;
            }
            return null;
        }
        /// <summary>
        /// 1- Test Context validity
        /// 2- Get DatabaseName and CollectionName from IConfiguration
        /// 3- Instanciate MongoDB Database and client
        /// 4- Return the collection (synch method)
        /// </summary>
        /// <returns>The MongoDB Collection for Countries or null if any element of the context is invalid.
        /// Does not catch any Exception.
        /// </returns>
        private IMongoCollection<HRCountry> GetCountriesCollection()
        {
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
    }
}
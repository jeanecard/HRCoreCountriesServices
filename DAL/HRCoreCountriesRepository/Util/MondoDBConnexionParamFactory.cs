using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRCoreCountriesRepository.Util
{
    internal static class MondoDBConnexionParamFactory
    {
        /// <summary>
        /// 1- Context consistence check
        /// 2- Get value from IConfig.
        /// 3- Create and retrun MondoDBConnexionParam
        /// </summary>
        /// <returns></returns>
        public static MondoDBConnexionParam CreateMondoDBConnexionParam(IConfiguration config)
        {
            //1-
            if (config == null)
            {
                throw new ArgumentNullException();
            }
            //2-
            String connectionString = config.GetConnectionString(MongoDBConstant.MONGO_CX_STRING_KEY);
            String mongoUSerName = config[MongoDBConstant.MONGO_USERNAME];
            String mongoPassword = config[MongoDBConstant.MONGO_PASSWORD];
            String clusterName = config[MongoDBConstant.MONGO_CLUSTER];
            String collectionName = config[MongoDBConstant.MONGO_COUNTRIES_COLLECTION_KEY];
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

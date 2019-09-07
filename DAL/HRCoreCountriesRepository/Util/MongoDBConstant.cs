using System;
using System.Collections.Generic;
using System.Text;

namespace HRCoreCountriesRepository
{
    /// <summary>
    /// MongoDB constants.
    /// </summary>
    internal static class MongoDBConstant
    {
        public static String MONGO_CX_STRING_KEY = "CountriesConnection";
        public static String MONGO_CLUSTER = "MongoDBDataBaseName:ClusterName";
        public static String MONGO_COUNTRIES_COLLECTION_KEY = "MongoDBDataBaseName:CountriesCollection";
        public static String MONGO_USERNAME = "MongoDBDataBaseName:Username";
        public static String MONGO_PASSWORD = "MongoDBDataBaseName:Password";
    }
}

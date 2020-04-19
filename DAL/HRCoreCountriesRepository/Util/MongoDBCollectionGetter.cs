using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRCoreCountriesRepository.Util
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class MongoDBCollectionGetter<T>
    {
        /// <summary>
        /// 1- Instanciate MongoDB Database and client
        /// 2- Return the collection (synch method)
        /// </summary>
        /// <param name="connexionParam"></param>
        /// <returns></returns>
        static public IMongoCollection<T> GetCollection(MondoDBConnexionParam connexionParam)
        {
            IMongoCollection<T> retour = null;
            //1-
            String connectionString = String.Format(connexionParam.ConnectionString, connexionParam.UserName, connexionParam.Password);
            if (!String.IsNullOrEmpty(connectionString) && !String.IsNullOrEmpty(connexionParam.ClusterName))
            {
                MongoClient client = new MongoClient(connectionString);
                IMongoDatabase database = client.GetDatabase(connexionParam.ClusterName);
                if (database != null)
                {
                    //2-
                    retour = database.GetCollection<T>(connexionParam.CollectionName);
                }
            }
            return retour;
        }
    }
}

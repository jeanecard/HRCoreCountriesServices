using System;
using System.Collections.Generic;
using System.Text;

namespace HRCoreCountriesRepository
{
    internal class MondoDBConnexionParam
    {
        private readonly String _connectionString = String.Empty;
        private readonly String _userName = String.Empty;
        private readonly String _password = String.Empty;
        private readonly String _clusterName = String.Empty;
        private readonly String _collectionName = String.Empty;

        private MondoDBConnexionParam()
        {
            //Dummy.
        }

        public MondoDBConnexionParam(
            String connectionString,
            String userName,
            String password,
            String clusterName,
            String collectionName
            )
        {
            _connectionString = connectionString;
            _userName = userName;
            _password = password;
            _clusterName = clusterName;
            _collectionName = collectionName;
        }
        public string ConnectionString => _connectionString;

        public string UserName => _userName;

        public string Password => _password;

        public string ClusterName => _clusterName;

        public string CollectionName => _collectionName;
    }
}

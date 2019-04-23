using Npgsql;
using Npgsql.BackendMessages;
using Npgsql.PostgresTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tools.Interfaces;

namespace Tools
{
    //Act like a facade over a NpgsqlDataReader
    public class PostGisFieldValueGetter : IFieldValueGetter
    {
        private NpgsqlDataReader _reader = null;

        public int FieldCount => throw new NotImplementedException();

        private PostGisFieldValueGetter()
        {

        }
        public PostGisFieldValueGetter(NpgsqlDataReader reader)
        {
            _reader = reader;
        }
        public T GetFieldValue<T>(int ordinal)
        {
            if(_reader != null)
            {
                return _reader.GetFieldValue<T>(ordinal);
            }
            else
            {
                throw new TypeInitializationException("NpgsqlDataReader", new NotImplementedException());
            }
        }
        public PostgresType GetPostgresType(int ordinal)
        {
            if (_reader != null)
            {
                return _reader.GetPostgresType(ordinal);
            }
            else
            {
                throw new TypeInitializationException("NpgsqlDataReader", new NotImplementedException());
            }

        }
    }
}

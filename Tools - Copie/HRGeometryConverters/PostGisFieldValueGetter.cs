using Npgsql;
using Npgsql.BackendMessages;
using Npgsql.PostgresTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HRConverters.Interfaces;

namespace HRConverters
{
    //Act like a facade over a NpgsqlDataReader
    public class PostGisFieldValueGetter : IFieldValueGetter
    {
        private NpgsqlDataReader _reader = null;

        public int FieldCount
        {
            get {
                if (_reader != null)
                {
                    return _reader.FieldCount;
                }
                else
                {
                    return 0;
                }
            }
        }
        


        private PostGisFieldValueGetter()
        {

        }
        public PostGisFieldValueGetter(NpgsqlDataReader reader)
        {
            _reader = reader;
        }
        public PostgisGeometry GetFieldValue<PostgisGeometry>(int ordinal)
        {
            if(_reader != null)
            {
                return _reader.GetFieldValue<PostgisGeometry>(ordinal);
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

        //PostgisGeometry IFieldValueGetter.GetFieldValue<PostgisGeometry>(int ordinal)
        //{
        //    throw new NotImplementedException();
        //}

        //PostgresType IFieldValueGetter.GetPostgresType(int ordinal)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

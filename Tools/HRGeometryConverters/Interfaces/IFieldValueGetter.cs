using Npgsql.LegacyPostgis;
using Npgsql.PostgresTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRConverters.Interfaces
{
    public interface IFieldValueGetter
    {
        T GetFieldValue<T>(int ordinal);
        int FieldCount
        {
            get;

        }

        PostgresType GetPostgresType(int ordinal);
    }
}


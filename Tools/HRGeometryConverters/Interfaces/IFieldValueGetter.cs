using Npgsql.PostgresTypes;

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


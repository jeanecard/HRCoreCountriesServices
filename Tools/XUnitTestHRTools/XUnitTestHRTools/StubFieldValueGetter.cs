using HRConverters.Interfaces;
using Npgsql.LegacyPostgis;
using Npgsql.PostgresTypes;
using System;

namespace XUnitPostGis
{
    internal class StubFieldValueGetter : IFieldValueGetter
    {
        public int FieldCount { get; set; }

        public T GetFieldValue<T>(int ordinal)
        {
            if (ordinal == OrdinalToStub)
            {
                return (T)(Object)GeometryToStub;
            }
            else
            {
                throw new Exception();
            }
        }
        public PostgresType GetPostgresType(int ordinal)
        {
            if (ordinal == OrdinalToStub)
            {

                return new PostgresTypeStub();
            }
            else
            {
                throw new Exception();
            }
        }
        public PostgisGeometry GeometryToStub { get; set; }
        public int OrdinalToStub { get; set; }

    }
}

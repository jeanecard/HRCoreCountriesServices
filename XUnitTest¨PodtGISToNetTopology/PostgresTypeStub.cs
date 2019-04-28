using Npgsql.PostgresTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitPostGis
{
    internal class PostgresTypeStub : PostgresType
    {
        public PostgresTypeStub() : base("geometry", "geometry", 4326)
        {

        }
        //
        // Résumé :
        //     The data type's name.
        //
        // Remarques :
        //     Note that this is the standard, user-displayable type name (e.g. integer[]) rather
        //     than the internal PostgreSQL name as it is in pg_type (_int4). See Npgsql.PostgresTypes.PostgresType.InternalName
        //     for the latter.
        public new string Name
        {
            get
            {
                return "geometry";
            }
        }
    }
}

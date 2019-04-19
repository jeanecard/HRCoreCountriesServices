using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Npgsql;
using Npgsql.PostgresTypes;
using System;
using System.Data;

namespace Tools
{
    internal class HRConverterPostGisToGeoJsonNet 
    {
        static public IGeometryObject ConvertFrom(NpgsqlDataReader pgCursor)
        {
            if(pgCursor != null)
            {
                int cols = pgCursor.FieldCount;
                for(int i = 0; i < cols; i++)
                {
                    PostgresType typei = pgCursor.GetPostgresType(i);
                    if(typei.Name == "geometry")
                    {
                       //PostgisGeometry saucisse = pgCursor.GetFieldValue<PostgisGeometry>(i);
                        

                        int j = 42;
                        j++;

                        break;
                    }

                }
            }
            //PostgisMultiPolygon geometry = reader.GetFieldValue<PostgisMultiPolygon>(1);
            throw new System.NotImplementedException();
        }
    }
}
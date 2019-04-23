//using GeoJSON.Net.Feature;
//using GeoJSON.Net.Geometry;
using Npgsql;
using Npgsql.LegacyPostgis;
using Npgsql.PostgresTypes;
using System;
using System.Data;
using NetTopologySuite;
using NetTopologySuite.Geometries; //C'est la geometrie se rapprochant le plus du standard dans le mode .net
using Tools.Interfaces;

namespace Tools
{
    //Normalement devrait etre fait par le derniere version de Npgsql.NetTopologySuite mais ca marhe pas. On dirait que la version de postgis avec qgis n'est pas encore compatible avec ce nouiveau driver.
    public class HRConverterPostGisToNetTopologySuite 
    {
        static public Geometry ConvertFrom(IFieldValueGetter pgCursor)
        {
            if(pgCursor != null)
            {
                int cols = pgCursor.FieldCount;
                for(int i = 0; i < cols; i++)
                {
                    PostgresType typei = pgCursor.GetPostgresType(i);
                    if(typei.Name == "geometry")
                    {
                       PostgisGeometry geometry = pgCursor.GetFieldValue<PostgisGeometry>(i);
                        if(geometry is PostgisPoint)
                        {
                            PostgisPoint point = (PostgisPoint)geometry;
                            Console.WriteLine("La geometry est un point)");
                        }
                        if(geometry is PostgisMultiPolygon)
                        {
                            PostgisMultiPolygon mp = (PostgisMultiPolygon)geometry;
                            //MultiPolygon retour = new MultiPolygon(null);
                            //retour.Factory.CreateMultiPolygon()
                            //retour.SRID = Integer(mp.SRID);
                            //retour.Factory.CreatePolygon()
                            //retour.
                            Console.WriteLine("Nombre de polygones : " + mp.PolygonCount);
                        }
                        

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
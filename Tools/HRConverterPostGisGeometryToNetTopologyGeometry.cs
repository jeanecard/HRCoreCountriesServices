//using GeoJSON.Net.Feature;
//using GeoJSON.Net.Geometry;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries; //C'est la geometrie se rapprochant le plus du standard dans le mode .net
using Npgsql.LegacyPostgis;
using Npgsql.PostgresTypes;
using System;
using System.Collections.Generic;
using Tools.Interfaces;

namespace Tools
{
    //Npgsql.NetTopologySuite do this job but it's not working well with the PostGis version of QGIS.
    public class HRConverterPostGisToNetTopologySuite 
    {
        /// <summary>
        /// Name of geometry column in PostGis.
        /// </summary>
        private static String POSTGIS_GEOMETRY_TYPE = "GEOMETRY";
        /// <summary>
        /// Convert the first PostGis Geometry column or the column specified by geomOrdinal into the corresponding NetTopologyGeometry
        /// Can throw the following Exceptions
        /// - ArgumentNullException
        /// - InvalidCastException in case of SRID not compatible for example or if no converter is available.
        /// </summary>
        /// <param name="pgCursor">The cursor where to look for the Geometry column</param>
        /// <param name="geomOrdinal">optionnal, geom. column index if known, otherwise will be search for.</param>
        /// <returns></returns>
        static public Geometry ConvertFrom(IFieldValueGetter pgCursor, int? geomOrdinal = null)
        {
            Geometry retour = null;
            if (pgCursor != null)
            {
                int geomColumnIndex = GetGeomtryColumnIndex(pgCursor, geomOrdinal);
                int cols = pgCursor.FieldCount;
                for(int i = 0; i < cols; i++)
                {
                    PostgresType typei = pgCursor.GetPostgresType(i);
                    if(typei != null && !String.IsNullOrEmpty(typei.Name) && typei.Name.ToUpper() == POSTGIS_GEOMETRY_TYPE)
                    {
                       PostgisGeometry geometry = pgCursor.GetFieldValue<PostgisGeometry>(i);
                        if(geometry is PostgisPoint)
                        {
                            retour = ProcessPoint((PostgisPoint)geometry);
                        }
                        else if (geometry is PostgisMultiPoint)
                        {
                            retour = ProcessMultiPoint((PostgisMultiPoint)geometry);
                        }
                        else if (geometry is PostgisLineString)
                        {
                            retour = ProcessLineString((PostgisLineString)geometry);
                        }
                        else if (geometry is PostgisMultiLineString)
                        {
                            retour = ProcessMultiLineString((PostgisMultiLineString)geometry);
                        }
                        else if (geometry is PostgisPolygon)
                        {
                            retour = ProcessPolygon((PostgisPolygon)geometry);
                        }
                        else if (geometry is PostgisMultiPolygon)
                        {
                            retour = ProcessMultiPolygon((PostgisMultiPolygon)geometry);
                        }
                        else
                        {
                            throw new InvalidCastException();
                        }
                        SetSRID(retour, geometry);
                        break;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
            return retour;
        }

        /// <summary>
        /// 1- Check, if geomOrdinal is not null, it is corresponding to a Geom column type. Throw InvalidCastException if type is not geometry or  IndexOutOfRangeException if not in column's range.
        /// 2- Else, iterate from 0 to end the cursor columns and return as soon as a geometry column is found. If no column exists throw ArgumentOutOfRangeException
        /// Get The first Geometry column searching from beginning or 
        /// </summary>
        /// <param name="pgCursor">a DB Cursor where to search a geometry column. Must be compatible with PostGIS.</param>
        /// <param name="geomOrdinal">if not null, use to convert directly this field into NetTopologyGeometry, otherwise look for column indice one to the end.</param>
        /// <returns></returns>
        private static int GetGeomtryColumnIndex(IFieldValueGetter pgCursor, int? geomOrdinal)
        {
            if (pgCursor != null)
            {
                int cursorFieldCount = pgCursor.FieldCount;
                //1-
                if (geomOrdinal != null)
                {
                    if(geomOrdinal >= 0 && geomOrdinal < cursorFieldCount)
                    {
                        if(IsGeomColumn(pgCursor.GetPostgresType(geomOrdinal.Value)))
                        {
                            return geomOrdinal.Value;
                        }
                        else
                        {
                            throw new InvalidCastException();
                        }
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
                //2-
                else
                {
                    for (int i = 0; i < cursorFieldCount; i++)
                    {
                        if (IsGeomColumn(pgCursor.GetPostgresType(i)))
                        {
                            return i;
                        }
                    }
                    throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
        /// <summary>
        /// Check if column name is POSTGIS_GEOMETRY_TYPE.
        /// </summary>
        /// <param name="type">a PostGres Type</param>
        /// <returns></returns>
        private static bool IsGeomColumn(PostgresType type)
        {
            if(type != null)
            {
                return (type != null && !String.IsNullOrEmpty(type.Name) && type.Name.ToUpper() == POSTGIS_GEOMETRY_TYPE);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// Set the SRID from PostGis to NetTopology equivalent.
        /// Throw InvalidCastException if SRID is out of range.
        /// Throw ArgumentNullException if any of input marameter is null.
        /// </summary>
        /// <param name="netTopologyGeom">the geometry to set the SRID</param>
        /// <param name="postGisGeom">the geometry to get the SRID</param>
        private static void SetSRID(Geometry netTopologyGeom, PostgisGeometry postGisGeom)
        {
            if (postGisGeom != null && netTopologyGeom != null)
            {
                if (postGisGeom.SRID > Int32.MaxValue)
                {
                    throw new InvalidCastException("SRID is out of range value.");
                }
                else
                {
                    netTopologyGeom.SRID = (int)postGisGeom.SRID;
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        private static Geometry ProcessPolygon(PostgisPolygon geometry)
        {
            if (geometry != null)
            {
                int ringCount = geometry.RingCount;
                LinearRing exteriorRing = null;
                LinearRing[] netLRings = null;
                if (ringCount > 0)
                {
                    netLRings = new LinearRing[ringCount -1 ];
                }
                //First is exterior, others are considered as holes
                for (int i = 0; i < ringCount; i++)
                {
                    Coordinate2D[] pgCoords = geometry[i];
                    int coordCounti = pgCoords.Length;
                    Coordinate[] netTopoCoord = new Coordinate[pgCoords.Length];
                    for (int j = 0; j < coordCounti; j++)
                    {
                        netTopoCoord[j] = new Coordinate(pgCoords[j].X, pgCoords[j].Y);
                    }
                    if (i == 0)
                    {
                        exteriorRing = new LinearRing(netTopoCoord);
                    }
                    else
                    {
                        netLRings[i-1] = new LinearRing(netTopoCoord);
                    }
                }
                NetTopologySuite.Geometries.Polygon retour = new NetTopologySuite.Geometries.Polygon(exteriorRing, netLRings);
                return retour;
            }
            else
            {
                throw new ArgumentNullException();
            }

        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        private static Geometry ProcessMultiLineString(PostgisMultiLineString geometry)
        {
            if (geometry != null)
            {
                int lineCount = geometry.LineCount;
                LineString[] lines = new LineString[lineCount];

                for (int i = 0; i < lineCount; i++)
                {
                    Geometry geomi = ProcessLineString(geometry[i]);
                    lines[i] = (LineString)geomi;

                }
                MultiLineString retour = new MultiLineString(lines);
                return retour;
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        private static Geometry ProcessLineString(PostgisLineString geometry)
        {
            if(geometry != null)
            {
                int pointCount = geometry.PointCount;
                Coordinate[] coords = new Coordinate[pointCount];

                for(int i = 0; i < pointCount; i++)
                {
                    Coordinate2D coord2D = geometry[i];
                    coords[i] = new Coordinate();
                    coords[i].X = coord2D.X;
                    coords[i].Y = coord2D.Y;
                }
                LineString retour = new LineString(coords);
                return retour;
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        private static Geometry ProcessMultiPoint(PostgisMultiPoint geometry)
        {
            if (geometry != null)
            {
                List<IPoint> arrpoints = new List<IPoint>();
                int pointCount = geometry.PointCount;
                for (int i = 0; i < pointCount; i++)
                {

                    Coordinate2D iteri = geometry[i];
                    arrpoints.Add(new Point(iteri.X, iteri.Y));
                }
                MultiPoint retour = new MultiPoint(arrpoints.ToArray());
                return retour;
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        private static Geometry ProcessMultiPolygon(PostgisMultiPolygon geometry)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        private static Point ProcessPoint(PostgisPoint geometry)
        {
            if (geometry != null)
            {
                Point retour = new Point(geometry.X, geometry.Y);
                return retour;
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}
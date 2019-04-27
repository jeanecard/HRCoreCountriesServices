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
        /// - InvalidCastException in case of SRID not compatible for example.
        /// - NotImplementedException if no converter is avaialble for this type.
        /// </summary>
        /// <param name="pgCursor">The cursor where to look for the Geometry column</param>
        /// <param name="geomOrdinal">optionnal, geom. column index if known, otherwise will be search for.</param>
        /// <returns></returns>
        static public Geometry ConvertFrom(IFieldValueGetter pgCursor, int? geomOrdinal = null)
        {
            Geometry retour = null;
            if (pgCursor != null)
            {
                PostgisGeometry geometry = pgCursor.GetFieldValue<PostgisGeometry>(GetGeomtryColumnIndex(pgCursor, geomOrdinal));
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
                    throw new  NotImplementedException();
                }
                SetSRID(retour, geometry);
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
        /// From OpenGIS : http://schemas.opengis.net/sf/1.0/simple_features_geometries.rdf
        /// A Polygon is a planar Surface defined by 1 exterior boundary (counterclock) and 0 or more interior boundaries(reverse counterclock). 
        /// Each interior boundary defines a hole in the Polygon. 
        ///     a) Polygons are topologically closed; 
        ///     b) The boundary of a Polygon consists of a set of LinearRings that make up its exterior and interior boundaries; 
        ///     c) No two Rings in the boundary cross and the Rings in the boundary of a Polygon may intersect at a Point but only as a tangent. 
        ///     d) A Polygon may not have cut lines, spikes or punctures. 
        ///     e) The interior of every Polygon is a connected point set; 
        ///     f) The exterior of a Polygon with 1 or more holes is not connected. Each hole defines a connected component of the exterior
        ///     Can throw the following Exceptions :
        ///         - ArgumentNullException
        /// </summary>
        /// <param name="geometry">a PostGisPolygon</param>
        /// <returns>a NetTopology Polygon</returns>
        private static Geometry ProcessPolygon(PostgisPolygon geometry)
        {
            if (geometry != null)
            {
                int ringCount = geometry.RingCount;
                LinearRing exteriorNetTopoRing = null;
                LinearRing[] holeNetTopoRings = null;
                //First is exterior, others are considered as holes
                for (int i = 0; i < ringCount; i++)
                {
                    Coordinate[] netTopoCoord = ConvertCoordinates2D(geometry[i]);
                    if (i == 0)
                    {
                        exteriorNetTopoRing = new LinearRing(netTopoCoord);
                    }
                    else
                    {
                        if(holeNetTopoRings == null)
                        {
                            holeNetTopoRings = new LinearRing[ringCount-1];
                        }
                        holeNetTopoRings[i-1] = new LinearRing(netTopoCoord);
                    }
                }
                Polygon retour = new Polygon(exteriorNetTopoRing, holeNetTopoRings);
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
        /// <param name="postGisCoord"></param>
        /// <returns></returns>
        private static Coordinate ConvertCoordinate2D(Coordinate2D postGisCoord)
        {
            if(postGisCoord != null)
            {
                return new Coordinate(postGisCoord.X, postGisCoord.Y);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="postGisCoords"></param>
        /// <returns></returns>
        private static Coordinate[] ConvertCoordinates2D(Coordinate2D[] postGisCoords)
        {
            if (postGisCoords != null)
            {
                int coordsCount = postGisCoords.Length;
                Coordinate[] retour = null;
                if (coordsCount > 0)
                {
                    retour = new Coordinate[coordsCount];
                    for (int i = 0; i < coordsCount; i++)
                    {
                        retour[i] = ConvertCoordinate2D(postGisCoords[i]);
                    }
                }
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
            if(geometry != null)
            {
                int polygonCount = geometry.PolygonCount;
                MultiPolygon retour = null;
                if (polygonCount > 0)
                {
                    IPolygon[] netTopoPolygons = new Polygon[polygonCount];
                    for (int i = 0; i < polygonCount; i++)
                    {
                        netTopoPolygons[i] = (Polygon)ProcessPolygon(geometry[i]);
                    }
                    retour = new MultiPolygon(netTopoPolygons);
                }
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
        private static Point ProcessPoint(PostgisPoint geometry)
        {
            if (geometry != null)
            {
                return new Point(geometry.X, geometry.Y);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}
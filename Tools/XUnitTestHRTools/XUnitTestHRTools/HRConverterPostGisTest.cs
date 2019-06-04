using GeoAPI.Geometries;
using HRConverters;
using NetTopologySuite.Geometries;
using Npgsql.LegacyPostgis;
using System;
using Xunit;

namespace XUnitPostGis
{
    public class HRConverterPostGisToNetTopologySuiteTest
    {
        private const int _SRID = 4326;
        private const double _Y1 = 2.2;
        private const double _X1 = 2.1;
        private const double _Y2 = 1.2;
        private const double _X2 = 1.1;
        private const double _Y3 = 1.4;
        private const double _X3 = 1.9;
        private const double _Y4 = 4.2;
        private const double _X4 = 4.1;
        private const double _Y5 = 5.2;
        private const double _X5 = 5.1;
        private const double _Y6 = 6.4;
        private const double _X6 = 6.9;

        [Fact]
        public void TestWrongColumnIndexThrowException()
        {
            StubFieldValueGetter valueGetter = new StubFieldValueGetter();
            valueGetter.OrdinalToStub = 0;
            valueGetter.FieldCount = 1;
            int wrongIndex = 42;
            Assert.Throws<ArgumentOutOfRangeException>(() => HRConverterPostGisToNetTopologySuite.ConvertFrom(valueGetter, wrongIndex));
        }
        [Fact]
        public void TestPostGisPointReturnPointWithCoordinatesAndSRS()
        {
            StubFieldValueGetter valueGetter = new StubFieldValueGetter();
            valueGetter.OrdinalToStub = 0;
            valueGetter.FieldCount = 1;
            valueGetter.GeometryToStub = new PostgisPoint(_X1, _Y1);
            valueGetter.GeometryToStub.SRID = _SRID;
            Geometry geomResult = HRConverterPostGisToNetTopologySuite.ConvertFrom(valueGetter);
            Assert.IsType<Point>(geomResult);
            Point pointResult = (Point)geomResult;
            Assert.Equal(_X1, pointResult.X);
            Assert.Equal(_Y1, pointResult.Y);
            Assert.Equal(_SRID, pointResult.SRID);
        }
        [Fact]
        public void TestPostGisMultiPointReturnMultiPointWithCoordinatesAndSRS()
        {
            StubFieldValueGetter valueGetter = new StubFieldValueGetter();
            valueGetter.OrdinalToStub = 0;
            valueGetter.FieldCount = 1;
            Coordinate2D[] coords = new Coordinate2D[] { new Coordinate2D(_X1, _Y1), new Coordinate2D(_X2, _Y2) };
            valueGetter.GeometryToStub = new PostgisMultiPoint(coords);
            valueGetter.GeometryToStub.SRID = _SRID;
            Geometry geomResult = HRConverterPostGisToNetTopologySuite.ConvertFrom(valueGetter);
            Assert.IsType<MultiPoint>(geomResult);
            MultiPoint multiPointResult = (MultiPoint)geomResult;

            Assert.Equal(_X1, multiPointResult.Coordinates[0].X);
            Assert.Equal(_Y1, multiPointResult.Coordinates[0].Y);
            Assert.Equal(_X2, multiPointResult.Coordinates[1].X);
            Assert.Equal(_Y2, multiPointResult.Coordinates[1].Y);
            Assert.Equal(_SRID, multiPointResult.SRID);
        }
        [Fact]
        public void TestPostGisLineStringReturnLineStringWithCoordinatesAndSRS()
        {
            StubFieldValueGetter valueGetter = new StubFieldValueGetter();
            valueGetter.OrdinalToStub = 0;
            valueGetter.FieldCount = 1;
            Coordinate2D[] coords = new Coordinate2D[] { new Coordinate2D(_X1, _Y1), new Coordinate2D(_X2, _Y2), new Coordinate2D(_X3, _Y3) };
            valueGetter.GeometryToStub = new PostgisLineString(coords);
            valueGetter.GeometryToStub.SRID = _SRID;
            Geometry geomResult = HRConverterPostGisToNetTopologySuite.ConvertFrom(valueGetter);
            Assert.NotNull(geomResult);
            Assert.IsType<LineString>(geomResult);
            LineString lineStringResult = (LineString)geomResult;
            Assert.Equal(3, lineStringResult.Coordinates.Length);
            Assert.Equal(_X1, lineStringResult.Coordinates[0].X);
            Assert.Equal(_Y1, lineStringResult.Coordinates[0].Y);
            Assert.Equal(_X2, lineStringResult.Coordinates[1].X);
            Assert.Equal(_Y2, lineStringResult.Coordinates[1].Y);
            Assert.Equal(_X3, lineStringResult.Coordinates[2].X);
            Assert.Equal(_Y3, lineStringResult.Coordinates[2].Y);
            Assert.Equal(_SRID, lineStringResult.SRID);
        }
        [Fact]
        public void TestPostGisMultiLineStringReturnMultiLineStringWithCoordinatesAndSRS()
        {
            StubFieldValueGetter valueGetter = new StubFieldValueGetter();
            valueGetter.OrdinalToStub = 0;
            valueGetter.FieldCount = 1;
            Coordinate2D[] coords1 = new Coordinate2D[] { new Coordinate2D(_X1, _Y1), new Coordinate2D(_X2, _Y2), new Coordinate2D(_X3, _Y3) };
            Coordinate2D[] coords2 = new Coordinate2D[] { new Coordinate2D(_X4, _Y4), new Coordinate2D(_X5, _Y5), new Coordinate2D(_X6, _Y6) };
            PostgisLineString pgLS1 = new PostgisLineString(coords1);
            PostgisLineString pgLS2 = new PostgisLineString(coords2);
            valueGetter.GeometryToStub = new PostgisMultiLineString(new PostgisLineString[] { pgLS1, pgLS2 });
            valueGetter.GeometryToStub.SRID = _SRID;
            Geometry geomResult = HRConverterPostGisToNetTopologySuite.ConvertFrom(valueGetter);
            Assert.NotNull(geomResult);
            Assert.IsType<MultiLineString>(geomResult);
            MultiLineString multiLineStringResult = (MultiLineString)geomResult;
            Assert.Equal(2, multiLineStringResult.NumGeometries);
            LineString ls1 = (LineString)multiLineStringResult[0];
            Assert.Equal(3, ls1.NumPoints);
            LineString ls2 = (LineString)multiLineStringResult[1];
            Assert.Equal(3, ls2.NumPoints);
            Assert.Equal(3, ls1.Coordinates.Length);
            Assert.Equal(_X1, ls1.Coordinates[0].X);
            Assert.Equal(_Y1, ls1.Coordinates[0].Y);
            Assert.Equal(_X2, ls1.Coordinates[1].X);
            Assert.Equal(_Y2, ls1.Coordinates[1].Y);
            Assert.Equal(_X3, ls1.Coordinates[2].X);
            Assert.Equal(_Y3, ls1.Coordinates[2].Y);
            Assert.Equal(_SRID, ls1.SRID);
            Assert.Equal(3, ls2.Coordinates.Length);
            Assert.Equal(_X4, ls2.Coordinates[0].X);
            Assert.Equal(_Y4, ls2.Coordinates[0].Y);
            Assert.Equal(_X5, ls2.Coordinates[1].X);
            Assert.Equal(_Y5, ls2.Coordinates[1].Y);
            Assert.Equal(_X6, ls2.Coordinates[2].X);
            Assert.Equal(_Y6, ls2.Coordinates[2].Y);
            Assert.Equal(_SRID, ls2.SRID);
        }
        [Fact]
        public void TestPostGisPolygonReturnPolygonWithCoordinatesAndSRS()
        {
            StubFieldValueGetter valueGetter = new StubFieldValueGetter();
            valueGetter.OrdinalToStub = 0;
            valueGetter.FieldCount = 1;
            //Polygon with one hole, first element is exterior ring. Declaration with a jagged Array.
            PostgisPolygon postGisPolygon1 = CreatePostGisPolygon(1);
            valueGetter.GeometryToStub = postGisPolygon1;
            valueGetter.GeometryToStub.SRID = _SRID;
            Geometry geomResult = HRConverterPostGisToNetTopologySuite.ConvertFrom(valueGetter);
            Assert.NotNull(geomResult);
            Assert.IsType<Polygon>(geomResult);
            Polygon polygonResult = (Polygon)geomResult;
            Assert.Equal(_SRID, polygonResult.SRID);
            CheckIsPolygonRightConverted(1, polygonResult);
        }
        [Fact]
        public void TestPostGisMultiPolygonReturnMultiPolygonWithCoordinatesAndSRS()
        {
            StubFieldValueGetter valueGetter = new StubFieldValueGetter();
            valueGetter.OrdinalToStub = 0;
            valueGetter.FieldCount = 1;
            PostgisPolygon[] postGisPolygons = new PostgisPolygon[] { CreatePostGisPolygon(1), CreatePostGisPolygon(2) };
            valueGetter.GeometryToStub = new PostgisMultiPolygon(postGisPolygons);
            valueGetter.GeometryToStub.SRID = _SRID;
            Geometry geomResult = HRConverterPostGisToNetTopologySuite.ConvertFrom(valueGetter);
            Assert.NotNull(geomResult);
            Assert.IsType<MultiPolygon>(geomResult);
            MultiPolygon multiPolygonResult = (MultiPolygon)geomResult;
            Assert.Equal(_SRID, multiPolygonResult.SRID);
            Assert.Equal(2, multiPolygonResult.Count);
            Assert.IsType<Polygon>(multiPolygonResult[0]);
            Assert.IsType<Polygon>(multiPolygonResult[1]);
            CheckIsPolygonRightConverted(1, (Polygon)multiPolygonResult[0]);
            CheckIsPolygonRightConverted(2, (Polygon)multiPolygonResult[1]);
        }
         private void CheckIsPolygonRightConverted(int polygonTestIndex, Polygon polygon)
        {
            if (polygonTestIndex == 1)
            {
                Assert.NotNull(polygon.Holes);
                Assert.Single(polygon.Holes);
                Assert.NotNull(polygon.Holes[0].Coordinates);
                Assert.Equal(1, polygon.Holes[0].Coordinates[0].X);
                Assert.Equal(1, polygon.Holes[0].Coordinates[0].Y);
                Assert.Equal(2, polygon.Holes[0].Coordinates[2].X);
                Assert.Equal(2, polygon.Holes[0].Coordinates[2].Y);
            }
            else
            {
                Assert.NotNull(polygon.Holes);
                Assert.Single(polygon.Holes);
                Assert.NotNull(polygon.Holes[0].Coordinates);
                Assert.Equal(51, polygon.Holes[0].Coordinates[1].X);
                Assert.Equal(50, polygon.Holes[0].Coordinates[1].Y);
                Assert.Equal(50, polygon.Holes[0].Coordinates[3].X);
                Assert.Equal(50, polygon.Holes[0].Coordinates[3].Y);
            }
        }

        private PostgisPolygon CreatePostGisPolygon(int id)
        {
            Coordinate2D[][] pgCoords = new Coordinate2D[2][];
            if (id == 1)
            {
                pgCoords[0] = new Coordinate2D[] {
                new Coordinate2D(0, 0),
                new Coordinate2D(10, 0),
                new Coordinate2D(10, 10),
                new Coordinate2D(0, 10),
                new Coordinate2D(0, 0)
                };
                pgCoords[1] = new Coordinate2D[] {
                new Coordinate2D(1, 1),
                new Coordinate2D(2, 1),
                new Coordinate2D(2, 2),
                new Coordinate2D(1, 1)
                };
            }
            else
            {
                pgCoords[0] = new Coordinate2D[] {
                new Coordinate2D(10, 10),
                new Coordinate2D(100, 10),
                new Coordinate2D(100, 100),
                new Coordinate2D(10, 100),
                new Coordinate2D(10, 10)
                };
                pgCoords[1] = new Coordinate2D[] {
                new Coordinate2D(50, 50),
                new Coordinate2D(51, 50),
                new Coordinate2D(51, 52),
                new Coordinate2D(50, 50)
                };

            }
            return new PostgisPolygon(pgCoords);
        }

    }
}

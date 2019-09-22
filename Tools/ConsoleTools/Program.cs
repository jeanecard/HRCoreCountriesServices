using HRConverters;
using HRCoreBordersModel;
using NetTopologySuite.Geometries;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace ConsoleTools
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ConnectToPostGis2();
            Console.ReadKey();
        }

        //private static async void ConnectToPostGis()
        //{
        //    var connString = "ToGetFromAppConfigFileDO";
        //    using (var conn = new NpgsqlConnection(connString))
        //    {

        //        conn.Open();
        //        conn.TypeMapper.UseLegacyPostgis();
        //        // Retrieve all rows
        //        using (var cmd = new NpgsqlCommand("SELECT name, wkb_geometry FROM boundaries", conn))
        //        using (Task<DbDataReader> readerTask = cmd.ExecuteReaderAsync())
        //        {
        //            await readerTask;

        //            NpgsqlDataReader reader = (NpgsqlDataReader)readerTask.Result;
        //            PostGisFieldValueGetter readerFacade = new PostGisFieldValueGetter(reader);
        //            Task<bool> reading = reader.ReadAsync();
        //            await reading;
        //            while (reading.Result)
        //            {
        //                Geometry geo = HRConverterPostGisToNetTopologySuite.ConvertFrom(readerFacade);
        //                reading.Dispose();
        //                reading = reader.ReadAsync();
        //            }
        //            reading.Dispose();
        //            reader.Close();
        //            readerTask.Dispose();
        //        }
        //    }
        //    Console.ReadKey();
        //}

        private static void ConnectToPostGis2()
        {
            Console.ReadKey();
        }
    }
}

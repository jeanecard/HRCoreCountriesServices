using HRConverters;
using HRCoreBordersModel;
using HRCoreBordersRepository;
using HRCoreBordersServices;
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
            int[] saucisse = new int[0];
            int length = saucisse.Length;
            //ReadAllCountriesFromMongoDB();
            ConnectToPostGis2();
            Console.ReadKey();
        }

        private static void ImportAllCountriesInMongoDB()
        {
            //using (StreamReader file = File.OpenText(@"../Assets/allCountries.json"))
            //{
            //    Console.WriteLine("Fichier lu OK");
            //    HRCountry[] countries = HRCountry.FromJson(file.ReadToEnd());
            //    if (countries != null)
            //    {
            //        String connectionString = "ToGetFromAppConfig";
            //        MongoClient client = new MongoClient(connectionString);
            //        var database = client.GetDatabase("HRMongoDBCluster");
            //        IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("Countries");
            //        int countriesCount = countries.Length;
            //        for (int i = 0; i < countriesCount; i++)
            //        {
            //            collection.InsertOne(countries[i].ToBsonDocument());
            //        }
            //        Console.WriteLine("Jusque ici tout va bien");
            //        try
            //        {
            //            Console.WriteLine("Nombre de document en base : " + collection.EstimatedDocumentCount().ToString());
            //            var documentReceived = collection.Find(new BsonDocument()).FirstOrDefault();
            //            Console.WriteLine("Doc. Lu : " + documentReceived.ToString());
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine(ex.Message);
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        private static void ReadAllCountriesFromMongoDB()
        {
            //String connectionString = "ToGetFromAppConfigFile";

            //try
            //{
            //    MongoClient client = new MongoClient(connectionString);
            //    var database = client.GetDatabase("HRMongoDBCluster");
            //    IMongoCollection<HRCountry> collection = database.GetCollection<HRCountry>("Countries");
            //    FilterDefinitionBuilder<HRCountry> bld = new FilterDefinitionBuilder<HRCountry>();
            //    List<HRCountry> retour = collection.Find(bld.Empty).ToList();
            //    foreach (HRCountry iterator in retour)
            //    {
            //        Console.WriteLine(iterator.Name);
            //    }

            //}
            //catch (Exception ex)
            //{

            //    Console.WriteLine(ex.Message);
            //}
        }

        private static async void ConnectToPostGis()
        {
            List<HRBorder> retour = new List<HRBorder>();
            var connString = "ToGetFromAppConfigFileDO";
            using (var conn = new NpgsqlConnection(connString))
            {

                conn.Open();
                conn.TypeMapper.UseLegacyPostgis();
                // Retrieve all rows
                using (var cmd = new NpgsqlCommand("SELECT name, wkb_geometry FROM boundaries", conn))
                using (Task<DbDataReader> readerTask = cmd.ExecuteReaderAsync())
                {
                    await readerTask;

                    NpgsqlDataReader reader = (NpgsqlDataReader)readerTask.Result;
                    PostGisFieldValueGetter readerFacade = new PostGisFieldValueGetter(reader);
                    Task<bool> reading = reader.ReadAsync();
                    await reading;
                    while (reading.Result)
                    {
                        Geometry geo = HRConverterPostGisToNetTopologySuite.ConvertFrom(readerFacade);
                        reading = reader.ReadAsync();
                    }
                }
            }
            Console.ReadKey();
        }

        private static async void ConnectToPostGis2()
        {
            HRCoreBordersService _borderService = new HRCoreBordersService(new CoreBordersRepository(null));
            Task<IEnumerable<HRBorder>> bordersAction = _borderService.GetBordersAsync();
            await bordersAction;
            IEnumerable<HRBorder> t = bordersAction.Result;

            Console.ReadKey();

        }
    }
}

using System;
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Converters;
using System.Collections.Generic;
using QuickType;
using Npgsql;
using GeoJSON.Net.Geometry;

namespace Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            //ReadAllCountriesFromMongoDB();
            ConnectToPostGis();
        }
        static void ImportAllCountriesInMongoDB()
        {
            using (StreamReader file = File.OpenText(@"../Assets/allCountries.json"))
            {
                Console.WriteLine("Fichier lu OK");
                HRCountry[] countries = HRCountry.FromJson(file.ReadToEnd());
                if (countries != null)
                {
                    String connectionString = "mongodb+srv://hradmin:C%40mben71@hrmongodbcluster-wtmqq.gcp.mongodb.net/test?retryWrites=true";
                    MongoClient client = new MongoClient(connectionString);
                    var database = client.GetDatabase("HRMongoDBCluster");
                    IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("Countries");
                    int countriesCount = countries.Length;
                    for (int i = 0; i < countriesCount; i++)
                    {
                        collection.InsertOne(countries[i].ToBsonDocument());
                    }
                    Console.WriteLine("Jusque ici tout va bien");
                    try
                    {
                        Console.WriteLine("Nombre de document en base : " + collection.EstimatedDocumentCount().ToString());
                        var documentReceived = collection.Find(new BsonDocument()).FirstOrDefault();
                        Console.WriteLine("Doc. Lu : " + documentReceived.ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        static void ReadAllCountriesFromMongoDB()
        {
            String connectionString = "mongodb+srv://hradmin:C%40mben71@hrmongodbcluster-wtmqq.gcp.mongodb.net/test?retryWrites=true";

            try
            {
                MongoClient client = new MongoClient(connectionString);
                var database = client.GetDatabase("HRMongoDBCluster");
                IMongoCollection<HRCountry> collection = database.GetCollection<HRCountry>("Countries");
                FilterDefinitionBuilder<HRCountry> bld = new FilterDefinitionBuilder<HRCountry>();
                List<HRCountry> retour = collection.Find(bld.Empty).ToList();
                foreach (HRCountry iterator in retour)
                {
                    Console.WriteLine(iterator.Name);
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
        static void ConnectToPostGis()
        {

            var connString = "host = db.qgiscloud.com; Username = gxxawt_obddnf; Password = 8d2b58e2; Database = gxxawt_obddnf";
            //Host = myserver; Username = mylogin; Password = mypass; Database = mydatabase
            using (var conn = new NpgsqlConnection(connString))
            {

                conn.Open();
                //conn.TypeMapper.UseLegacyPostgis();
                // Retrieve all rows
                using (var cmd = new NpgsqlCommand("SELECT name, wkb_geometry FROM boundaries", conn))
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        IGeometryObject geoNetGeom = HRConverterPostGisToGeoJsonNet.ConvertFrom(reader);
                    }
            }
            Console.ReadKey();
        }
    }
}



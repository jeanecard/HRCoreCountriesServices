using System;
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Converters;
using System.Collections.Generic;
using QuickType;

namespace Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadAllCountriesFromMongoDB();
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
            MongoClient client = new MongoClient(connectionString);
            var database = client.GetDatabase("HRMongoDBCluster");
            IMongoCollection<HRCountry> collection = database.GetCollection<HRCountry>("Countries");
            try
            {
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
    }
}



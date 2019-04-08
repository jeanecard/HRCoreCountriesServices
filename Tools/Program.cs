using System;
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Converters;

namespace Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            String connectionString = "mongodb+srv://hradmin:C%40mben71@hrmongodbcluster-wtmqq.gcp.mongodb.net/test?retryWrites=true";
            MongoClient client = new MongoClient(connectionString);
            ImportJsonCountry(client);
            Console.WriteLine("Connexion OK");
            GeoJsonConverter truc = new GeoJsonConverter();

            // read JSON directly from a file
            using (StreamReader file = File.OpenText(@"../Assets/Boundaries.geojson"))
            {
                Console.WriteLine("Fichier lu OK");

                FeatureCollection featureCollection = JsonConvert.DeserializeObject<FeatureCollection>(file.ReadToEnd());
                Console.WriteLine("Taille: " + featureCollection.Features.Count.ToString());
                int i = 4;
                i++;
            }
        }

        private static void ImportJsonCountry(MongoClient mongoClient)
        {
            //     var database = mongoClient.GetDatabase("HRMongoDBCluster");
            //     IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("Countries");
            //     string inputFileName = @"C:\CODE\NET CORE\HRCoreCountriesServices\Tools\allCountries.json"; // initialize to the input file
            //     using (var streamReader = new StreamReader(inputFileName))
            //     {
            //         Console.WriteLine("Jusque ici tout va bien");
            //         try
            //         {
            //             String result = streamReader.ReadToEnd();
            //             var jsonReader = new JsonReader(result);

            //             var context = BsonDeserializationContext.CreateRoot(jsonReader);
            //             //var context = roger.CreateRoot(streamReader);
            //             //var document = null;//collection.DocumentSerializer.Deserialize(result);
            //             //collection.InsertOne(document);
            //             Console.WriteLine("Ah ?");
            //         }
            //         catch (Exception ex)
            //         {
            //             Console.WriteLine(ex.Message);
            //         }
            //         string line;
            //         // while ((line = await streamReader.ReadLineAsync()) != null)
            //         // {
            //         //     Console.WriteLine(line);
            //         // using (var jsonReader = new JsonReader(line))
            //         // {
            //         //     var context = BsonDeserializationContext.CreateRoot(jsonReader);
            //         //     var document = collection.DocumentSerializer.Deserialize(context);
            //         //     await collection.InsertOneAsync(document);
            //         // }
            //         //}
            //     }
        }
    }
}

using Databases2.EF;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Databases2.Mongo
{
    class Mongoz
    {
        private IMongoDatabase db;
        private IMongoCollection<MongoFilmSerie> collection;
        private IMongoCollection<BsonDocument> bsonCollection;
        private readonly FilmSerie fs;
        private readonly FilmSerie newFs;
        private readonly long totalMs;

        public Mongoz(int[] rowsArray, FilmSerie fs, FilmSerie newFs)
        {
            Init("Netflax");

            this.fs = fs;
            this.newFs = newFs;

            int idCounter = 0;
            totalMs = 0;

            foreach (int rows in rowsArray)
            {
                Console.WriteLine("For {0} rows: ", rows);

                long result = Insert(rows);
                totalMs += result;
                Console.Write("\rInsert: ");
                Console.WriteLine(result + " milisecons");

                result = Select(rows);
                totalMs += result;
                Console.Write("\rSelect: ");
                Console.WriteLine(result + " milisecons");

                result = Update(rows);
                totalMs += result;
                Console.Write("\rupdate: ");
                Console.WriteLine(result + " milisecons");

                result = Delete(rows);
                totalMs += result;
                Console.Write("\rDelete: ");
                Console.WriteLine(result + " milisecons");

                idCounter += rows;
            }

            Console.WriteLine();
            Console.WriteLine("Total of {0}ms", totalMs);

            // Running the garbage collector because working with mongo db doesnt do that???
            GC.Collect();
        }

        public long GetResult()
        {
            return totalMs;
        }


        private void Init(string database)
        {
            var client = new MongoClient();
            try
            {
                client.DropDatabase(database);
                Console.WriteLine("Replacing mongo db...");
            }
            catch
            {
                Console.WriteLine("Creating new mongo db...");
            }

            db = client.GetDatabase(database);

            db.CreateCollection("Films/Series");
            db.CreateCollection("Videos");
            db.CreateCollection("Seasons");
            db.CreateCollection("Episodes");

            collection = db.GetCollection<MongoFilmSerie>("Films/Series");
            bsonCollection = db.GetCollection<BsonDocument>("Films/Series");
        }

        private long Insert(int rows)
        {
            var watch = Stopwatch.StartNew();

            Console.Write("Inserting");

            List<MongoFilmSerie> toAdd = new List<MongoFilmSerie>();

            for (int i = 0; i < rows; i++)
            {
                toAdd.Add(new MongoFilmSerie(fs));
            }

            collection.InsertMany(toAdd);

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        private long Select(int rows)
        {
            var watch = Stopwatch.StartNew();

            Console.Write("Selecting");

            var set = collection.Find(x => true).Limit(rows).ToList();

            // Do something with the data
            for (int i = 0; i < set.Count; i++)
            {
                MongoFilmSerie filmSerie = set[i];
                Console.Write("\rSelecting {0}", filmSerie.Title + i);
            }

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
        private long Update(int rows)
        {

            var watch = Stopwatch.StartNew();

            Console.Write("Updating");

            var update = Builders<BsonDocument>.Update
                .Set("Title", newFs.Title)
                .Set("Description", newFs.Descritption)
                .Set("Type", newFs.Type);

            var docs = bsonCollection.Find(x => true).Limit(rows).ToList();

            foreach (var doc in docs)
            {
                bsonCollection.UpdateOne(doc, update);
            }

            //bsonCollection.UpdateMany(GetFilter(rows), update);


            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        private long Delete(int rows)
        {

            var watch = Stopwatch.StartNew();

            Console.Write("Deleting");

            var docs = bsonCollection.Find(x => true).Limit(rows).ToList();

            foreach (var doc in docs)
            {
                bsonCollection.DeleteOne(doc);
            }

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
    }
}

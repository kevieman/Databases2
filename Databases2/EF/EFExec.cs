using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Databases2.EF
{
    class EFExec
    {
        private FilmSerie filmSerie;
        private FilmSerie newFilmSerie;
        private readonly long totalMs;
        public EFExec(int[] rowsArray, FilmSerie filmSerie, FilmSerie newFilmSerie)
        {
            this.filmSerie = filmSerie;
            this.newFilmSerie = newFilmSerie;

            int idCounter = 0;
            totalMs = 0;

            using (NetflaxContext dbContext = new NetflaxContext())
            {
                // Saves the database before testing
                dbContext.Genre.Add(new Genre() { Name = "name" });
                dbContext.SaveChanges();
            }

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

                    result = Update(rows, idCounter);
                    totalMs += result;
                    Console.Write("\rUpdate: ");
                    Console.WriteLine(result + " milisecons");

                    result = Delete(rows, idCounter);
                    totalMs += result;
                    Console.Write("\rDelete: ");
                    Console.WriteLine(result + " milisecons");

                    idCounter += rows;
                }

            Console.WriteLine();
            Console.WriteLine("Total of {0}ms", totalMs);
        }

        public long GetResult()
        {
            return totalMs;
        }

        private long Insert(int rows)
        {
            var watch = Stopwatch.StartNew();

            Console.Write("Inserting");

            List<FilmSerie> toAdd = new List<FilmSerie>();

            for (int i = 0; i < rows; i++)
            {
                Console.Write("\r Inserting {0}", i);
                toAdd.Add(new FilmSerie()
                {
                    Title = filmSerie.Title,
                    Type = filmSerie.Type,
                    Descritption = filmSerie.Descritption
                });
            }

            // Insert steps of 1000
            int startRange = 0;
            int endRange = 1000;

            for (int i = 0; i < rows; i += endRange)
            {
                using (NetflaxContext dbContext = new NetflaxContext())
                {
                    if (endRange > toAdd.Count)
                        endRange = toAdd.Count;

                    dbContext.FilmsSeries.AddRange(toAdd.GetRange(startRange, endRange));
                    startRange = endRange;
                    endRange += endRange;

                    dbContext.SaveChanges();
                }

            }

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        private long Select(int rows)
        {
            var watch = Stopwatch.StartNew();

            using (NetflaxContext dbContext = new NetflaxContext())
            {
                var set = dbContext.FilmsSeries;

                Console.Write("Selecting");

                // Do something with the data
                foreach (FilmSerie filmSerie in set)
                {
                    FilmSerie filmSerie1 = filmSerie;
                    int percent = (int)((filmSerie.ID / rows) * 100);
                    Console.Write("\rSelecting {0}%", percent);
                }
            }

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        private long Update(int rows, int idHelper)
        {
            var watch = Stopwatch.StartNew();

            Console.Write("Updating");

            using (NetflaxContext dbContext = new NetflaxContext())
            {

                var FilmsSeries = dbContext.FilmsSeries;

                foreach (FilmSerie filmSerie in FilmsSeries)
                {
                    filmSerie.Title = newFilmSerie.Title;
                    filmSerie.Type = newFilmSerie.Type;
                    filmSerie.Descritption = newFilmSerie.Descritption;
                }

                dbContext.SaveChanges();
            }


            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        private long Delete(int rows, int idHelper)
        {
            var watch = Stopwatch.StartNew();
            Console.Write("Deleting");
            using (NetflaxContext dbContext = new NetflaxContext())
            {
                var toRemove = dbContext.FilmsSeries.ToList();

                dbContext.FilmsSeries.RemoveRange(toRemove);

                dbContext.SaveChanges();

            }

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
    }
}

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
        public EFExec(FilmSerie filmSerie, FilmSerie newFilmSerie)
        {
            this.filmSerie = filmSerie;
            this.newFilmSerie = newFilmSerie;

            int[] rowsArray = { 10000 };
            //int[] rowsArray = {100};
            int idCounter = 0;
            long totalMs = 0;

            using (NetflaxContext dbContext = new NetflaxContext())
            {
                // Saves the database before testing
                dbContext.Genre.Add(new Genre() { Name = "name" });
                dbContext.SaveChanges();

                foreach (int rows in rowsArray)
                {
                    Console.WriteLine("For {0} rows: ", rows);

                    long result = Insert(rows, dbContext);
                    totalMs += result;
                    Console.Write("\rInsert: ");
                    Console.WriteLine(result + " milisecons");

                    result = Select(dbContext, rows);
                    totalMs += result;
                    Console.Write("\rSelect: ");
                    Console.WriteLine(result + " milisecons");

                    result = Update(rows, dbContext, idCounter);
                    totalMs += result;
                    Console.Write("\rUpdate: ");
                    Console.WriteLine(result + " milisecons");

                    result = Delete(rows, dbContext, idCounter);
                    totalMs += result;
                    Console.Write("\rDelete: ");
                    Console.WriteLine(result + " milisecons");

                    idCounter += rows;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Total of {0}ms", totalMs);
        }

        public long Insert(int rows, NetflaxContext dbContext)
        {
            var watch = Stopwatch.StartNew();

            Console.Write("Inserting");

            IList<FilmSerie> toAdd = new List<FilmSerie>();

            for (int i = 0; i < rows; i++)
            {
                toAdd.Add(new FilmSerie()
                {
                    Title = filmSerie.Title,
                    Type = filmSerie.Type,
                    Descritption = filmSerie.Descritption
                });
            }

            dbContext.FilmsSeries.AddRange(toAdd);

            dbContext.SaveChanges();

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        public long Select(NetflaxContext dbContext, int rows)
        {
            var watch = Stopwatch.StartNew();

            var set = dbContext.FilmsSeries;

            Console.Write("Selecting");

            foreach(FilmSerie filmSerie in set)
            {
                FilmSerie filmSerie1 = filmSerie;
                int percent = (int)((filmSerie.ID / rows) * 100 );
                Console.Write("\rSelecting {0}%", percent);
            }

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        public long Update(int rows, NetflaxContext dbContext, int idHelper)
        {
            var watch = Stopwatch.StartNew();

            Console.Write("Updating");

            var FilmsSeries = dbContext.FilmsSeries;

            foreach (FilmSerie filmSerie in FilmsSeries)
            {
                filmSerie.Title = newFilmSerie.Title;
                filmSerie.Type = newFilmSerie.Type;
                filmSerie.Descritption = newFilmSerie.Descritption;
            }

            dbContext.SaveChanges();


            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        public long Delete(int rows, NetflaxContext dbContext, int idHelper)
        {
            var watch = Stopwatch.StartNew();
            Console.Write("Deleting");

            var FilmsSeries = dbContext.FilmsSeries;

            dbContext.FilmsSeries.RemoveRange(FilmsSeries);

            dbContext.SaveChanges();

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
    }
}

using Databases2.ADO;
using Databases2.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

namespace Databases2
{
    class ADOExec
    {
        private readonly string connectionString;
        private readonly FilmSerie filmSerie;
        private readonly FilmSerie newFilmSerie;
        private readonly long totalMs;

        public ADOExec(int[] rowsArray, FilmSerie filmSerie, FilmSerie newFilmSerie)
        {
            ADOInit init = new ADOInit();
            init.Initialize();

            this.connectionString = init.GetDbConnString();
            this.filmSerie = filmSerie;
            this.newFilmSerie = newFilmSerie;

            int idCounter = 0;
            totalMs = 0;

            foreach (int rows in rowsArray)
            {
                Console.WriteLine("For {0} rows: ", rows);

                long result = InsertInFilmsSeries(rows);
                totalMs += result;
                Console.Write("Insert: ");
                Console.WriteLine(result + "ms");

                result = SelectFilmsSeries(rows);
                totalMs += result;
                Console.Write("Select: ");
                Console.WriteLine(result + "ms");

                result = UpdateFilmsSeries(rows, idCounter);
                totalMs += result;
                Console.Write("Update: ");
                Console.WriteLine(result + "ms");

                result = DeleteFilmsSeries(rows, idCounter);
                totalMs += result;
                Console.Write("Delete: ");
                Console.WriteLine(result+ "ms");

                idCounter += rows;
            }

            Console.WriteLine();
            Console.WriteLine("Total of {0}ms", totalMs);

        }

        public long GetResult()
        {
            return totalMs;
        }

        // Executes insert statements in the Profiles table returns the time it took in ms
        private long InsertInFilmsSeries(int rows)
        {
            var watch = Stopwatch.StartNew();

            string query = "INSERT INTO [dbo].[Films/Series]" +
            "([Title] ,[Type],[Description])" +
            "VALUES" +
            "(@title, @type, @desc)";

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                for (int i = 0; i < rows; i++)
                {

                    SqlCommand command = new SqlCommand(query, conn);

                    command.Parameters.AddWithValue("@title", filmSerie.Title);
                    command.Parameters.AddWithValue("@type", filmSerie.Type);
                    command.Parameters.AddWithValue("@desc", filmSerie.Descritption);

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine();
                        Console.WriteLine(e.Message);
                    }
                }

                conn.Close();
            }

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        private long SelectFilmsSeries(int rows)
        {
            var watch = Stopwatch.StartNew();

            string query = "SELECT TOP(@rows) * FROM [dbo].[Films/Series]";

            using(var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@rows", rows);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    Console.WriteLine();
                    Console.WriteLine(e.Message);
                }

                conn.Close();
            }

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        private long UpdateFilmsSeries(int rows, int idHelper)
        {
            var watch = Stopwatch.StartNew();

            using(var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "UPDATE [dbo].[Films/Series]" +
                    "SET [Title] = '@title'" +
                    ",[Type] = @type" +
                    ",[Description] = '@desc'" +
                    "WHERE ID = @id";

                for (int i = 1; i <= rows; i++)
                {

                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@title", newFilmSerie.Title);
                    command.Parameters.AddWithValue("@type", newFilmSerie.Type);
                    command.Parameters.AddWithValue("@desc", newFilmSerie.Descritption);
                    command.Parameters.AddWithValue("@id", i + idHelper);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine();
                        Console.WriteLine(e.Message);
                    }
                }

                conn.Close();
            }

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        private long DeleteFilmsSeries(int rows, int idHelper)
        {
            var watch = Stopwatch.StartNew();

            using(var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "DELETE FROM [dbo].[Films/Series] WHERE ID = @id";

                for (int i = 1; i <= rows; i++)
                {
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@id", i + idHelper);

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine();
                        Console.WriteLine(e.Message);
                    }
                }

                conn.Close();
            }

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
    }

    
}

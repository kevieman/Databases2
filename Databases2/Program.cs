using Databases2.EF;
using Databases2.Mongo;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace Databases2
{
    class Program
    {
        // This contians the data which will be inserted.
        private readonly FilmSerie filmSerie = new FilmSerie()
        {
            Title = "A Title",
            Type = "Film",
            Descritption = "A description"
        };

        // This contains the data which will be updated.
        private readonly FilmSerie newFilmSerie = new FilmSerie()
        {
            Title = "A new Title",
            Type = "Serie",
            Descritption = "A new description"
        };

        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            // Which amount of rows should be tested.
            int[] rowsArray = { 1, 1000, 100000, 1000000 };


            bool looper = true;
            while (looper)
            {
                Console.Write("> ");
                string command = Console.ReadLine();

                switch (command)
                {
                    case "exit":
                        looper = false;
                        break;
                    case "ado":
                        Console.WriteLine("Starting the Ado test");
                        new ADOExec(rowsArray, filmSerie, newFilmSerie);
                        break;
                    case "ef":
                        Console.WriteLine("Starting the EF test");
                        new EFExec(rowsArray, filmSerie, newFilmSerie);
                        break;
                    case "mongo":
                        new Mongoz(rowsArray, filmSerie, newFilmSerie);
                        break;
                    case "all":
                        Console.WriteLine("U sure? (Y/N)");
                        string answer = Console.ReadLine();
                        if (!answer.Equals("Y"))
                            break;

                        Console.WriteLine("Ok lets go");
                        Console.WriteLine("ADO-------------------------------------");
                        ADOExec ado = new ADOExec(rowsArray, filmSerie, newFilmSerie);
                        Console.WriteLine();
                        Console.WriteLine("EF--------------------------------------");
                        EFExec ef = new EFExec(rowsArray, filmSerie, newFilmSerie);
                        Console.WriteLine();
                        Console.WriteLine("Mongo------------------------------------");
                        Mongoz mongo = new Mongoz(rowsArray, filmSerie, newFilmSerie);

                        Console.WriteLine();
                        Console.WriteLine("Ado took:  {0}", ado.GetResult());
                        Console.WriteLine("EF took:  {0}", ef.GetResult());
                        Console.WriteLine("MongoDB took:  {0}", mongo.GetResult());

                        break;
                    default:
                        Console.WriteLine("Dont know that command bro");
                        break;
                }
            }
        }

        
    }
}

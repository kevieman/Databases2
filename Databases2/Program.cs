using Databases2.EF;
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
        FilmSerie filmSerie = new FilmSerie()
        {
            Title = "A Title",
            Type = "Film",
            Descritption = "A description"
        };

        // This contains the data which will be updated.
        FilmSerie newFilmSerie = new FilmSerie()
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
                        new ADOExec(filmSerie, newFilmSerie);
                        break;
                    case "ef":
                        Console.WriteLine("Starting the EF test");
                        new EFExec(filmSerie, newFilmSerie);
                        break;
                    default:
                        Console.WriteLine("Dont know that command bro");
                        break;
                }
            }
        }

        
    }
}

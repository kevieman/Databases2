using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Databases2
{
    class NetflaxContext : DbContext
    {
        public DbSet<FilmSerie> FilmsSeries { get; set; }

        public NetflaxContext() : base("NetflaxContext")
        {

        }
    }

    public class FilmSerie
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Descritption { get; set; }
    }
}

using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Databases2.Mongo
{
    [BsonIgnoreExtraElements]
    class MongoFilmSerie
    {
        public string Title;
        public string Description;
        public string Type;

        public MongoFilmSerie(EF.FilmSerie fs)
        {
            this.Title = fs.Title;
            this.Description = fs.Descritption;
            this.Type = fs.Type;
        }
    }
}

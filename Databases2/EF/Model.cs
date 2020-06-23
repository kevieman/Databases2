using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Databases2
{
    class NetflaxContext : DbContext
    {
        public DbSet<FilmSerie> FilmsSeries { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Episode> Episode { get; set; }
        public DbSet<Film_Has_Video> Film_Has_Video { get; set; }
        public DbSet<Quality> Quality { get; set; }
        public DbSet<Video> Video { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Viewer_Guide> Viewer_Guide { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<Subscription> Subscription { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Blocked_Account> Blocked_Account { get; set; }
        public DbSet<Profile> Profile { get; set; }
        public DbSet<Profile_Seen_Videos> Profile_Seen_Videos { get; set; }
        public DbSet<Profile_Preference> Profile_Preference { get; set; }

        public NetflaxContext() : base("NetflaxContext")
        {
            Database.SetInitializer(
                new DropCreateDatabaseAlways<NetflaxContext>()
            );
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // This coinstraints the foreign key of language in profiles.
            modelBuilder.Entity<Profile>()
                .HasOptional<Language>(s => s.Language)
                .WithMany()
                .WillCascadeOnDelete(false);
        }
    }

    public class FilmSerie
    {
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        public string Descritption { get; set; }
        public string Type { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Viewer_Guide> Viewer_Guides { get; set; }
        public List<Profile> InQueue { get; set; }
    }

    public class Season
    {
        public int ID { get; set; }
        public string SeasonTitle { get; set; }
        public List<Episode> Episodes { get; set; }
        [Required]
        public FilmSerie Serie { get; set; }
    }

    public class Episode
    {
        public int ID { get; set; }
        public string Title { get; set; }
        [Required]
        public Video Video { get; set; }
    }

    public class Film_Has_Video
    {
        public int ID { get; set; }
        public FilmSerie Film { get; set; }
        public Video Video { get; set; }
    }

    public class Quality
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Resolution { get; set; }
    }

    public class Video
    {
        public int ID { get; set; }
        [Required]
        public string File { get; set; }
        [Required]
        public int Length { get; set; }
        public int Credits_Start { get; set; }
        [Required]
        public Quality Quality { get; set; }
        public List<Subtitle> Subtitles { get; set; }
    }

    public class Genre
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public List<FilmSerie> FilmsSeries { get; set; }
        public List<Profile_Preference> Profile_Preferences { get; set; }
    }

    public class Viewer_Guide
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public List<FilmSerie> FilmsSeries { get; set; }
        public List<Profile_Preference> Profile_Preferences { get; set; }

    }

    public class Language
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Abbrivation { get; set; }
    }

    public class Subscription
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public Quality Quality { get; set; }
    }

    public class Subtitle
    {
        public int ID { get; set; }
        [Required]
        public string File { get; set; }
        [Required]
        public Language Language { get; set; }
        [Required]
        public Video Video { get; set; }
    }

    public class Account
    {
        public int ID { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool Activated { get; set; }
        public bool Had_Discount { get; set; }
        public int Trial_Days_Left { get; set; }
        [Required]
        public Language Language { get; set; }
        public Subscription Subscription { get; set; }
        public Account RelatedAccount { get; set; }
    }

    public class Blocked_Account
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public bool Currently_Blocked { get; set; }
        public List<Profile> Profiles { get; set; }
    }

    public class Profile
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public int Age { get; set; }
        public string ProfilePicture { get; set; }
        [Required]
        public Language Language { get; set; }
        [Required]
        public Account Account { get; set; }
        public List<FilmSerie> Queue { get; set; }
        public List<Profile_Seen_Videos> Profile_Seen_Videos { get; set; }
        public Profile_Preference Profile_Preference { get; set; }
    }

    public class Profile_Seen_Videos
    {
        public int ID { get; set; }
        [Required]
        public bool Currently_Watching { get; set; }
        public DateTime Date { get; set; }
        public int Paused_Timestamp { get; set; }
        [Required]
        public Profile Profile { get; set; }
        [Required]
        public Video Video { get; set; }
        public Subtitle Subtitle { get; set; }
    }

    public class Profile_Preference
    {
        public int ID { get; set; }
        public bool InterestedInFilms { get; set; }
        public bool InterestedInSeries { get; set; }
        [Required]
        public Language Language { get; set; }
        [Required]
        public Profile Profile { get; set; }
        public List<Viewer_Guide> Viewer_Guides { get; set; }
        public List<Genre> Genres { get; set; }
    }

}

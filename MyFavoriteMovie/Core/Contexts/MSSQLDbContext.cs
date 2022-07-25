using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contexts
{
    public class MSSQLDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<Actor> Actors { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Episode> Episodes { get; set; } = null!;
        public DbSet<MovieRate> MovieRates { get; set; } = null!;
        public DbSet<ActorAward> ActorAwards { get; set; } = null!;
        public DbSet<MovieAward> MovieAwards { get; set; } = null!;
        public DbSet<Account> Users { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;

        public MSSQLDbContext() : base() { }
        public MSSQLDbContext(DbContextOptions<MSSQLDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureMovieRelationship(modelBuilder);
            ConfigureUserRelationship(modelBuilder);
            ConfigureActorRelationship(modelBuilder);
        }

        private void ConfigureMovieRelationship(ModelBuilder modelBuilder)
        {
            // Movies - Actors
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Actors)
                .WithMany(a => a.ActorsInMovie)
                .UsingEntity(t => t.ToTable("MovieActor"));

            // Movies - MovieAwards
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Awards)
                .WithMany(a => a.MovieHolder)
                .UsingEntity(t => t.ToTable("MovieHolderAwards"));

            // Movies - Genres
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Genres)
                .WithMany(g => g.Movies)
                .UsingEntity(t => t.ToTable("MovieGenre"));

            // Movies - MovieRate
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.MovieRates)
                .WithOne(r => r.Movie)
                .HasForeignKey(r => r.Id);

            // Movies - Episode
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Episodes)
                .WithOne(e => e.Movie)
                .HasForeignKey(e => e.Id);

            // Movies - Revie
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Reviews)
                .WithOne(r => r.Movie)
                .HasForeignKey(r => r.Id);

            // Movies - DirectedBy
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.DirectedBy)
                .WithMany(a => a.DirectorsInMovie)
                .UsingEntity(t => t.ToTable("MovieDirectedBy"));
        }

        private void ConfigureUserRelationship(ModelBuilder modelBuilder)
        {
            // Accounts - Movies
            modelBuilder.Entity<Account>()
                .HasMany(u => u.FavoriteMovies)
                .WithMany(m => m.AccountsFavorite)
                .UsingEntity(t => t.ToTable("UserFavoriteMovie"));

            // Accounts - Actors
            modelBuilder.Entity<Account>()
                .HasMany(u => u.FavoriteActors)
                .WithMany(a => a.AccountsFavorite)
                .UsingEntity(t => t.ToTable("UserFavoriteActor"));

            // Accounts - Rewiew
            modelBuilder.Entity<Account>()
                .HasMany(u => u.Reviews)
                .WithOne(r => r.Author)
                .HasForeignKey(r => r.Id);

            // Accounts - MovieRete
            modelBuilder.Entity<Account>()
                .HasMany(u => u.MovieRates)
                .WithOne(r => r.Account)
                .HasForeignKey(r => r.Id);
        }

        private void ConfigureActorRelationship(ModelBuilder modelBuilder)
        {
            // Actors - ActorAwards
            modelBuilder.Entity<Actor>()
                .HasMany(ac => ac.Awards)
                .WithMany(aw => aw.ActorHolder)
                .UsingEntity(t => t.ToTable("ActorHolderAwards"));
        }
    }
}

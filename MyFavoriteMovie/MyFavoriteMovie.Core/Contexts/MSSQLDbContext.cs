using Microsoft.EntityFrameworkCore;
using MyFavoriteMovie.Core.Models;

namespace MyFavoriteMovie.Core.Contexts
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
        public DbSet<User> Users { get; set; } = null!;
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
                .HasForeignKey(r => r.MovieId);

            // Movies - Episode
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Episodes)
                .WithOne(e => e.Movie)
                .HasForeignKey(e => e.MovieId);

            // Movies - Revie
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Reviews)
                .WithOne(r => r.Movie)
                .HasForeignKey(r => r.MovieId);

            // Movies - DirectedBy
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.DirectedBy)
                .WithMany(a => a.DirectorsInMovie)
                .UsingEntity(t => t.ToTable("MovieDirectedBy"));

            // Movies - Images
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Images)
                .WithOne(i => i.Movie)
                .HasForeignKey(i => i.MovieId);
        }

        private void ConfigureUserRelationship(ModelBuilder modelBuilder)
        {
            // Accounts - Movies
            modelBuilder.Entity<User>()
                .HasMany(u => u.FavoriteMovies)
                .WithMany(m => m.UsersFavorite)
                .UsingEntity(t => t.ToTable("UserFavoriteMovie"));

            // Accounts - Actors
            modelBuilder.Entity<User>()
                .HasMany(u => u.FavoriteActors)
                .WithMany(a => a.UsersFavorite)
                .UsingEntity(t => t.ToTable("UserFavoriteActor"));

            // Accounts - Rewiew
            modelBuilder.Entity<User>()
                .HasMany(u => u.Reviews)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.AuthorId);

            // Accounts - MovieRate
            modelBuilder.Entity<User>()
                .HasMany(u => u.MovieRates)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.AccountId);
        }

        private void ConfigureActorRelationship(ModelBuilder modelBuilder)
        {
            // Actors - ActorAwards
            modelBuilder.Entity<Actor>()
                .HasMany(ac => ac.Awards)
                .WithMany(aw => aw.ActorHolder)
                .UsingEntity(t => t.ToTable("ActorHolderAwards"));

            // Actors - Images
            modelBuilder.Entity<Actor>()
                .HasMany(ac => ac.Images)
                .WithOne(i => i.Actor)
                .HasForeignKey(i => i.ActorId);
        }
    }
}

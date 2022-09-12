﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyFavoriteMovie.Core.Contexts;

#nullable disable

namespace MyFavoriteMovie.Core.Migrations
{
    [DbContext(typeof(MSSQLDbContext))]
    partial class MSSQLDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AccountActor", b =>
                {
                    b.Property<int>("AccountsFavoriteId")
                        .HasColumnType("int");

                    b.Property<int>("FavoriteActorsId")
                        .HasColumnType("int");

                    b.HasKey("AccountsFavoriteId", "FavoriteActorsId");

                    b.HasIndex("FavoriteActorsId");

                    b.ToTable("UserFavoriteActor", (string)null);
                });

            modelBuilder.Entity("AccountMovie", b =>
                {
                    b.Property<int>("AccountsFavoriteId")
                        .HasColumnType("int");

                    b.Property<int>("FavoriteMoviesId")
                        .HasColumnType("int");

                    b.HasKey("AccountsFavoriteId", "FavoriteMoviesId");

                    b.HasIndex("FavoriteMoviesId");

                    b.ToTable("UserFavoriteMovie", (string)null);
                });

            modelBuilder.Entity("ActorActorAward", b =>
                {
                    b.Property<int>("ActorHolderId")
                        .HasColumnType("int");

                    b.Property<int>("AwardsId")
                        .HasColumnType("int");

                    b.HasKey("ActorHolderId", "AwardsId");

                    b.HasIndex("AwardsId");

                    b.ToTable("ActorHolderAwards", (string)null);
                });

            modelBuilder.Entity("ActorMovie", b =>
                {
                    b.Property<int>("ActorsId")
                        .HasColumnType("int");

                    b.Property<int>("ActorsInMovieId")
                        .HasColumnType("int");

                    b.HasKey("ActorsId", "ActorsInMovieId");

                    b.HasIndex("ActorsInMovieId");

                    b.ToTable("MovieActor", (string)null);
                });

            modelBuilder.Entity("ActorMovie1", b =>
                {
                    b.Property<int>("DirectedById")
                        .HasColumnType("int");

                    b.Property<int>("DirectorsInMovieId")
                        .HasColumnType("int");

                    b.HasKey("DirectedById", "DirectorsInMovieId");

                    b.HasIndex("DirectorsInMovieId");

                    b.ToTable("MovieDirectedBy", (string)null);
                });

            modelBuilder.Entity("GenreMovie", b =>
                {
                    b.Property<int>("GenresId")
                        .HasColumnType("int");

                    b.Property<int>("MoviesId")
                        .HasColumnType("int");

                    b.HasKey("GenresId", "MoviesId");

                    b.HasIndex("MoviesId");

                    b.ToTable("MovieGenre", (string)null);
                });

            modelBuilder.Entity("MovieMovieAward", b =>
                {
                    b.Property<int>("AwardsId")
                        .HasColumnType("int");

                    b.Property<int>("MovieHolderId")
                        .HasColumnType("int");

                    b.HasKey("AwardsId", "MovieHolderId");

                    b.HasIndex("MovieHolderId");

                    b.ToTable("MovieHolderAwards", (string)null);
                });

            modelBuilder.Entity("MyFavoriteMovie.Core.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MyFavoriteMovie.Core.Models.Actor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AvatarImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeathDate")
                        .HasColumnType("datetime2");

                    b.Property<double?>("Height")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Actors");
                });

            modelBuilder.Entity("MyFavoriteMovie.Core.Models.ActorAward", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("AwardDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("AwardType")
                        .HasColumnType("int");

                    b.Property<string>("NominationName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("ActorAwards");
                });

            modelBuilder.Entity("MyFavoriteMovie.Core.Models.ActorImage", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ActorImage");
                });

            modelBuilder.Entity("MyFavoriteMovie.Core.Models.Episode", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<TimeSpan?>("Duration")
                        .HasColumnType("time");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("RealeseDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Season")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Episodes");
                });

            modelBuilder.Entity("MyFavoriteMovie.Core.Models.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("MyFavoriteMovie.Core.Models.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<TimeSpan?>("Duration")
                        .HasColumnType("time");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Poster")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("MyFavoriteMovie.Core.Models.MovieAward", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("AwardDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("AwardType")
                        .HasColumnType("int");

                    b.Property<string>("NominationName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("MovieAwards");
                });

            modelBuilder.Entity("MyFavoriteMovie.Core.Models.MovieImage", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MovieImage");
                });

            modelBuilder.Entity("MyFavoriteMovie.Core.Models.MovieRate", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<byte>("Rate")
                        .HasMaxLength(10)
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.ToTable("MovieRates");
                });

            modelBuilder.Entity("MyFavoriteMovie.Core.Models.Review", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("ReviewType")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(3000)
                        .HasColumnType("nvarchar(3000)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("AccountActor", b =>
                {
                    b.HasOne("MyFavoriteMovie.Core.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountsFavoriteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyFavoriteMovie.Core.Models.Actor", null)
                        .WithMany()
                        .HasForeignKey("FavoriteActorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AccountMovie", b =>
                {
                    b.HasOne("MyFavoriteMovie.Core.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountsFavoriteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyFavoriteMovie.Core.Models.Movie", null)
                        .WithMany()
                        .HasForeignKey("FavoriteMoviesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ActorActorAward", b =>
                {
                    b.HasOne("MyFavoriteMovie.Core.Models.Actor", null)
                        .WithMany()
                        .HasForeignKey("ActorHolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyFavoriteMovie.Core.Models.ActorAward", null)
                        .WithMany()
                        .HasForeignKey("AwardsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ActorMovie", b =>
                {
                    b.HasOne("MyFavoriteMovie.Core.Models.Actor", null)
                        .WithMany()
                        .HasForeignKey("ActorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyFavoriteMovie.Core.Models.Movie", null)
                        .WithMany()
                        .HasForeignKey("ActorsInMovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ActorMovie1", b =>
                {
                    b.HasOne("MyFavoriteMovie.Core.Models.Actor", null)
                        .WithMany()
                        .HasForeignKey("DirectedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyFavoriteMovie.Core.Models.Movie", null)
                        .WithMany()
                        .HasForeignKey("DirectorsInMovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GenreMovie", b =>
                {
                    b.HasOne("MyFavoriteMovie.Core.Models.Genre", null)
                        .WithMany()
                        .HasForeignKey("GenresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyFavoriteMovie.Core.Models.Movie", null)
                        .WithMany()
                        .HasForeignKey("MoviesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MovieMovieAward", b =>
                {
                    b.HasOne("MyFavoriteMovie.Core.Models.MovieAward", null)
                        .WithMany()
                        .HasForeignKey("AwardsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyFavoriteMovie.Core.Models.Movie", null)
                        .WithMany()
                        .HasForeignKey("MovieHolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyFavoriteMovie.Core.Models.ActorImage", b =>
                {
                    b.HasOne("MyFavoriteMovie.Core.Models.Actor", "Actor")
                        .WithMany("Images")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Actor");
                });

            modelBuilder.Entity("MyFavoriteMovie.Core.Models.Episode", b =>
                {
                    b.HasOne("MyFavoriteMovie.Core.Models.Movie", "Movie")
                        .WithMany("Episodes")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("MyFavoriteMovie.Core.Models.MovieImage", b =>
                {
                    b.HasOne("MyFavoriteMovie.Core.Models.Movie", "Movie")
                        .WithMany("Images")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("MyFavoriteMovie.Core.Models.MovieRate", b =>
                {
                    b.HasOne("MyFavoriteMovie.Core.Models.Account", "Account")
                        .WithMany("MovieRates")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyFavoriteMovie.Core.Models.Movie", "Movie")
                        .WithMany("MovieRates")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("MyFavoriteMovie.Core.Models.Review", b =>
                {
                    b.HasOne("MyFavoriteMovie.Core.Models.Account", "Author")
                        .WithMany("Reviews")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyFavoriteMovie.Core.Models.Movie", "Movie")
                        .WithMany("Reviews")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("MyFavoriteMovie.Core.Models.Account", b =>
                {
                    b.Navigation("MovieRates");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("MyFavoriteMovie.Core.Models.Actor", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("MyFavoriteMovie.Core.Models.Movie", b =>
                {
                    b.Navigation("Episodes");

                    b.Navigation("Images");

                    b.Navigation("MovieRates");

                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}

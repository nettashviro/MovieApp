﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieApp.Data;

namespace MovieApp.Migrations
{
    [DbContext(typeof(MovieAppContext))]
    [Migration("20200831071910_FIX-KEY")]
    partial class FIXKEY
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MovieApp.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("ProfileImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("MovieApp.Models.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<int>("Genre")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Language")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MovieClickedId")
                        .HasColumnType("int");

                    b.Property<int>("MovieIdInTMDB")
                        .HasColumnType("int");

                    b.Property<int?>("MovieWatchedId")
                        .HasColumnType("int");

                    b.Property<int?>("MovieWatchlistId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.Property<string>("TrailerUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MovieClickedId");

                    b.HasIndex("MovieWatchedId");

                    b.HasIndex("MovieWatchlistId");

                    b.ToTable("Movie");
                });

            modelBuilder.Entity("MovieApp.Models.Official", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OriginCountry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Official");
                });

            modelBuilder.Entity("MovieApp.Models.OfficialOfMovie", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<int>("OfficialId")
                        .HasColumnType("int");

                    b.HasKey("MovieId", "OfficialId");

                    b.HasIndex("OfficialId");

                    b.ToTable("OfficialOfMovie");
                });

            modelBuilder.Entity("MovieApp.Models.Soundtrack", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Duration")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PerformerId")
                        .HasColumnType("int");

                    b.Property<string>("SoundtrackUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("WriterId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PerformerId");

                    b.HasIndex("WriterId");

                    b.ToTable("Soundtrack");
                });

            modelBuilder.Entity("MovieApp.Models.SoundtrackOfMovie", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<int>("SoundtrackId")
                        .HasColumnType("int");

                    b.HasKey("MovieId", "SoundtrackId");

                    b.HasIndex("SoundtrackId");

                    b.ToTable("SoundtrackOfMovie");
                });

            modelBuilder.Entity("MovieApp.Models.Tweet", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Author")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<long>("TweetId")
                        .HasColumnType("bigint");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Tweet");
                });

            modelBuilder.Entity("MovieApp.Models.Movie", b =>
                {
                    b.HasOne("MovieApp.Models.Account", null)
                        .WithMany("MovieClicked")
                        .HasForeignKey("MovieClickedId");

                    b.HasOne("MovieApp.Models.Account", null)
                        .WithMany("MovieWatched")
                        .HasForeignKey("MovieWatchedId");

                    b.HasOne("MovieApp.Models.Account", null)
                        .WithMany("MovieWatchlist")
                        .HasForeignKey("MovieWatchlistId");
                });

            modelBuilder.Entity("MovieApp.Models.OfficialOfMovie", b =>
                {
                    b.HasOne("MovieApp.Models.Movie", "Movie")
                        .WithMany("OfficialOfMovies")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MovieApp.Models.Official", "Official")
                        .WithMany("OfficialOfMovies")
                        .HasForeignKey("OfficialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MovieApp.Models.Soundtrack", b =>
                {
                    b.HasOne("MovieApp.Models.Official", "Performer")
                        .WithMany()
                        .HasForeignKey("PerformerId");

                    b.HasOne("MovieApp.Models.Official", "Writer")
                        .WithMany()
                        .HasForeignKey("WriterId");
                });

            modelBuilder.Entity("MovieApp.Models.SoundtrackOfMovie", b =>
                {
                    b.HasOne("MovieApp.Models.Movie", "Movie")
                        .WithMany("SoundtracksOfMovie")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MovieApp.Models.Soundtrack", "Soundtrack")
                        .WithMany("SoundtrackOfMovies")
                        .HasForeignKey("SoundtrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

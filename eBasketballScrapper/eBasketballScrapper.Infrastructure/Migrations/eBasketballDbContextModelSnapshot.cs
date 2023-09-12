﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eBasketballScrapper.Infrastructure.Data;

#nullable disable

namespace eBasketballScrapper.Infrastructure.Migrations
{
    [DbContext(typeof(eBasketballDbContext))]
    partial class eBasketballDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("eBasketballScrapper.Core.Entities.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("MatchDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("PlayerA")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PlayerB")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ScoreA")
                        .HasColumnType("int");

                    b.Property<int>("ScoreB")
                        .HasColumnType("int");

                    b.Property<string>("TeamA")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("TeamB")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Matches");
                });
#pragma warning restore 612, 618
        }
    }
}

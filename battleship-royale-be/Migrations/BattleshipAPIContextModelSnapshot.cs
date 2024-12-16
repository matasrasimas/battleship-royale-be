﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using battleship_royale_be.Data;

#nullable disable

namespace battleship_royale_be.Migrations
{
    [DbContext(typeof(BattleshipAPIContext))]
    partial class BattleshipAPIContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("battleship_royale_be.DesignPatterns.Memento.Caretaker", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PlayerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Caretakers");
                });

            modelBuilder.Entity("battleship_royale_be.Models.Cell", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Col")
                        .HasColumnType("int");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsHit")
                        .HasColumnType("bit");

                    b.Property<bool>("IsIsland")
                        .HasColumnType("bit");

                    b.Property<bool>("IsShip")
                        .HasColumnType("bit");

                    b.Property<Guid?>("PlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Row")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("Cells");
                });

            modelBuilder.Entity("battleship_royale_be.Models.Coordinates", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Col")
                        .HasColumnType("int");

                    b.Property<int>("Row")
                        .HasColumnType("int");

                    b.Property<Guid?>("ShipId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ShipId");

                    b.ToTable("Coordinates");
                });

            modelBuilder.Entity("battleship_royale_be.Models.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ShotResultMessage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("battleship_royale_be.Models.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConnectionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("GameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("GameStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsYourTurn")
                        .HasColumnType("bit");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("battleship_royale_be.Models.Ship", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("CanMove")
                        .HasColumnType("bit");

                    b.Property<int>("HitPoints")
                        .HasColumnType("int");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsHorizontal")
                        .HasColumnType("bit");

                    b.Property<Guid?>("PlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("Ships");
                });

            modelBuilder.Entity("battleship_royale_be.Models.UserConnection", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("GameId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserConnections");
                });

            modelBuilder.Entity("battleship_royale_be.Models.Cell", b =>
                {
                    b.HasOne("battleship_royale_be.Models.Player", null)
                        .WithMany("Cells")
                        .HasForeignKey("PlayerId");
                });

            modelBuilder.Entity("battleship_royale_be.Models.Coordinates", b =>
                {
                    b.HasOne("battleship_royale_be.Models.Ship", null)
                        .WithMany("Coordinates")
                        .HasForeignKey("ShipId");
                });

            modelBuilder.Entity("battleship_royale_be.Models.Player", b =>
                {
                    b.HasOne("battleship_royale_be.Models.Game", null)
                        .WithMany("Players")
                        .HasForeignKey("GameId");
                });

            modelBuilder.Entity("battleship_royale_be.Models.Ship", b =>
                {
                    b.HasOne("battleship_royale_be.Models.Player", null)
                        .WithMany("Ships")
                        .HasForeignKey("PlayerId");
                });

            modelBuilder.Entity("battleship_royale_be.Models.Game", b =>
                {
                    b.Navigation("Players");
                });

            modelBuilder.Entity("battleship_royale_be.Models.Player", b =>
                {
                    b.Navigation("Cells");

                    b.Navigation("Ships");
                });

            modelBuilder.Entity("battleship_royale_be.Models.Ship", b =>
                {
                    b.Navigation("Coordinates");
                });
#pragma warning restore 612, 618
        }
    }
}

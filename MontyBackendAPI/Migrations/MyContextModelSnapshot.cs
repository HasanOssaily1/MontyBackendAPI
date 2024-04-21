﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MontyBackendAPI.Migrations
{
    [DbContext(typeof(MyContext))]
    partial class MyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.29")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MontyBackendAPI.Models.Subscriptions", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<DateTime?>("creationdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("enddate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("modificationdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("startdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("title")
                        .HasColumnType("text");

                    b.Property<string>("type")
                        .HasColumnType("text");

                    b.Property<int>("userid")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("MontyBackendAPI.Models.Users", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<DateTime?>("creationdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("email")
                        .HasColumnType("text");

                    b.Property<DateTime>("modificationdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.Property<string>("password")
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}

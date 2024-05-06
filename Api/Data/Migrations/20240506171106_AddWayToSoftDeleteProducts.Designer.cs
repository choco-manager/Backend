﻿// <auto-generated />
using System;
using Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Api.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240506171106_AddWayToSoftDeleteProducts")]
    partial class AddWayToSoftDeleteProducts
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Api.Data.Models.FcmToken", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("FcmTokens");
                });

            modelBuilder.Entity("Api.Data.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("CostPrice")
                        .HasColumnType("numeric");

                    b.Property<bool>("IsBulk")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<decimal>("RetailPrice")
                        .HasColumnType("numeric");

                    b.Property<int>("StockBalance")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Api.Data.Models.ProductTag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.HasKey("Id");

                    b.ToTable("ProductTags");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ca398e3e-b307-4fb1-897d-0a879e07ed0a"),
                            Title = "Молочный"
                        },
                        new
                        {
                            Id = new Guid("3f9c5572-2a30-4738-ad5d-e64a78d0b1c5"),
                            Title = "Горький"
                        },
                        new
                        {
                            Id = new Guid("8983ea6f-d617-4b68-bf35-951e987b6df2"),
                            Title = "Фирменный"
                        },
                        new
                        {
                            Id = new Guid("1cb55e22-b608-4eb1-9b00-bcf99227a45b"),
                            Title = "Мармелад"
                        },
                        new
                        {
                            Id = new Guid("800ce8c6-cda8-4a33-8232-94d1039fdcbf"),
                            Title = "На развес"
                        },
                        new
                        {
                            Id = new Guid("3916b26d-cb71-459d-9692-2c8376ed3222"),
                            Title = "Брикет"
                        },
                        new
                        {
                            Id = new Guid("740c0d16-cff1-4fc2-9b32-198c41cd8f5b"),
                            Title = "Конфеты"
                        },
                        new
                        {
                            Id = new Guid("66978e41-810b-4e15-b271-0185365319ea"),
                            Title = "Паста"
                        },
                        new
                        {
                            Id = new Guid("6010b624-5e50-4c74-aedd-62d86968b8aa"),
                            Title = "С орехами"
                        });
                });

            modelBuilder.Entity("Api.Data.Models.RefreshToken", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ExpireAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("Api.Data.Models.RevokedAccessToken", b =>
                {
                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.HasKey("Token");

                    b.ToTable("RevokedAccessTokens");
                });

            modelBuilder.Entity("Api.Data.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ProductProductTag", b =>
                {
                    b.Property<Guid>("ProductsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TagsId")
                        .HasColumnType("uuid");

                    b.HasKey("ProductsId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("ProductProductTag");
                });

            modelBuilder.Entity("ProductProductTag", b =>
                {
                    b.HasOne("Api.Data.Models.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Models.ProductTag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

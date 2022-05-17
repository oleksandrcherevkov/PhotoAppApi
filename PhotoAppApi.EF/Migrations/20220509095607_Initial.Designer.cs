﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PhotoAppApi.EF;

#nullable disable

namespace PhotoAppApi.EF.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220509095607_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("PhotoAppApi.EF.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("PhotoAppApi.EF.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<string>("CreatorLogin")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Photo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorLogin");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("PhotoAppApi.EF.Models.User", b =>
                {
                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EMail")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Login");

                    b.HasIndex("EMail")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PostUser", b =>
                {
                    b.Property<string>("LikedLogin")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("LikesId")
                        .HasColumnType("int");

                    b.HasKey("LikedLogin", "LikesId");

                    b.HasIndex("LikesId");

                    b.ToTable("PostUser");
                });

            modelBuilder.Entity("PhotoAppApi.EF.Models.Post", b =>
                {
                    b.HasOne("PhotoAppApi.EF.Models.User", "Creator")
                        .WithMany("Posts")
                        .HasForeignKey("CreatorLogin");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("PostUser", b =>
                {
                    b.HasOne("PhotoAppApi.EF.Models.User", null)
                        .WithMany()
                        .HasForeignKey("LikedLogin")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PhotoAppApi.EF.Models.Post", null)
                        .WithMany()
                        .HasForeignKey("LikesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PhotoAppApi.EF.Models.User", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}

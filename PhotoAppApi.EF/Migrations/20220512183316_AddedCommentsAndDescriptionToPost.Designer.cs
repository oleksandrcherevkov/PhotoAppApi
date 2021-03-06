// <auto-generated />
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
    [Migration("20220512183316_AddedCommentsAndDescriptionToPost")]
    partial class AddedCommentsAndDescriptionToPost
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

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PostId")
                        .IsUnique();

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("PhotoAppApi.EF.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("PhotoAppApi.EF.Models.PostComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CreatorLogin")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorLogin");

                    b.HasIndex("PostId");

                    b.ToTable("PostComment");
                });

            modelBuilder.Entity("PhotoAppApi.EF.Models.User", b =>
                {
                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Confirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Login");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

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

            modelBuilder.Entity("PhotoAppApi.EF.Models.Photo", b =>
                {
                    b.HasOne("PhotoAppApi.EF.Models.Post", null)
                        .WithOne("Photo")
                        .HasForeignKey("PhotoAppApi.EF.Models.Photo", "PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PhotoAppApi.EF.Models.Post", b =>
                {
                    b.HasOne("PhotoAppApi.EF.Models.User", "Creator")
                        .WithMany("Posts")
                        .HasForeignKey("CreatorId");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("PhotoAppApi.EF.Models.PostComment", b =>
                {
                    b.HasOne("PhotoAppApi.EF.Models.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorLogin")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("PhotoAppApi.EF.Models.Post", null)
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

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

            modelBuilder.Entity("PhotoAppApi.EF.Models.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Photo");
                });

            modelBuilder.Entity("PhotoAppApi.EF.Models.User", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}

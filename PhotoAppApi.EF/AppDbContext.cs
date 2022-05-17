using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PhotoAppApi.EF.Models;

namespace PhotoAppApi.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User>        Users { get; set; }
        public DbSet<Avatar>      Avatars { get; set; }
        public DbSet<Post>        Posts { get; set; }
        public DbSet<PostPhoto>   PostPhotos { get; set; }
        public DbSet<PostComment> PostComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Avatar)
                .WithOne()
                .HasForeignKey<Avatar>(a => a.UserLogin);
                
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Creator)
                .WithMany(u => u.Posts);

            modelBuilder.Entity<PostComment>()
                .HasOne(c => c.Creator)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PostUserView>()
                .HasKey(v => new { v.PostId, v.UserLogin });

            modelBuilder.Entity<PostUserView>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(a => a.UserLogin);
        }
    }

    //public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    //{
    //    public AppDbContext CreateDbContext(string[] args)
    //    {
    //        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
    //        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=PhotoAppDb;Trusted_Connection=True;");

    //        return new AppDbContext(optionsBuilder.Options);
    //    }
    //}
}

using AppCore.DataAccess.Configs;
using Entities.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityFramework.Contexts
{
   public class ECicekSepetiContext : DbContext
    {

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           // ConnectionConfig.ConnectionString = "server=.\\SQLEXPRESS;database=ECicekSepetiDB;user id=sa;password=telefon.123;multipleactiveresultsets=true;";
            optionsBuilder.UseSqlServer(ConnectionConfig.ConnectionString);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                //.ToTable("Urunler")
                
                .HasOne(product => product.Category)
                .WithMany(category => category.Products)
                .HasForeignKey(product => product.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<City>()
                
                .HasOne(city => city.Country)
                .WithMany(country => country.Cities)
                .HasForeignKey(city => city.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                
                .HasOne(user => user.Role)
                .WithMany(role => role.Users)
                .HasForeignKey(user => user.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserDetail>()
                
                .HasOne(userDetail => userDetail.Country)
                .WithMany(country => country.UserDetail)
                .HasForeignKey(userDetail => userDetail.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserDetail>()
                .HasOne(userDetail => userDetail.City)
                .WithMany(city => city.UserDetail)
                .HasForeignKey(userDetail => userDetail.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasOne(user => user.UserDetail)
                .WithOne(userDetail => userDetail.User)
                .HasForeignKey<User>(user => user.UserDetailId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserDetail>()
                .HasIndex(userDetail => userDetail.EMail)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(product => product.Name);

           
        }
    }

    }


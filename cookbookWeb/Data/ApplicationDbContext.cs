using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using cookbookWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace cookbookWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, long, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<FavoriteRecipe> FavoriteRecipes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Category> Categories { get; set; }
        //public DbSet<Role> Roles { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options){}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql("Host=localhost;Database=cookbookWeb;Username=postgres;Password=postgres");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<UserToken>().ToTable("UserTokens");
            modelBuilder.Entity<RoleClaim>().ToTable("RoleClaims");

            modelBuilder.Entity<Recipe>().ToTable("Recipes");
            modelBuilder.Entity<FavoriteRecipe>().ToTable("FavoriteRecipes");
            modelBuilder.Entity<Instruction>().ToTable("Instructions");
            modelBuilder.Entity<Ingredient>().ToTable("Ingredients");
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Comment>().ToTable("Comments");
        }

    }
}

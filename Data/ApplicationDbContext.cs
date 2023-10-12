using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tommava.Models;

namespace Tommava.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Video> Video { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<TimeLineVideo> TimeLineVideo { get; set; }
        public DbSet<Bank> Bank { get; set; }
        public DbSet<VipPackage> VipPackage { get; set; }
        
    }
}

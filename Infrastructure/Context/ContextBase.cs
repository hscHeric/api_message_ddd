using Entities.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class ContextBase : IdentityDbContext<ApplicationUser>
    {
        public ContextBase(DbContextOptions<ContextBase> options) : base(options)
        {
        }

        public DbSet<Message> Message { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=Heric\\SQLEXPRESS; Initial Catalog=DbMessageAPI_DDD; Integrated Security=True");
                base.OnConfiguring(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().ToTable("AspNetUsers").HasKey(t => t.Id);

            base.OnModelCreating(builder);
        }
    }
}

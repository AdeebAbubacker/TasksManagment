using Microsoft.EntityFrameworkCore;
using TaskManagement.Infrastructure;

namespace TasManagment.Tests.Infrastructure
{
    public class TestDbContext : TaskManagmentDbContext
    {
        public TestDbContext(DbContextOptions<TaskManagmentDbContext> options)
            : base(options)
        {
        }

        // REGISTER TEST ENTITY
        public DbSet<TestEntity> TestEntities { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TestEntity>().HasKey(x => x.Id);
        }
    }
}

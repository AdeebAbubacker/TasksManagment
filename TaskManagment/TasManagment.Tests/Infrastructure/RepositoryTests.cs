using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Infrastructure;
using TaskManagement.Infrastructure.Repositories;
using TasManagment.Tests.Infrastructure;

namespace TaskManagement.Tests.Infrastructure.Repositories
{
    [TestClass]
    public class RepositoryTests
    {
        private TestDbContext context = null!;
        private Repository<TestEntity> repository = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TaskManagmentDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            context = new TestDbContext(options);
            context.Database.EnsureCreated();

            repository = new Repository<TestEntity>(context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [TestMethod]
        public async Task Add_AddsEntity()
        {
            var entity = new TestEntity { Name = "Test" };

            await repository.Add(entity);
            await context.SaveChangesAsync();

            Assert.AreEqual(1, context.TestEntities.Count());
        }

        [TestMethod]
        public async Task Update_UpdatesEntity()
        {
            var entity = new TestEntity { Name = "Old" };
            context.TestEntities.Add(entity);
            await context.SaveChangesAsync();

            entity.Name = "Updated";

            await repository.Update(entity);
            await context.SaveChangesAsync();

            var updated = await repository.GetById(entity.Id);
            Assert.AreEqual("Updated", updated!.Name);
        }

        [TestMethod]
        public async Task Delete_RemovesEntity()
        {
            var entity = new TestEntity { Name = "To Delete" };
            context.TestEntities.Add(entity);
            await context.SaveChangesAsync();

            await repository.Delete(entity);
            await context.SaveChangesAsync();

            Assert.AreEqual(0, context.TestEntities.Count());
        }

    }
}

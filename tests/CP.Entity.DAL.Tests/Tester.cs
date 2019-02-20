using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CP.Entity.DAL.Tests
{
    public class Tester<TDbContext> 
        where TDbContext : DbContext
    {
        protected Tester()
        {
        }
        [OneTimeSetUp]
        public virtual void Init()
        {
            //var optionsBuilder = new DbContextOptionsBuilder<TDbContext>();

            //optionsBuilder.UseInMemoryDatabase();

      //      Options= CreateNewContextOptions();
        }

        public DbContextOptions<TDbContext> Options { get; set; }
        //protected static DbContextOptions<TDbContext> CreateNewContextOptions()
        //{
        //    // Create a fresh service provider, and therefore a fresh 
        //    // InMemory database instance.
        //    var serviceProvider = new ServiceCollection()
        //        .AddEntityFrameworkInMemoryDatabase()
        //        .BuildServiceProvider();

        //    // Create a new options instance telling the context to use an
        //    // InMemory database and the new service provider.
        //    var builder = new DbContextOptionsBuilder<TDbContext>();
        //    builder.UseInMemoryDatabase()
        //           .UseInternalServiceProvider(serviceProvider);
        //    return builder.Options;
        //}

    }
}

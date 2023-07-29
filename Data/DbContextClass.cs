using GraphQLTest.Entities;
using Microsoft.EntityFrameworkCore;

namespace GraphQLTest.Data
{
    public class DbContextClass : DbContext
    {
        public DbContextClass(DbContextOptions<DbContextClass> options) : base(options)
        {

        }

        public DbSet<ProductDetails> Products { get; set; }
    }
}

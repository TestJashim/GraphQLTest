global using GraphQLTest.Data;
global using GraphQLTest.GraphQL.MutationTypes;
global using GraphQLTest.Repositories;
global using Microsoft.EntityFrameworkCore;
using GraphQLTest.GraphQL.QueryTypes;

namespace GraphQLTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //Register Service
            builder.Services.AddScoped<IProductService, ProductService>();

            //InMemory Database
            builder.Services.AddDbContext<DbContextClass>
            (o => o.UseInMemoryDatabase("GraphQLTest"));

            //GraphQL Config
            builder.Services.AddGraphQLServer()
                .AddQueryType<ProductQueryTypes>()
                .AddMutationType<ProductMutations>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            //Seed Data
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<DbContextClass>();

                SeedData.Initialize(services);
            }

            //GraphQL
            app.MapGraphQL();

            app.UseHttpsRedirection();
            app.Run();
        }
    }
}
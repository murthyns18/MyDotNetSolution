
using LMS_API.Interfaces;
using LMS_API.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace WEBAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();


            //Swagger Configuration
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //CORS configuration
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("DefaultCorsPolicy", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            //DB connection 
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            //DI

            //Book
            builder.Services.AddScoped<IBookRepository>( m => new BookRepository(connectionString));
            //Category
            builder.Services.AddScoped<ICategoryRepository>(m => new CategoryRepository(connectionString));
            //Publisher
            builder.Services.AddScoped<IPublisherRepository>(m => new PublisherRepository(connectionString));
            //User
            builder.Services.AddScoped<IUserRepository>(m => new UserRepository(connectionString));
            //Role
            builder.Services.AddScoped<IRoleRepository>(m => new RoleRepository(connectionString));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("DefaultCorsPolicy");


            // Redirect root to Swagger
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    context.Response.Redirect("/swagger");
                    return;
                }
                await next();
            });

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

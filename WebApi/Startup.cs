using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Data;
using WebApi.Services;

namespace WebApi
{
    public class Startup
    {

        private const string SWAGGER_ENDPOINT = "/swagger/v1/swagger.json";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // Enable API versioning
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                //options.AssumeDefaultVersionWhenUnspecified = true;
            });
            // Enable EF
            services.AddDbContext<ProductsDbContext>(option =>
                option.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            // Resolves the depencencies to always created
            services.AddScoped<IProduct, ProductRepository>();
            // Register the swagger generator
            services.AddSwaggerGen(swagger =>
                swagger.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "New API",
                    Version = "v1"
                })
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ProductsDbContext productsDbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // Enable middleware to serve generate Swagger as a JSON endpoint
            app.UseSwagger();
            // Enable the middleware to serve swagger-ui
            app.UseSwaggerUI(swagger =>
            {
                swagger.SwaggerEndpoint(SWAGGER_ENDPOINT, "My API v1");
                // Use the swagger app's root
                swagger.RoutePrefix = string.Empty;
            });

            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
            );

            app.UseHttpsRedirection();
            app.UseMvc();
            productsDbContext.Database.EnsureCreated();
        }
    }
}
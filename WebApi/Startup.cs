using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.IO;
using WebApi.Data;
using WebApi.Models;
using WebApi.Services;

namespace WebApi
{
    public class Startup
    {
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
               options.Conventions.Add(new VersionByNamespaceConvention());

           });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;

            });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();
            // Enable EF
            services.AddDbContext<ProductsDbContext>(option =>
                option.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            // Resolves the depencencies to always created
            services.AddScoped<IProduct, ProductRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider, ProductsDbContext productsDbContext)
        {
            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
            );
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            // Create and seed the db
            Seed(productsDbContext);        
            // Enable middleware to serve generate Swagger as a JSON endpoint
            app.UseSwagger();
            // Enable the middleware to serve swagger-ui
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                });
        }

        /// <summary>
        /// Seed the DB with the default values
        /// </summary>
        /// <param name="context"></param>
        private static void Seed(ProductsDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Add a new product
            using (StreamReader reader = new StreamReader("data.json"))
            {
                var json = reader.ReadToEnd();
                var products = JsonConvert.DeserializeObject<List<Product>>(json);
                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}
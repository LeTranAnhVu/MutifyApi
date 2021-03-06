using System;
using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Mutify.Middlewares;
using Mutify.Models;

namespace Mutify
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
            services.AddCors(o => o.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod();
            }));

            services.AddAutoMapper(typeof(AutoMapConfig));
            services.AddDbContext<MutifyContext>(
                opt =>
                    opt .UseSqlServer(Configuration.GetConnectionString("MutifyContext"))
                        // .UseLazyLoadingProxies()
                        .LogTo(Console.WriteLine, LogLevel.Information)
            );

            services.AddControllers()
                .AddNewtonsoftJson(x =>
                {
                    x.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Mutify", Version = "v1"}); });
        }

        private void CheckAndGenerateFolders()
        {
            var root = Constants.RootPath;
            var resourcePath = Path.Combine(root, Constants.Resource.ResourceFolder);
            var audioPath = Path.Combine(resourcePath, Constants.Resource.AudioFolder);

            if (!Directory.Exists(resourcePath))
            {
                Console.WriteLine($"Generate {Constants.Resource.ResourceFolder} folder");
                Directory.CreateDirectory(resourcePath);
            }

            if (!Directory.Exists(audioPath))
            {
                Console.WriteLine($"Generate {Constants.Resource.AudioFolder} folder");
                Directory.CreateDirectory(audioPath);
            }

            Console.WriteLine("Generated folders done!");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors();
                app.UseExceptionHandler(new ExceptionHandlerOptions
                {
                    ExceptionHandler = new ErrorHandlerMiddleware().Invoke
                });

                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mutify v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            CheckAndGenerateFolders();

        }
    }
}
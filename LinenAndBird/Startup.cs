using LinenAndBird.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinenAndBird
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
            // REGISTERING AN SERVICE // Hey ASP.Net, I want you to know that this configuration exists and if I ever ask you for a copy of it, I want you to know which copy of it to give me // Telling Asp.net how to build a thing //

            // services.AddTransient<IConfiguration>() -> create a new thing anytime someone asks for one //
            // services.AddScoped<IConfiguration>() -> Create a new thing once per http request //
            services.AddSingleton<IConfiguration>(Configuration); // -> Anytime someone asks for this thing, give them the same copy forever until the application stops //
            services.AddTransient<BirdRepository>(); // -> create a new thing anytime someone asks for one //

            // REGISTER EVERY REPOSITORY AS A TRANSIENT //
            services.AddTransient<OrdersRepository>(); // -> create a new thing anytime someone asks // 
            services.AddTransient<HatRepository>(); // -> create a new thing anytime someone asks  

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LinenAndBird", Version = "v1" });
            });
        }

        // REGISTERING AN SERVICE //

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LinenAndBird v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

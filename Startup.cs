using System;
using System.Collections.Generic;
using System.Linq;
using CommandoAPI.Models;
using CommandoAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CommandoAPI
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
            // Allow CORS during development
            services.AddCors(options =>  options.AddPolicy("MyPolicy", builder =>
            {
                builder
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddDbContext<CommandContext>(opt =>
               opt.UseSqlite(
                   Configuration.GetConnectionString("DefaultConnection")));

            // use FakeCommandItemService whenever ICommandItemService is requested
            //services.AddSingleton<ICommandItemService, FakeCommandItemService>();

            // always use the scoped lifecycle for services with EFCore
            services.AddScoped<ICommandItemService, CommandItemService>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // apply CORS for every request
            app.UseCors("MyPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

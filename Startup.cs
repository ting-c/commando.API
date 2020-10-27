using CommandoAPI.Models;
using CommandoAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

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
            services.AddCors(options => options.AddPolicy("MyPolicy", builder =>
            {
                builder
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));

            services.AddDbContext<CommandoDBContext>(opt =>
               opt.UseSqlite(
                   Configuration.GetConnectionString("DefaultConnection")));

            //use FakeCommandItemService whenever ICommandItemService is requested
            services.AddSingleton<ICommandItemService, FakeCommandItemService>();

            //always use the scoped lifecycle for services with EFCore

            services.AddScoped<ICommandItemService, CommandItemService>();
            services.AddControllers();

            //services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    // URL of our identity server
                    options.Authority = "https://localhost:5001";
                    // HTTPS required for the authority (defaults to true but disabled for development).
                    //options.RequireHttpsMetadata = false;
                    //// the name of this API - note: matches the API resource name configured in Commando.API config.cs
                    //options.Audience = "commandoapi";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            // adds an authorization policy to make sure the token is for scope 'commandoapi'
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "commandoapi");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            // apply CORS for every request
            app.UseCors("MyPolicy");

            //app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseMvc();

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Commando API is running.");
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization("ApiScope");
            });
        }
    }
}
 
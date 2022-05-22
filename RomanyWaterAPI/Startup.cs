using AquaWater.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RomanyWaterAPI.BusinessLogic.ConfigurationExtentions;
using RomanyWaterAPI.Data.Context;
using RomanyWaterAPI.Data.Seed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RomanyWaterAPI
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
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
             getAssembly => getAssembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            ));
            services.AddControllers();
            services.ConfigureServices();
            services.AddSwaggerConfiguration();
            services.ConfigurationIdentity();
            services.ConfigureAuthentication(Configuration);
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RomanyWaterAPI", Version = "v1" });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            RoleManager<IdentityRole> roleManager, UserManager<User> userManager, AppDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RomanyWaterAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            Seeder.Seed(roleManager, userManager, dbContext).GetAwaiter().GetResult();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

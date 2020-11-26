using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PaymentRetrieve.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Microsoft.OpenApi.Models;


namespace PaymentRetrieve
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            
            services.AddDbContext<PaymentContext>(opt => opt.UseSqlServer
                (Configuration.GetConnectionString("PaymentRetrieveConnection")));

            services.AddControllers().AddNewtonsoftJson(s => {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IPaymentRepository, SqlPaymentRepo>();

            //ADDED AFTER TUTORIAL
            services.AddSwaggerGen(c=> {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment API", Version = "v1" });
            });

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI( c=> {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

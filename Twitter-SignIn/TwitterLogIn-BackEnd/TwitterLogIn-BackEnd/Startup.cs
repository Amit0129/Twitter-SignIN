using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterLogIn_BackEnd.Context;
using TwitterLogIn_BackEnd.Data.Interface;
using TwitterLogIn_BackEnd.Data.Service;
using TwitterLogIn_BackEnd.Model;

namespace TwitterLogIn_BackEnd
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
            //CORS SERVICE CONFIGURATION:-
            services.AddCors(data =>
            {
                data.AddPolicy(
                    name: "AllowOrigin",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Twitter SignIn",
                    Description = "Sign In API's",
                    Contact = new OpenApiContact
                    {
                        Name = "Amit Kumar Nayak",
                        Email = "amitkumarnayak40@email.com",
                    },
                });
            });

            services.AddControllers();

            //HttpClient Service
            services.AddHttpClient("twitter");

            //Twitter Service
            services.Configure<TwitterSettings>(Configuration.GetSection("TwitterSettings"));

            services.AddDbContext<DataContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:TwitterDB"]));

            //Service and Interface ConfigurationService
            services.AddScoped<ITwitterAuthRepository, TwitterAuthRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowOrigin");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Twitter SignIn V1");
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

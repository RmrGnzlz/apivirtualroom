using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Domain.Contracts;
using Infrastructure.Base;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Models;
using Utils.ExternalServices;

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
            // services.AddCors();
            services.AddControllers();
            services.AddControllers().AddJsonOptions(opt => opt.JsonSerializerOptions.IgnoreNullValues = true);
            //"Host=ec2-52-0-155-79.compute-1.amazonaws.com;Port=5432;Database=darscg1heeg51g;Username=mgkshjrkovllrc;Password=2ee5ea5cf4c5c50565a2f1815cd591f109cce67f15c15c0def594b79d424b501;SSL=true;SslMode=Require"
            // Environment.SetEnvironmentVariable("DATABASE_URL", "postgres://yulyztrgxphqmt:eafe676c27a2e344a5f3d2ef498a22ac4b999e6e0582b5bb22eea3296edc4dcc@ec2-18-214-211-47.compute-1.amazonaws.com:5432/dc6hphq4tm6dr5");

            services.AddDbContext<SolumaticaContext>(opt => opt.UseNpgsql(HerokuStringPostgres(Environment.GetEnvironmentVariable("DATABASE_URL"))));
            // services.AddDbContext<SolumaticaContext>(opt => opt.UseMySql(Configuration.GetConnectionString("MySql")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDbContext, SolumaticaContext>();
            services.AddSingleton<IEmailService>(new GmailService(Configuration.GetSection("EMAIL").Value, Configuration.GetSection("PASSWORD_EMAIL").Value));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "yourdomain.com",
                    ValidAudience = "yourdomain.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("TOKEN").Value)),
                    ClockSkew = TimeSpan.Zero
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Api Rest - Solumatica",
                    Version = "v1"
                });
            });
        }

        private string HerokuStringPostgres(string connectionString)
        {
            connectionString.Replace("//", "");

            char[] delimiterChars = { '/', ':', '@', '?' };
            string[] strConn = connectionString.Split(delimiterChars);

            strConn = strConn.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            string user = strConn[1], pass = strConn[2], server = strConn[3], port = strConn[4], database = strConn[5];

            return $"Host={server};Port={port};Database={database};Username={user};Password={pass};SslMode=Require;Trust Server Certificate=true;";
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Rest - Solumatica");
                c.RoutePrefix = string.Empty;
            });

            app.UseCors(options => options.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
            );
        }
    }
}

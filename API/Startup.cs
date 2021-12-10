using API.Context;
using API.Repository.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API
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
            services.AddControllers();
            services.AddScoped<EmployeeRepository>();
            services.AddScoped<AccountRepository>();
            services.AddScoped<EducationRepository>();
            services.AddScoped<ProfilingRepository>();
            services.AddScoped<UniversityRepository>();
            services.AddScoped<RoleRepository>();
            services.AddScoped<AccountRoleRepository>();
            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddDbContext<MyContext>(options => options.UseSqlServer(Configuration.GetConnectionString("APIContext")));
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero

                };
            });
            services.AddCors(c =>
            {
                c.AddDefaultPolicy(
                 builder =>
                 {
                     builder.WithOrigins("https://localhost:44367")
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials();
                 });
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            });
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
            app.Use(async (context, next) => {
                await next();
                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    await context.Response.WriteAsync("Token Belum Dimasukkan, Akses ditolak");
                }
                else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                {
                    await context.Response.WriteAsync("Anda tidak diijinkan untuk menggunakan fitur ini, Akses ditolak");
                }
            });
            app.UseCors("AllowOrigin");
            app.UseAuthentication();
            app.UseAuthorization();
            /*app.UseCors(options => options.AllowAnyOrigin());*/
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }
    }
}

using ClassLibrary.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.Common.Extenstions;
using AutoMapper;
using ClassLibrary.Core.Mapper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using ClassLibrary.Core.Managers;
using ClassLibrary.Core.Managers.Interfaces;

namespace ToDoApp.API
{
    public class Startup
    {
        private MapperConfiguration _mapperConfiguration { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            #region MapperConfiguration
            Configuration = configuration;
            _mapperConfiguration = new MapperConfiguration(
                cfg =>
                cfg.AddProfile(new Mapping())
                );
            #endregion
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ToDoDBContext>();
            services.AddLogging();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<ICommonManager, CommonManager>();
            services.AddScoped<IRoleManager, RoleManager>();
            services.AddScoped<IToDoManager, ToDoManager>();
            services.AddSingleton(singltonMapper => _mapperConfiguration.CreateMapper());
            services.AddControllers();

            #region AddSwaggerGen
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDoApp.API", Version = "v1" });
                    c.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Description = "Plz insert Bearer JWT token, Example: 'Bearer{token}'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
        }
        );
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
            });
                }); 
            #endregion

            #region AddAuthentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ClockSkew = TimeSpan.Zero,
                            ValidIssuer = Configuration["Jwt:Issuer"], //test.com
                            ValidAudience = Configuration["Jwt:Issuer"],
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.
                                UTF8.
                                GetBytes(Configuration["Jwt:Key"])
                                )
                        };
                    });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDoApp.API v1"));
            }

            #region LoggerConfiguration
            Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
                    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger();
            #endregion

            #region ConfigureExceptionHandler
            app.ConfigureExceptionHandler(Log.Logger, env, typeof(Startup).Namespace);
            #endregion

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

using AutoMapper;
using HelloBuild.Application.Services;
using HelloBuild.Application.Services.Interfaces;
using HelloBuild.Domain.Models;
using HelloBuild.Infrastructure.Context;
using HelloBuild.Infrastructure.Mapper;
using HelloBuild.Infrastructure.Repositories;
using HelloBuild.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics;
using System.Text;

namespace HelloBuild.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _ = Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Trace.AutoFlush = true;
            Trace.Indent();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            IConfigurationSection sectionConfig = Configuration.GetSection("SectionConfigurationWebApi");
            _ = services.Configure<SectionConfigurationWebApi>(sectionConfig);
            SectionConfigurationWebApi configAppSetting = sectionConfig.Get<SectionConfigurationWebApi>();

            _ = services.AddCors(options => options.AddPolicy("AllowPolicySecureDomains", op =>
            {
                _ = op.AllowAnyOrigin()
                .WithOrigins(configAppSetting.SecureDomains)
                .AllowAnyHeader()
                .AllowCredentials()
                .AllowAnyMethod();
            }));

            AddSwaggerDocument(services);

            AddDbContext(services);
            AddControllersConfig(services);
            AddMapper(services);

            _ = services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Authentication:Issuer"],
                    ValidAudience = Configuration["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]))
                };
            });

            AddServices(services);
            AddRepositories(services);
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                _ = app.UseDeveloperExceptionPage();
            }

            _ = app.UseHttpsRedirection();
            _ = app.UseRouting();

            _ = app.UseAuthentication();
            _ = app.UseAuthorization();

            _ = app.UseCors("AllowPolicySecureDomains");
            _ = app.UseEndpoints(endpoints =>
            {
                _ = endpoints.MapControllers();
            });

            _ = app.UseOpenApi();
            _ = app.UseSwaggerUi3();
        }

        public void AddMapper(IServiceCollection services)
        {
            MapperConfiguration mapperConfig = new(mc =>
            {
                mc.AddProfile(new MapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            _ = services.AddSingleton(mapper);
        }

        public void AddDbContext(IServiceCollection services)
        {
            _ = services.AddDbContext<PersistenceContext>(opt =>
            {
                _ = opt.UseInMemoryDatabase("HelloBuildDB");
            });
        }

        public void AddControllersConfig(IServiceCollection services)
        {
            _ = services.AddControllers(mvcOpts =>
            {
            });
        }

        public void AddServices(IServiceCollection services)
        {
            _ = services.AddScoped<IUserService, UserService>();
            _ = services.AddScoped<IGithubService, GithubService>();
        }

        public void AddRepositories(IServiceCollection services)
        {
            _ = services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            _ = services.AddScoped<IUserRepository, UserRepository>();
        }

        public void AddSwaggerDocument(IServiceCollection services)
        {
            _ = services.AddSwaggerDocument(config =>
            {
                config.DocumentName = "Hello Build technical test";
                config.Title = "Andr�s Paniagua Hello Build Test";
                config.Version = "1.0";

                config.Description = "This is the technical test to enter Hello Build | Andr�s Paniagua";
            });
        }

        public void AddAuthentication(ref IServiceCollection services)
        {

        }

    }
}

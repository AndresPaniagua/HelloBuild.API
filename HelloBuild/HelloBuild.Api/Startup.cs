using HelloBuild.Application.Helpers;
using HelloBuild.Application.Helpers.Interfaces;
using HelloBuild.Application.Services;
using HelloBuild.Application.Services.Interfaces;
using AutoMapper;
using HelloBuild.Infrastructure.Repositories;
using HelloBuild.Infrastructure.Repositories.Interfaces;
using HelloBuild.Infrastructure.Context;
using HelloBuild.Infrastructure.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;

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
            AddSwaggerDocument(services);

            AddDbContext(services);
            AddControllersConfig(services);
            AddMapper(services);

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
                _ = opt.UseInMemoryDatabase("PruebaIngreso");
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
            _ = services.AddScoped<ILoanService, LoanService>();
            _ = services.AddScoped<IValidateUserHelper, ValidateUserHelper>();
        }

        public void AddRepositories(IServiceCollection services)
        {
            _ = services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            _ = services.AddScoped<ILoanRepository, LoanRepository>();
        }

        public void AddSwaggerDocument(IServiceCollection services)
        {
            _ = services.AddSwaggerDocument(config =>
            {
                config.DocumentName = "Ceiba technical test";
                config.Title = "Andrés Paniagua Ceiba Test";
                config.Version = "1.0";

                config.Description = "This is the technical test to enter Ceiba | Andrés Paniagua";
            });
        }

    }
}

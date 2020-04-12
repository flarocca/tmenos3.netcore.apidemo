using TMenos3.NetCore.ApiDemo.Infrastructure.API.Swagger;
using TMenos3.NetCore.ApiDemo.Infrastructure.Logging;
using EnsureThat;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using AutoMapper;
using TMenos3.NetCore.ApiDemo.Database.Extensions;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.API.Mvc
{
    public abstract class StartupBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;

        protected StartupBase(
            IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            Ensure.Any.IsNotNull(configuration, nameof(configuration));
            Ensure.Any.IsNotNull(hostEnvironment, nameof(hostEnvironment));

            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(GetAssemblies())
                    .AddDatabase(_configuration)
                    .AddCustomApi(_configuration, _hostEnvironment);

            ConfigureServices(services, _configuration, _hostEnvironment);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCorrelationContext()
               .UseSwaggerApiVersioning(_configuration);

            if (_hostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection()
               .UseRouting()
               .UseEndpoints(endpoints =>
               {
                   endpoints.MapControllers();
               });

            Configure(app, _configuration, _hostEnvironment);
        }

        protected virtual void ConfigureServices(
            IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment environment)
        {
        }

        protected virtual void Configure(
            IApplicationBuilder app,
            IConfiguration configuration,
            IHostEnvironment environment)
        {
        }

        protected abstract Assembly[] GetAssemblies();
    }
}

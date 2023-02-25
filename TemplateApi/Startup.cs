using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using TemplateApi.Application.Interfaces;
using TemplateApi.Application.UseCases;
using TemplateApi.Domain.Interfaces;
using TemplateApi.Infrastructure.Repositories.Cache;

namespace TemplateApi
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
                { 
                    Title = "TodoAPI", 
                    Version = "v1" 
                });
            });

            services.AddStackExchangeRedisCache(options =>            
            {
                options.Configuration = Configuration.GetConnectionString("ConnectionStrings:Redis");
            });

            services.AddScoped<IRedisRepository<object>, RedisRepository<object>>();
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<HttpClient>();
            services.AddScoped<IRealizarConsultaPorCepUseCase, RealizarConsultaPorCepUseCase>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo Api");
            });

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

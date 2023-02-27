using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using TemplateApi.Application.Interfaces;
using TemplateApi.Application.UseCases;
using TemplateApi.Domain.Interfaces;
using TemplateApi.Domain.Interfaces.Repository;
using TemplateApi.Infrastructure.Repositories;
using TemplateApi.Infrastructure.Repositories.Abstractions;
using TemplateApi.Infrastructure.Repositories.Cache;

namespace TemplateApi.CrossCutting
{
    public static class ContainerInjection
    {
        public static void InitContainer(this IServiceCollection services)
        {
            // Use cases
            services.AddScoped<IRealizarConsultaPorCepUseCase, RealizarConsultaPorCepUseCase>();

            // Client
            services.AddScoped<HttpClient>();
            
            // Repository
            services.AddScoped<IRedisRepository<object>, RedisRepository<object>>();
            services.AddScoped<IConsultaCepRepository, ConsultaCepRepository>();

            // Data
            services.AddScoped<IRepository<object>, Repository<object>>();
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TemplateApi.Infrastructure.Extensions
{
    public static class RedisExtensions
    {
        public static void InitRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("ConnectionStrings:Redis");
            });
        }
    }
}

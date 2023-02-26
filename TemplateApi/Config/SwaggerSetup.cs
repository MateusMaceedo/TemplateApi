using Microsoft.Extensions.DependencyInjection;

namespace TemplateApi.Config
{
    public static class SwaggerSetup
    {
        public static void SwaggerInit(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "TodoAPI",
                    Version = "v1"
                });
            });
        }
    }
}

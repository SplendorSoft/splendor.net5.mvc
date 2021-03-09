using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using splendor.net5.mvc.implementers;

namespace splendor.net5.mvc.tools.ioc
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDefaultMVCTracer(this IServiceCollection services)
        {
            services.AddSingleton<DefaultMVCTracer>();
        }

        public static void AddMVCWebContextAccesors(this IServiceCollection services)
        {
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
namespace UsefulDev.Core
{

    using Microsoft.Extensions.DependencyInjection;
    using UsefulDev.Core.Handlers;

    public static class Setup
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddSingleton<FileGeneratorHandler>();

            return services;
        }
    }
}

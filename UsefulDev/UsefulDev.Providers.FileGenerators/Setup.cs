namespace UsefulDev.Providers.FileGenerators
{

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IO;
    using UsefulDev.Core.Services;
    using UsefulDev.Providers.FileGenerators.Pdf;
    using UsefulDev.Providers.FileGenerators.Txt;

    public static class Setup
    {
        public static IServiceCollection AddFileGenerators(this IServiceCollection services, ApplicationProviders providers)
        {
            providers.AddFileGenerator<TxtGenerator>(services, TxtGenerator.FileExtension);
            providers.AddFileGenerator<PdfGenerator>(services, PdfGenerator.FileExtension);
            services.AddSingleton(serviceProvider => 
            { 
                return new RecyclableMemoryStreamManager(); 
            });
            return services;
        }
    }
}

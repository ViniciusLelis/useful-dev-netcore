namespace UsefulDev.Core.Services
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using UsefulDev.Core.ValueObjects;

    public class ApplicationProviders : IDisposable
    {
        private IServiceProvider _service;
        private IServiceScope _scope;
        private bool disposedValue;

        public IServiceProvider Services
        {
            get
            {
                var acessor = _service.GetService<IHttpContextAccessor>();
                if (acessor?.HttpContext != null)
                {
                    return acessor.HttpContext.RequestServices;
                }
                _scope ??= _service.CreateScope();
                return _scope.ServiceProvider;
            }
            set => _service = value;
        }

        /// <summary>
        /// File Generator providers
        /// </summary>
        private readonly ConcurrentDictionary<FileExtension, Type> FileGenerators = new ConcurrentDictionary<FileExtension, Type>();

        /// <summary>
        /// Get all File generators providers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IFileGeneratorService> GetFileGenerators()
        {
            return FileGenerators.Keys.Select(st => (IFileGeneratorService)Services.GetService(FileGenerators[st]));
        }

        /// <summary>
        /// Get a file generator based on the file extension
        /// </summary>
        public IFileGeneratorService GetFileGenerator(FileExtension fileExtension)
        {
            if (FileGenerators.TryGetValue(fileExtension, out var provider))
            {
                return (IFileGeneratorService)Services.GetService(provider);
            }
            return null;
        }

        public void AddFileGenerator(IServiceCollection services, Type sanitizerType, FileExtension fileExtension, object instance)
        {
            FileGenerators[fileExtension] = sanitizerType;
            services.Add(new ServiceDescriptor(sanitizerType, instance));
        }

        public void AddFileGenerator<TService>(IServiceCollection services, FileExtension fileExtension,
            Func<IServiceProvider, TService> factory = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped) where TService : IFileGeneratorService
        {
            FileGenerators[fileExtension] = typeof(TService);
            if (factory == null)
            {
                services.Add(new ServiceDescriptor(typeof(TService), typeof(TService), lifetime));
            }
            else
            {
                services.Add(new ServiceDescriptor(typeof(TService), sp => factory(sp), lifetime));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    FileGenerators.Clear();
                }

                _scope?.Dispose();
                disposedValue = true;
            }
        }

        ~ApplicationProviders()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

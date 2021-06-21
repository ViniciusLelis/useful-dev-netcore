namespace UsefulDev.Core.Handlers
{
    using Microsoft.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using UsefulDev.Core.Events;
    using UsefulDev.Core.Exceptions;
    using UsefulDev.Core.Services;

    public class FileGeneratorHandler
    {

        private readonly ApplicationProviders _appProviders;

        public FileGeneratorHandler(ApplicationProviders appProviders)
        {
            _appProviders = appProviders;
        }

        public async Task<RecyclableMemoryStream> Handle(FileGenerateEvent @event, CancellationToken ctx)
        {
            var fileGeneratorService = _appProviders.GetFileGenerator(@event.FileExtension) ?? throw new ProviderUnavailableException($"FileGeneration:{@event.FileExtension}");

            var stream = await fileGeneratorService.GenerateFile(@event.FileSize, @event.RandomWordsSet, ctx);

            return stream;
        }

    }
}

namespace UsefulDev.Core.Handlers
{
    using Microsoft.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using UsefulDev.Core.Events;
    using UsefulDev.Core.Exceptions;
    using UsefulDev.Core.Services;

    public class FileGeneratorHandler
    {

        private readonly ApplicationProviders _appProviders;
        private readonly string[] PREDEFINED_WORDS = new string[] { "Lorem", "dolor", "sit", "amet", "consectetur", "adipiscing", "elit", "Nullam", 
                                                                    "magna", "elit", "venenatis", "eget", "commodo", "nec", "pretium", "ac", "arcu", 
                                                                    "Sed", "luctus", "turpis", "eu", "elit", "blandit", "dignissim", "Vivamus", "dapibus", 
                                                                    "leo", "eu", "metus", "aliquet", "vitae", "ultrices", "orci", "viverra", "Nulla", 
                                                                    "vel", "consequat", "enim", "Nam", "eget", "quam", "neque", "Curabitur", "ac", "diam",
                                                                    "tincidunt", "pretium", "nisl", "eget", "eleifend", "risus", "Duis", "pretium", 
                                                                    "imperdiet", "elit", "eget", "molestie", "Proin", "molestie", "justo", "et", "volutpat", 
                                                                    "fermentum", "justo", "nulla", "fermentum", "nunc", "tincidunt", "blandit", "ligula", "ex", 
                                                                    "sit", "amet", "augue", "Integer", "ac", "libero", "eu", "libero", "ultrices", "dictum", "ac", 
                                                                    "luctus", "enim", "Aliquam", "tempor", "tincidunt", "nisl", "in", "efficitur", "Duis", "facilisis", 
                                                                    "fringilla", "lorem", "et", "facilisis", "sapien", "malesuada" };

        public FileGeneratorHandler(ApplicationProviders appProviders)
        {
            _appProviders = appProviders;
        }

        public async Task<RecyclableMemoryStream> Handle(FileGenerateEvent @event, CancellationToken ctx)
        {
            var fileGeneratorService = _appProviders.GetFileGenerator(@event.FileExtension) ?? throw new ProviderUnavailableException($"FileGeneration:{@event.FileExtension}");

            var words = true == @event.RandomWordsSet?.Any() ? @event.RandomWordsSet : PREDEFINED_WORDS;
            var stream = await fileGeneratorService.GenerateFile(@event.FileSize, words, ctx);

            return stream;
        }

    }
}

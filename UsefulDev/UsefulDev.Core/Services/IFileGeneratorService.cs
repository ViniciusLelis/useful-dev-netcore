namespace UsefulDev.Core.Services
{

    using Microsoft.IO;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UsefulDev.Core.ValueObjects;

    public interface IFileGeneratorService : IProvider
    {
        static FileExtension FileExtension { get; }

        Task<RecyclableMemoryStream> GenerateFile(int fileSize, IEnumerable<string> randomWordsSet, CancellationToken ctx);
    }
}

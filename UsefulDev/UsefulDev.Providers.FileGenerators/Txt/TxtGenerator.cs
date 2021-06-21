namespace UsefulDev.Providers.FileGenerators.Txt
{
    using Microsoft.IO;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using UsefulDev.Core.Services;
    using UsefulDev.Core.ValueObjects;
    using UsefulDev.Providers.FileGenerators.Exceptions;

    public class TxtGenerator : IFileGeneratorService
    {

        public static FileExtension FileExtension { get; } = FileExtension.txt;
        private readonly RecyclableMemoryStreamManager _manager;

        public TxtGenerator(RecyclableMemoryStreamManager manager)
        {
            _manager = manager;
        }

        public async Task<RecyclableMemoryStream> GenerateFile(int fileSize, string[] randomWordsSet, CancellationToken ctx)
        {
            if (true != randomWordsSet?.Any())
            {
                throw new EmptyRandomWordSetException(FileExtension);
            }

            var possibleWords = randomWordsSet.Distinct().ToList();
            int sumWordsSize = possibleWords.Select(word => word.Length + 1).Sum();

            int averageWordSize = sumWordsSize / possibleWords.Count;
            int wordCountBufferSize = (1024 * 1024) / averageWordSize;

            var wordsBuffer = new List<string>(wordCountBufferSize);

            var random = new Random();
            var stream = _manager.GetStream() as RecyclableMemoryStream;
            while (stream.Length < fileSize)
            {
                var randomIndex = random.Next(0, possibleWords.Count);
                var randomWord = possibleWords[randomIndex];

                wordsBuffer.Add(randomWord);

                if (wordsBuffer.Count >= wordCountBufferSize)
                {
                    var buffer = Encoding.UTF8.GetBytes(string.Join(" ", wordsBuffer));
                    await stream.WriteAsync(buffer, ctx);
                    wordsBuffer.Clear();
                }
            }

            stream.SetLength(fileSize);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
    }
}

namespace UsefulDev.Providers.FileGenerators.Pdf
{
    using Microsoft.IO;
    using Syncfusion.Drawing;
    using Syncfusion.Pdf;
    using Syncfusion.Pdf.Graphics;
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

    public class PdfGenerator : IFileGeneratorService
    {
        public static FileExtension FileExtension { get; } = FileExtension.pdf;
        private readonly RecyclableMemoryStreamManager _manager;

        private const short WORD_COUNT = 80;

        public PdfGenerator(RecyclableMemoryStreamManager manager)
        {
            _manager = manager;
        }

        public async Task<RecyclableMemoryStream> GenerateFile(int fileSize, string[] randomWordsSet, CancellationToken ctx)
        {
            if (true != randomWordsSet?.Any())
            {
                throw new EmptyRandomWordSetException(FileExtension);
            }

            PdfDocument pdfDocument;

            if (fileSize <= Constants.THRESHOLD_SIZE_BYTES)
            {
                pdfDocument = GeneratePdfWithoutContent();
            }
            else
            {
                pdfDocument = GeneratePdfWithContent(randomWordsSet);
            }

            var finalStream = _manager.GetStream() as RecyclableMemoryStream;
            pdfDocument.Save(finalStream);

            pdfDocument.Dispose();

            await FillStreamToFileSize(finalStream, fileSize, ctx);

            return finalStream;
        }

        private PdfDocument GeneratePdfWithContent(string[] randomWordsSet)
        {
            var pdfDocument = new PdfDocument
            {
                Compression = PdfCompressionLevel.AboveNormal
            };
            pdfDocument.DocumentInformation.Author = Guid.NewGuid().ToString().Substring(0, 2);

            var page = pdfDocument.Pages.Add();
            PdfGraphics graphics = page.Graphics;
            PdfFont font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12);

            var words = new List<string>(WORD_COUNT);

            var random = new Random();
            for (int i = 0; i < WORD_COUNT; i++)
            {
                var randomIndex = random.Next(0, randomWordsSet.Length);
                var randomWord = randomWordsSet[randomIndex];
                words.Add(randomWord);
            }

            var pdfContent = string.Join(' ', words);

            graphics.DrawString(pdfContent, font, PdfBrushes.Black, new RectangleF(0, 0, 500, 1000));

            return pdfDocument;
        }

        private PdfDocument GeneratePdfWithoutContent()
        {
            var pdfDocument = new PdfDocument
            {
                Compression = PdfCompressionLevel.AboveNormal
            };
            pdfDocument.DocumentInformation.Author = Guid.NewGuid().ToString().Substring(0, 2);

            return pdfDocument;
        }

        private async Task FillStreamToFileSize(RecyclableMemoryStream stream, int fileSize, CancellationToken ctx)
        {
            bool fillingNecessary = stream.Length < fileSize;

            stream.Seek(0, SeekOrigin.End);

            byte[] buffer = Encoding.UTF8.GetBytes("\r\n" + string.Concat(Enumerable.Repeat(" ", 1000)));
            while (stream.Length < fileSize)
            {
                await stream.WriteAsync(buffer, ctx);
            }

            if (fillingNecessary)
            {
                stream.SetLength(fileSize);
            }

            stream.Seek(0, SeekOrigin.Begin);
        }
    }
}

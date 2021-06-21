namespace UsefulDev.Providers.FileGenerators.Tests.Txt
{
    using Microsoft.IO;
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using UsefulDev.Providers.FileGenerators.Exceptions;
    using UsefulDev.Providers.FileGenerators.Txt;
    using Xunit;

    public class TxtGeneratorsTests
    {

        [Fact]
        public async Task Generate_Txt()
        {
            // Arrange
            var manager = new RecyclableMemoryStreamManager();

            var txtGenerator = new TxtGenerator(manager);

            var randomWordsSet = new string[] { "teste" };
            var expectedOutput = "teste teste tes";

            // Act
            using var stream = await txtGenerator.GenerateFile(15, randomWordsSet, CancellationToken.None);
            var outputBytes = stream.GetBuffer().Take((int)stream.Length).ToArray();
            var outputContent = Encoding.UTF8.GetString(outputBytes);

            // Assert
            Assert.NotNull(stream);
            Assert.Equal(15, stream.Length);
            Assert.Equal(expectedOutput, outputContent);
        }

        [Fact]
        public async Task Generate_Txt_Null_Word_Set()
        {
            // Arrange
            var manager = new RecyclableMemoryStreamManager();

            var txtGenerator = new TxtGenerator(manager);

            string[] randomWordsSet = null;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<EmptyRandomWordSetException>(async () => await txtGenerator.GenerateFile(15, randomWordsSet, CancellationToken.None));
            Assert.Equal(string.Format(Resources.FileGenerators.EmptyRandomWordSetException, "txt"), exception.Message);
        }

        [Fact]
        public async Task Generate_Txt_Empty_Word_Set()
        {
            // Arrange
            var manager = new RecyclableMemoryStreamManager();

            var txtGenerator = new TxtGenerator(manager);

            string[] randomWordsSet = Array.Empty<string>();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<EmptyRandomWordSetException>(async () => await txtGenerator.GenerateFile(15, randomWordsSet, CancellationToken.None));
            Assert.Equal(string.Format(Resources.FileGenerators.EmptyRandomWordSetException, "txt"), exception.Message);
        }
    }
}

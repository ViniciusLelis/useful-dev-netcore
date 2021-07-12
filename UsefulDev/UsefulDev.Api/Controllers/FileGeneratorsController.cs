namespace UsefulDev.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using UsefulDev.Core;
    using UsefulDev.Core.Events;
    using UsefulDev.Core.Handlers;
    using UsefulDev.Core.ValueObjects;

    [ApiController]
    [Route("api/v1/file-generators")]
    public class FileGeneratorsController : ControllerBase
    {
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(500)]
        [HttpGet]
        public async Task<FileResult> GenerateFile([FromServices] FileGeneratorHandler handler, 
            [FromQuery] FileExtension fileExtension, 
            [FromQuery] string fileName,
            [FromQuery][Range(Constants.MIN_GENERATE_FILE_SIZE_BYTES, Constants.MAX_GENERATE_FILE_SIZE_BYTES)] int fileSizeBytes,
            [FromQuery] string[] randomWordsSet, CancellationToken ctx)
        {
            var words = randomWordsSet?.Where(word => !string.IsNullOrWhiteSpace(word)).SelectMany(word => word.Split(",")).Select(word => word.Trim());
            var @event = new FileGenerateEvent(fileExtension, fileSizeBytes, words);
            var stream = await handler.Handle(@event, ctx);
            
            var fileNameFormatted = $"{fileName}.{Enum.GetName(typeof(FileExtension), fileExtension)}";
            var contentType = MimeTypes.GetMimeType(fileNameFormatted);

            return File(stream, contentType, fileNameFormatted);
        }

    }
}

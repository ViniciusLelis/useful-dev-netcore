namespace UsefulDev.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using UsefulDev.Api.Models;
    using UsefulDev.Core.Events;
    using UsefulDev.Core.Handlers;
    using UsefulDev.Core.ValueObjects;

    [ApiController]
    [Route("file-generators")]
    public class FileGeneratorsController : ControllerBase
    {
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(500)]
        [HttpPost]
        public async Task<FileResult> GenerateFile([FromServices] FileGeneratorHandler handler, [FromBody] FileGeneratorModel model, CancellationToken ctx)
        {
            var @event = new FileGenerateEvent(model.FileExtension, model.FileSizeBytes, model.RandomWordsSet);
            var stream = await handler.Handle(@event, ctx);
            
            var fileNameFormatted = $"{model.FileName}.{Enum.GetName(typeof(FileExtension), model.FileExtension)}";
            var contentType = MimeTypes.GetMimeType(fileNameFormatted);

            return File(stream, contentType, fileNameFormatted);
        }

    }
}

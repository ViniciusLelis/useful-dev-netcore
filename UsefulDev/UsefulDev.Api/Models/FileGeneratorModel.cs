namespace UsefulDev.Api.Models
{
    using System.ComponentModel.DataAnnotations;
    using UsefulDev.Core;
    using UsefulDev.Core.ValueObjects;

    /// <summary>
    /// Model for generating a file
    /// </summary>
    public class FileGeneratorModel
    {
        /// <summary>
        /// File Extension for desired file
        /// </summary>
        public FileExtension FileExtension { get; set; }

        /// <summary>
        /// File name for desired file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// File size for desired file
        /// </summary>
        [Required]
        [Range(Constants.MIN_GENERATE_FILE_SIZE_BYTES, Constants.MAX_GENERATE_FILE_SIZE_BYTES)]
        public int FileSizeBytes { get; set; }

        /// <summary>
        /// Set of random words to use when generating a file (Can be null or empty if the api is supposed to generate freely)
        /// </summary>
        public string[] RandomWordsSet { get; set; }

    }
}

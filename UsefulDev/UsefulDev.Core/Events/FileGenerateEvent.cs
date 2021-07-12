namespace UsefulDev.Core.Events
{
    using System.Collections.Generic;
    using UsefulDev.Core.ValueObjects;
    
    /// <summary>
    /// Event for generating a downloadable file
    /// </summary>
    public struct FileGenerateEvent
    {

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public FileGenerateEvent(FileExtension fileExtension, int fileSize, IEnumerable<string> randomWordsSet)
        {
            FileExtension = fileExtension;
            FileSize = fileSize;
            RandomWordsSet = randomWordsSet;
        }

        /// <summary>
        /// File Extension for desired file
        /// </summary>
        public FileExtension FileExtension { get; set; }

        /// <summary>
        /// File size for desired file
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        /// Set of random words to use when generating a file (Can be null or empty if the api is supposed to generate freely)
        /// </summary>
        public IEnumerable<string> RandomWordsSet { get; set; }
    }
}

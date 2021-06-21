namespace UsefulDev.Providers.FileGenerators.Exceptions
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using UsefulDev.Core.ValueObjects;

    [Serializable, ExcludeFromCodeCoverage]
    public class EmptyRandomWordSetException : Exception
    {

        public EmptyRandomWordSetException(FileExtension fileExtension) : base(string.Format(Resources.FileGenerators.EmptyRandomWordSetException, Enum.GetName(typeof(FileExtension), fileExtension)))
           => FileExtension = fileExtension;

        protected EmptyRandomWordSetException(SerializationInfo info, StreamingContext context) : base(info, context)
            => FileExtension = (FileExtension)info.GetValue("FileExtension", typeof(FileExtension));

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("FileExtension", FileExtension);
        }

        public FileExtension FileExtension { get; set; }

    }
}

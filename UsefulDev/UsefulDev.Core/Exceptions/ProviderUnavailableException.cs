namespace UsefulDev.Core.Exceptions
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [Serializable, ExcludeFromCodeCoverage]
    public class ProviderUnavailableException : Exception
    {

        public ProviderUnavailableException(string providerName) : base(string.Format(Resources.Files.ProviderUnavailableException, providerName))
           => ProviderName = providerName;

        protected ProviderUnavailableException(SerializationInfo info, StreamingContext context) : base(info, context)
            => ProviderName = (string)info.GetValue("ProviderName", typeof(string));

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ProviderName", ProviderName);
        }

        public string ProviderName { get; set; }
    }
}

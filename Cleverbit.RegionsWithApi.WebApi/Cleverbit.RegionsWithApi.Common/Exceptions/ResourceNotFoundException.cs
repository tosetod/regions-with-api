using System.Runtime.Serialization;

namespace Cleverbit.RegionsWithApi.Common.Exceptions
{
    [Serializable]
    public class ResourceNotFoundException : Exception
    {
        public string ResourceName { get; set; }
        public int ResourceId { get; set; }

        public ResourceNotFoundException() : base()
        {
        }

        public ResourceNotFoundException(string message) : base(message)
        {
        }

        public ResourceNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ResourceNotFoundException(string resourceName, int id)
            : this($"{resourceName} with Id: {id} does not exist")
        {
            ResourceName = resourceName;
            ResourceId = id;
        }

        protected ResourceNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {
            ResourceName = serializationInfo.GetString(nameof(ResourceName)) ?? string.Empty;
            ResourceId = (int)(serializationInfo.GetValue(nameof(ResourceId), typeof(int)) ?? default(int));
        }
    }
}

using System.Runtime.Serialization;

namespace Cleverbit.RegionsWithApi.Common.Exceptions
{
    [Serializable]
    public class CoreException : Exception
    {
        public CoreException() : base()
        {
        }

        public CoreException(string message) : base(message)
        {
        }

        public CoreException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CoreException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {
        }
    }
}

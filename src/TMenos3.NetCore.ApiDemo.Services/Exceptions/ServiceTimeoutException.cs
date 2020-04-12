using System;
using System.Runtime.Serialization;

namespace TMenos3.NetCore.ApiDemo.Services.Exceptions
{
    public class ServiceTimeoutException : Exception
    {
        public ServiceTimeoutException()
        {
        }

        public ServiceTimeoutException(string message) : base(message)
        {
        }

        public ServiceTimeoutException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ServiceTimeoutException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

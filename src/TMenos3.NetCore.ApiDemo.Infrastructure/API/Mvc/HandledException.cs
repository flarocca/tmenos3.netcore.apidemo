using System;
using System.Runtime.Serialization;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.API.Mvc
{
    [Serializable]
    public class HandledException : Exception
    {
        public HandledException()
        {
        }

        public HandledException(string message)
            : base(message)
        {
        }

        public HandledException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected HandledException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
